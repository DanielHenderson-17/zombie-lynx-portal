using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using ZombieLynxPortal.Data;
using ZombieLynxPortal.Models;

namespace ZombieLynxPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SteamAuthController : ControllerBase
    {
        private readonly ZombieLynxPortalDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public SteamAuthController(ZombieLynxPortalDbContext dbContext, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok("SteamAuthController is active.");

        [HttpGet("login")]
        [Authorize]
        public IActionResult Login()
        {
            var aspNetUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return string.IsNullOrEmpty(aspNetUserId)
                ? Unauthorized("User must be logged in.")
                : Challenge(new AuthenticationProperties { RedirectUri = "/api/SteamAuth/link-steam", Items = { ["AspNetUserId"] = aspNetUserId } }, "Steam");
        }

        [HttpGet("link-steam")]
        [Authorize(AuthenticationSchemes = "Steam")]
        public async Task<IActionResult> LinkSteam()
        {
            var steamId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value?.Replace("https://steamcommunity.com/openid/id/", "");
            if (string.IsNullOrEmpty(steamId)) return Unauthorized("Steam linking failed.");

            var result = await HttpContext.AuthenticateAsync("Steam");
            if (!result.Succeeded || !result.Properties.Items.TryGetValue("AspNetUserId", out var aspNetUserId)) return Unauthorized("User must be logged in.");

            var userProfile = await _dbContext.UserProfiles.SingleOrDefaultAsync(up => up.IdentityUserId == aspNetUserId);
            if (userProfile == null) return NotFound("User profile not found.");

            var (steamName, steamImgUrl) = await GetSteamProfileAsync(steamId);
            if (steamName == null) return StatusCode(500, "Failed to fetch Steam data.");

            var existingLink = await _dbContext.ZLGMembers.SingleOrDefaultAsync(z => z.IdentityUserId == aspNetUserId);

            if (existingLink == null)
                _dbContext.ZLGMembers.Add(new ZLGMember { IdentityUserId = aspNetUserId, UserProfileId = userProfile.Id, SteamId = steamId, SteamName = steamName, SteamImgUrl = steamImgUrl });
            else
            {
                existingLink.SteamId = steamId;
                existingLink.SteamName = steamName;
                existingLink.SteamImgUrl = steamImgUrl;
            }

            await _dbContext.SaveChangesAsync();
            var jwtToken = GenerateJwtToken(aspNetUserId);

            return Content($@"<script>window.opener.postMessage({{ type: 'steamLinked', token: '{jwtToken}' }}, '*');window.close();</script>", "text/html");
        }

        [HttpPut("unlink")]
        [Authorize(AuthenticationSchemes = "SteamJwt")]
        public async Task<IActionResult> UnlinkSteamAccount()
        {
            var aspNetUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingLink = await _dbContext.ZLGMembers.FirstOrDefaultAsync(z => z.IdentityUserId == aspNetUserId);

            if (existingLink == null) return NotFound("No Steam account linked to unlink.");

            existingLink.SteamId = existingLink.SteamName = existingLink.SteamImgUrl = null;
            await _dbContext.SaveChangesAsync();

            return Ok("Steam account has been unlinked.");
        }

        [HttpGet("linked")]
        [Authorize(AuthenticationSchemes = "SteamJwt")]
        public async Task<IActionResult> GetLinkedSteamAccount()
        {
            var aspNetUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var linkedAccount = string.IsNullOrEmpty(aspNetUserId) ? null : await _dbContext.ZLGMembers
                .Where(z => z.IdentityUserId == aspNetUserId && z.SteamId != null)
                .Select(z => new { z.SteamId, z.SteamName, z.SteamImgUrl })
                .FirstOrDefaultAsync();

            return linkedAccount == null ? NotFound("No Steam account linked.") : Ok(linkedAccount);
        }

        private async Task<(string? SteamName, string? SteamImgUrl)> GetSteamProfileAsync(string steamId)
        {
            var url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={_configuration["Authentication:Steam:ApiKey"]}&steamids={steamId}";
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return (null, null);

            var player = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement.GetProperty("response").GetProperty("players").EnumerateArray().FirstOrDefault();
            return (player.GetProperty("personaname").GetString(), player.GetProperty("avatarfull").GetString());
        }

        private string GenerateJwtToken(string aspNetUserId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, aspNetUserId) };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"])), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
