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

    [HttpPut("{id}/restore")]
    [Authorize]
    public IActionResult RestoreTicket(int id)
    {
        var ticket = _dbContext.Tickets.SingleOrDefault(t => t.Id == id);

        if (ticket == null)
        {
            return NotFound();
        }

        ticket.Status = "Open";
        ticket.UpdatedAt = DateTime.Now;

        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateTicket(TicketDTO ticketDTO)
    {
        var identityUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.IdentityUserId == identityUserId);

        if (userProfile == null)
        {
            return BadRequest("UserProfile not found.");
        }

        var ticket = new Ticket
        {
            Subject = ticketDTO.Subject,
            Category = ticketDTO.Category,
            Game = ticketDTO.Game,
            Server = ticketDTO.Server,
            Description = ticketDTO.Description,
            Status = "Open",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _dbContext.Tickets.Add(ticket);
        _dbContext.SaveChanges();

        return Created($"/api/tickets/{ticket.Id}", ticket);
    }

    [HttpPut("{id}")]
    [Authorize]
    public IActionResult UpdateTicket(int id, TicketDTO ticketDTO)
    {
        var ticketToUpdate = _dbContext.Tickets.SingleOrDefault(t => t.Id == id);

        if (ticketToUpdate == null)
        {
            return NotFound();
        }

        if (id != ticketDTO.Id)
        {
            return BadRequest();
        }

        ticketToUpdate.Subject = ticketDTO.Subject;
        ticketToUpdate.Description = ticketDTO.Description;
        ticketToUpdate.Game = ticketDTO.Game;
        ticketToUpdate.Server = ticketDTO.Server;
        ticketToUpdate.Category = ticketDTO.Category;
        ticketToUpdate.UpdatedAt = DateTime.Now;

        _dbContext.SaveChanges();

        return NoContent();
    }

    [HttpPut("{id}/close")]
    [Authorize]
    public IActionResult CloseTicket(int id)
    {
        var ticket = _dbContext.Tickets.SingleOrDefault(t => t.Id == id);

        if (ticket == null)
        {
            return NotFound();
        }

        ticket.Status = "Closed";
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

        _dbContext.Tickets.Remove(ticket);
        _dbContext.SaveChanges();

        return NoContent();
    }
}
