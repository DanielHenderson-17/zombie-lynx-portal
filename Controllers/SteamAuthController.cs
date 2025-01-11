using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZombieLynxPortal.Data;
using ZombieLynxPortal.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation("Ping endpoint hit.");
            return Ok("SteamAuthController is active.");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            _logger.LogInformation("Steam login initiated.");

            var properties = new AuthenticationProperties
            {
                RedirectUri = "https://profile.zlg.gg:5001/api/SteamAuth/callback"
            };

            return Challenge(properties, "Steam");
        }

        [HttpGet("callback")]
        [Authorize(AuthenticationSchemes = "Steam")]
        public async Task<IActionResult> Callback()
        {
            _logger.LogInformation("Steam callback triggered.");

            var steamOpenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(steamOpenId))
            {
                _logger.LogError("Steam ID not found in the callback.");
                return Unauthorized("Steam login failed.");
            }

            var steamId = steamOpenId.Replace("https://steamcommunity.com/openid/id/", "");
            _logger.LogInformation($"Extracted Steam ID: {steamId}");

            // Check if the IdentityUser exists
            var user = await _userManager.FindByNameAsync($"steam_{steamId}");
            if (user == null)
            {
                var displayName = await GetSteamDisplayNameAsync(steamId) ?? "SteamUser";

                user = new IdentityUser
                {
                    UserName = $"steam_{steamId}",
                    Email = $"steam_{steamId}@steam.com"
                };

                var result = await _userManager.CreateAsync(user, Guid.NewGuid().ToString());

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Error creating user: {error.Code} - {error.Description}");
                    }
                    return StatusCode(500, "Failed to create user.");
                }

                _logger.LogInformation($"User {user.UserName} created successfully.");
            }

            // Check if UserProfile exists
            var profile = _dbContext.UserProfiles.SingleOrDefault(up => up.IdentityUserId == user.Id);
            if (profile == null)
            {
                var displayName = await GetSteamDisplayNameAsync(steamId) ?? "SteamUser";

                profile = new UserProfile
                {
                    IdentityUserId = user.Id,
                    FirstName = displayName,
                    LastName = "User",
                    Address = "N/A"
                };

                _dbContext.UserProfiles.Add(profile);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("User profile created successfully.");
            }

            // ✅ Check if ZLGMember exists
            var zlgMember = _dbContext.ZLGMembers.SingleOrDefault(m => m.IdentityUserId == user.Id);
            if (zlgMember == null)
            {
                var steamName = await GetSteamDisplayNameAsync(steamId) ?? "SteamUser";

                zlgMember = new ZLGMember
                {
                    IdentityUserId = user.Id,
                    UserProfileId = profile.Id,
                    SteamId = steamId,
                    SteamName = steamName,
                    EosId = "",  // Placeholder for future Epic integration
                    EpicName = "",
                    DiscordId = "",  // Placeholder for future Discord integration
                    DiscordName = ""
                };

                _dbContext.ZLGMembers.Add(zlgMember);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("ZLGMember entry created successfully.");
            }

            // Sign the user in
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            _logger.LogInformation($"User {user.UserName} signed in successfully.");

            return Redirect("http://localhost:5176/login-success");
        }

        [HttpGet("logout")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("User logged out successfully.");
            return Ok("Logged out successfully.");
        }

        // 🔥 Helper method to fetch Steam display name
        private async Task<string?> GetSteamDisplayNameAsync(string steamId)
        {
            var steamApiKey = _configuration["Authentication:Steam:ApiKey"];
            var url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={steamApiKey}&steamids={steamId}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to fetch Steam display name for Steam ID: {steamId}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(content);
            var player = data.RootElement
                             .GetProperty("response")
                             .GetProperty("players")
                             .EnumerateArray()
                             .FirstOrDefault();

            return player.GetProperty("personaname").GetString();
        }
    }
}
