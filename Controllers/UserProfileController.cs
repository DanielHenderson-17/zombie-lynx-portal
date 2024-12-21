using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZombieLynxPortal.Data;
using ZombieLynxPortal.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using ZombieLynxPortal.Models;
using Microsoft.AspNetCore.Identity;

namespace ZombieLynxPortal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly ZombieLynxPortalDbContext _dbContext;

    public UserProfileController(ZombieLynxPortalDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        // Get the currently logged-in user's IdentityUserId
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        // Fetch only the profile for the logged-in user
        var userProfile = _dbContext.UserProfiles
            .Include(up => up.IdentityUser)
            .Where(up => up.IdentityUserId == userId)
            .Select(up => new UserProfileDTO
            {
                Id = up.Id,
                FirstName = up.FirstName,
                LastName = up.LastName,
                Address = up.Address,
                IdentityUserId = up.IdentityUserId,
                Email = up.IdentityUser.Email,
                UserName = up.IdentityUser.UserName
            })
            .SingleOrDefault();

        if (userProfile == null)
        {
            return NotFound();
        }

        return Ok(userProfile);
    }

    [HttpGet("withroles")]
    [Authorize(Roles = "Admin")] // Restrict access to administrators only
    public IActionResult GetWithRoles()
    {
        var profilesWithRoles = _dbContext.UserProfiles
            .Include(up => up.IdentityUser)
            .Select(up => new UserProfileDTO
            {
                Id = up.Id,
                FirstName = up.FirstName,
                LastName = up.LastName,
                Address = up.Address,
                Email = up.IdentityUser.Email,
                UserName = up.IdentityUser.UserName,
                IdentityUserId = up.IdentityUserId,
                Roles = _dbContext.UserRoles
                    .Where(ur => ur.UserId == up.IdentityUserId)
                    .Select(ur => _dbContext.Roles.SingleOrDefault(r => r.Id == ur.RoleId).Name)
                    .ToList()
            })
            .ToList();

        return Ok(profilesWithRoles);
    }

    [HttpPost("promote/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Promote(string id)
    {
        IdentityRole role = _dbContext.Roles.SingleOrDefault(r => r.Name == "Admin");
        if (role == null)
        {
            return NotFound();
        }

        _dbContext.UserRoles.Add(new IdentityUserRole<string>
        {
            RoleId = role.Id,
            UserId = id
        });
        _dbContext.SaveChanges();
        return NoContent();
    }

    [HttpPost("demote/{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult Demote(string id)
    {
        IdentityRole role = _dbContext.Roles.SingleOrDefault(r => r.Name == "Admin");
        if (role == null)
        {
            return NotFound();
        }

        IdentityUserRole<string> userRole = _dbContext.UserRoles
            .SingleOrDefault(ur => ur.RoleId == role.Id && ur.UserId == id);

        if (userRole == null)
        {
            return NotFound();
        }

        _dbContext.UserRoles.Remove(userRole);
        _dbContext.SaveChanges();
        return NoContent();
    }
}
