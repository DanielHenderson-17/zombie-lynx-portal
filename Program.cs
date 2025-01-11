using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using ZombieLynxPortal.Data;
using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Swagger/OpenAPI setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS to allow frontend access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder.WithOrigins("https://zlg.gg", "https://profile.zlg.gg", "http://localhost:5176")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());  // 🔑 Must allow credentials
});

// 🔑 Configure Cookie Behavior for Cross-Origin Authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;  // ✅ Allow cross-origin cookies
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // ✅ Force HTTPS
    options.Cookie.HttpOnly = true;  // ✅ Protect against XSS
});

// Authentication setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "ZombieLynxPortalLoginCookie";
    options.Cookie.SameSite = SameSiteMode.None;  // ✅ Matches the global setting
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
})
.AddSteam(options =>
{
    options.ApplicationKey = builder.Configuration["Authentication:Steam:ApiKey"];
    options.CallbackPath = "/signin-steam";
    options.Events.OnRemoteFailure = context =>
    {
        context.Response.Redirect("/error?message=" + context.Failure?.Message);
        context.HandleResponse();
        return Task.CompletedTask;
    };
})
.AddOAuth("Discord", options =>
{
    options.ClientId = builder.Configuration["Authentication:Discord:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Discord:ClientSecret"];
    options.CallbackPath = "/signin-discord";

    options.AuthorizationEndpoint = "https://discord.com/api/oauth2/authorize";
    options.TokenEndpoint = "https://discord.com/api/oauth2/token";
    options.UserInformationEndpoint = "https://discord.com/api/users/@me";

    options.SaveTokens = true;
    options.Scope.Add("identify");
    options.Scope.Add("email");

    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            var response = await context.Backchannel.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;

            context.Identity.AddClaim(new System.Security.Claims.Claim("DiscordId", user.GetProperty("id").GetString()));
            context.Identity.AddClaim(new System.Security.Claims.Claim("Username", user.GetProperty("username").GetString()));
            context.Identity.AddClaim(new System.Security.Claims.Claim("Email", user.GetProperty("email").GetString() ?? ""));
        }
    };
});

// Identity setup
builder.Services.AddIdentityCore<IdentityUser>(config =>
{
    config.Password.RequireDigit = false;
    config.Password.RequiredLength = 8;
    config.Password.RequireLowercase = false;
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequireUppercase = false;
    config.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ZombieLynxPortalDbContext>();

// PostgreSQL database connection
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddNpgsql<ZombieLynxPortalDbContext>(builder.Configuration["ZombieLynxPortalDbConnectionString"]);

var app = builder.Build();

// Enable Forwarded Headers for reverse proxy setups (important for HTTPS and correct IP detection)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Swagger setup for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enforce HTTPS
app.UseHttpsRedirection();

// Enable routing
app.UseRouting();

// Enable CORS before authentication
app.UseCors("AllowFrontend");

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controller routes
app.MapControllers();

app.Run();
