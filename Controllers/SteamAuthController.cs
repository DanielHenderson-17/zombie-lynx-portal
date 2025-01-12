using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZombieLynxPortal.Data;
using ZombieLynxPortal.Models;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ZombieLynxPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SteamAuthController : ControllerBase
    {
        private readonly ZombieLynxPortalDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<SteamAuthController> _logger;
        private readonly IConfiguration _configuration;

        public SteamAuthController(
            ZombieLynxPortalDbContext dbContext,
            UserManager<IdentityUser> userManager,
            ILogger<SteamAuthController> logger,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        // ✅ Health check
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation("Ping endpoint hit.");
            return Ok("SteamAuthController is active.");
        }

        // 🔑 Initiates Steam login for linking
        [HttpGet("login")]
        [Authorize]
        public IActionResult Login()
        {
            _logger.LogInformation("Steam linking initiated.");

            var aspNetUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(aspNetUserId))
            {
                _logger.LogError("No logged-in ASP.NET user.");
                return Unauthorized("User must be logged in.");
            }

            var properties = new AuthenticationProperties
            {
                RedirectUri = "/api/SteamAuth/link-steam"
            };

            properties.Items["AspNetUserId"] = aspNetUserId;

            return Challenge(properties, "Steam");
        }

        // 🔄 Callback for linking Steam account
        [HttpGet("link-steam")]
        [Authorize(AuthenticationSchemes = "Steam")]
        public async Task<IActionResult> LinkSteam()
        {
            _logger.LogInformation("Steam link callback triggered.");

            var steamOpenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(steamOpenId))
            {
                _logger.LogError("Steam ID not found in the callback.");
                return Unauthorized("Steam linking failed.");
            }

            var steamId = steamOpenId.Replace("https://steamcommunity.com/openid/id/", "");
            _logger.LogInformation($"Extracted Steam ID: {steamId}");

            var result = await HttpContext.AuthenticateAsync("Steam");
            if (!result.Succeeded || !result.Properties.Items.TryGetValue("AspNetUserId", out var aspNetUserId))
            {
                _logger.LogError("No logged-in ASP.NET user found.");
                return Unauthorized("User must be logged in.");
            }

            _logger.LogInformation($"Logged-in ASP.NET User ID: {aspNetUserId}");

            var userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.IdentityUserId == aspNetUserId);
            if (userProfile == null)
            {
                _logger.LogError($"UserProfile not found for IdentityUserId: {aspNetUserId}");
                return NotFound("User profile not found.");
            }

            var (steamName, steamImgUrl) = await GetSteamProfileAsync(steamId);
            if (steamName == null)
            {
                _logger.LogError("Failed to fetch Steam profile data.");
                return StatusCode(500, "Failed to fetch Steam data.");
            }

            var existingLink = _dbContext.ZLGMembers.SingleOrDefault(z => z.IdentityUserId == aspNetUserId);

            if (existingLink == null)
            {
                var newLink = new ZLGMember
                {
                    IdentityUserId = aspNetUserId,
                    UserProfileId = userProfile.Id,
                    SteamId = steamId,
                    SteamName = steamName,
                    SteamImgUrl = steamImgUrl,

                    // ✅ Keep optional fields null until linked
                    DiscordId = null,
                    DiscordName = null,
                    DiscordImgUrl = null,
                    EosId = null,
                    EpicName = null,
                    EpicImgUrl = null
                };

                _dbContext.ZLGMembers.Add(newLink);
                _logger.LogInformation($"New Steam account linked for user {aspNetUserId}.");
            }
            else
            {
                existingLink.SteamId = steamId;
                existingLink.SteamName = steamName;
                existingLink.SteamImgUrl = steamImgUrl;
                _logger.LogInformation($"Updated Steam account link for user {aspNetUserId}.");
            }

            await _dbContext.SaveChangesAsync();

            // ✅ Return the linked Steam account data to the frontend
            return Ok(new
            {
                steamId = steamId,
                steamName = steamName,
                steamImgUrl = steamImgUrl
            });
        }

        // 🔥 Helper method to fetch Steam profile data
        private async Task<(string? SteamName, string? SteamImgUrl)> GetSteamProfileAsync(string steamId)
        {
            var steamApiKey = _configuration["Authentication:Steam:ApiKey"];
            var url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={steamApiKey}&steamids={steamId}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to fetch Steam profile for Steam ID: {steamId}");
                return (null, null);
            }

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(content);
            var player = data.RootElement
                             .GetProperty("response")
                             .GetProperty("players")
                             .EnumerateArray()
                             .FirstOrDefault();

            var steamName = player.GetProperty("personaname").GetString();
            var steamImgUrl = player.GetProperty("avatarfull").GetString();

            return (steamName, steamImgUrl);
        }

        // ✅ Fetch linked Steam account for the logged-in user
        [HttpGet("linked")]
        [Authorize]
        public IActionResult GetLinkedSteamAccount()
        {
            var aspNetUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(aspNetUserId))
            {
                return Unauthorized("User must be logged in.");
            }

            var linkedAccount = _dbContext.ZLGMembers
                .Where(z => z.IdentityUserId == aspNetUserId && z.SteamId != null)
                .Select(z => new
                {
                    steamId = z.SteamId,
                    steamName = z.SteamName,
                    steamImgUrl = z.SteamImgUrl
                })
                .FirstOrDefault();

            if (linkedAccount == null)
            {
                return NotFound("No Steam account linked.");
            }

            return Ok(linkedAccount);
        }
    }
}
