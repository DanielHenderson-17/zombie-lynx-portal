using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZombieLynxPortal.Data;
using ZombieLynxPortal.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ZombieLynxPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SteamAuthController : ControllerBase
    {
        private readonly ZombieLynxPortalDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public SteamAuthController(
            ZombieLynxPortalDbContext dbContext,
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
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
            if (string.IsNullOrEmpty(aspNetUserId)) return Unauthorized("User must be logged in.");

            var properties = new AuthenticationProperties { RedirectUri = "/api/SteamAuth/link-steam" };
            properties.Items["AspNetUserId"] = aspNetUserId;

            return Challenge(properties, "Steam");
        }

        [HttpGet("link-steam")]
        [Authorize(AuthenticationSchemes = "Steam")]
        public async Task<IActionResult> LinkSteam()
        {
            var steamOpenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(steamOpenId)) return Unauthorized("Steam linking failed.");

            var steamId = steamOpenId.Replace("https://steamcommunity.com/openid/id/", "");
            var result = await HttpContext.AuthenticateAsync("Steam");

            if (!result.Succeeded || !result.Properties.Items.TryGetValue("AspNetUserId", out var aspNetUserId))
                return Unauthorized("User must be logged in.");

            var userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.IdentityUserId == aspNetUserId);
            if (userProfile == null) return NotFound("User profile not found.");

            var (steamName, steamImgUrl) = await GetSteamProfileAsync(steamId);
            if (steamName == null) return StatusCode(500, "Failed to fetch Steam data.");

            var existingLink = _dbContext.ZLGMembers.SingleOrDefault(z => z.IdentityUserId == aspNetUserId);

            if (existingLink == null)
            {
                _dbContext.ZLGMembers.Add(new ZLGMember
                {
                    IdentityUserId = aspNetUserId,
                    UserProfileId = userProfile.Id,
                    SteamId = steamId,
                    SteamName = steamName,
                    SteamImgUrl = steamImgUrl
                });
            }
            else
            {
                existingLink.SteamId = steamId;
                existingLink.SteamName = steamName;
                existingLink.SteamImgUrl = steamImgUrl;
            }

            await _dbContext.SaveChangesAsync();

            return Content(@"<script>
                window.opener.postMessage('steamLinked', '*');
                window.close();
            </script>", "text/html");
        }

        private async Task<(string? SteamName, string? SteamImgUrl)> GetSteamProfileAsync(string steamId)
        {
            var steamApiKey = _configuration["Authentication:Steam:ApiKey"];
            var url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={steamApiKey}&steamids={steamId}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return (null, null);

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(content);
            var player = data.RootElement.GetProperty("response").GetProperty("players").EnumerateArray().FirstOrDefault();

            var steamName = player.GetProperty("personaname").GetString();
            var steamImgUrl = player.GetProperty("avatarfull").GetString();

            return (steamName, steamImgUrl);
        }

        [HttpPut("unlink")]
        [Authorize]
        public async Task<IActionResult> UnlinkSteamAccount()
        {
            var aspNetUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingLink = await _dbContext.ZLGMembers.FirstOrDefaultAsync(z => z.IdentityUserId == aspNetUserId);

            if (existingLink == null) return NotFound("No Steam account linked to unlink.");

            existingLink.SteamId = null;
            existingLink.SteamName = null;
            existingLink.SteamImgUrl = null;

            await _dbContext.SaveChangesAsync();

            return Ok("Steam account has been unlinked.");
        }

        [HttpGet("linked")]
        [Authorize]
        public async Task<IActionResult> GetLinkedSteamAccount()
        {
            var authResult = await HttpContext.AuthenticateAsync();
            if (!authResult.Succeeded)
            {
                return Unauthorized("Session not authenticated.");
            }

            var aspNetUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(aspNetUserId))
                return Unauthorized("User not authenticated.");

            var linkedAccount = await _dbContext.ZLGMembers
                .Where(z => z.IdentityUserId == aspNetUserId && z.SteamId != null)
                .Select(z => new
                {
                    steamId = z.SteamId,
                    steamName = z.SteamName,
                    steamImgUrl = z.SteamImgUrl
                })
                .FirstOrDefaultAsync();

            return linkedAccount == null ? NotFound("No Steam account linked.") : Ok(linkedAccount);
        }


        [HttpGet("all-steam-users")]
        [Authorize]
        public async Task<IActionResult> GetAllSteamUsers()
        {
            var steamUsers = await _dbContext.ZLGMembers
                .Where(z => z.SteamId != null)
                .Select(z => new
                {
                    z.IdentityUserId,
                    z.SteamId,
                    z.SteamName,
                    z.SteamImgUrl
                })
                .ToListAsync();

            return Ok(steamUsers);
        }

    }
}
