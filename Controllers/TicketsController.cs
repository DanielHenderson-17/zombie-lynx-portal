using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZombieLynxPortal.Data;
using ZombieLynxPortal.Models;
using ZombieLynxPortal.Models.DTOs;

namespace ZombieLynxPortal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ZombieLynxPortalDbContext _dbContext;

    public TicketsController(ZombieLynxPortalDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet("open")]
    [Authorize]
    public IActionResult GetOpenTickets()
    {
        var identityUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userRoles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(r => r.Value).ToList();
        var userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.IdentityUserId == identityUserId);

        var ticketsQuery = _dbContext.Tickets
            .Where(t => t.Status == "Open")
            .Select(t => new
            {
                t.Id,
                t.Subject,
                t.Category,
                t.Game,
                t.Server,
                t.Description,
                t.Status,
                t.CreatedAt,
                t.UpdatedAt,
                AssignedUsers = _dbContext.UserTickets
                    .Where(ut => ut.TicketId == t.Id)
                    .Select(ut => new
                    {
                        ut.UserProfile.FirstName,
                        ut.UserProfile.LastName
                    })
                    .ToList()
            });

        if (!userRoles.Contains("Admin") && userProfile != null)
        {
            ticketsQuery = ticketsQuery
                .Where(t => _dbContext.UserTickets
                    .Any(ut => ut.TicketId == t.Id && ut.UserProfileId == userProfile.Id));
        }

        return Ok(ticketsQuery.ToList());
    }

    [HttpGet("closed")]
    [Authorize]
    public IActionResult GetClosedTickets()
    {
        var identityUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userRoles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(r => r.Value).ToList();
        var userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.IdentityUserId == identityUserId);

        var ticketsQuery = _dbContext.Tickets
            .Where(t => t.Status == "Closed")
            .Select(t => new
            {
                t.Id,
                t.Subject,
                t.Category,
                t.Game,
                t.Server,
                t.Description,
                t.Status,
                t.CreatedAt,
                t.UpdatedAt,
                AssignedUsers = _dbContext.UserTickets
                    .Where(ut => ut.TicketId == t.Id)
                    .Select(ut => new
                    {
                        ut.UserProfile.FirstName,
                        ut.UserProfile.LastName
                    })
                    .ToList()
            });

        if (!userRoles.Contains("Admin") && userProfile != null)
        {
            ticketsQuery = ticketsQuery
                .Where(t => _dbContext.UserTickets
                    .Any(ut => ut.TicketId == t.Id && ut.UserProfileId == userProfile.Id));
        }

        return Ok(ticketsQuery.ToList());
    }

    [HttpPut("{id}/close")]
    [Authorize]
    public IActionResult CloseTicket(int id)
    {
        Ticket ticket = _dbContext.Tickets.SingleOrDefault(t => t.Id == id);
        if (ticket == null)
        {
            return NotFound();
        }

        ticket.Status = "Closed";
        ticket.UpdatedAt = DateTime.Now;
        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPut("{id}/restore")]
    [Authorize]
    public IActionResult RestoreTIcket(int id)
    {
        Ticket ticket = _dbContext.Tickets.SingleOrDefault(t => t.Id == id);
        if (ticket == null)
        {
            return NotFound();
        }

        ticket.Status = "Open";
        ticket.UpdatedAt = DateTime.Now;
        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult DeleteTicket(int id)
    {
        var ticket = _dbContext.Tickets.SingleOrDefault(t => t.Id == id);

        if (ticket == null)
        {
            return NotFound();
        }

        if (ticket.Status != "Closed")
        {
            return BadRequest("Only closed tickets can be deleted.");
        }

        _dbContext.Tickets.Remove(ticket);
        _dbContext.SaveChanges();

        return NoContent();
    }
}


