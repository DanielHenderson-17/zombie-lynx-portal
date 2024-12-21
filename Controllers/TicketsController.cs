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

    // Get open tickets for the active user or all tickets if the user is an admin
    [HttpGet("open")]
    [Authorize]
    public IActionResult GetOpenTickets()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userRoles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(r => r.Value).ToList();

        var ticketsQuery = _dbContext.Tickets
            .Include(t => t.ZombieMember)
            .Where(t => t.Status == "Open");

        if (!userRoles.Contains("Admin"))
        {
            // Filter tickets for the logged-in user only
            ticketsQuery = ticketsQuery.Where(t => t.UserId == int.Parse(userId));
        }

        var tickets = ticketsQuery
            .Select(t => new TicketDTO
            {
                Id = t.Id,
                UserId = t.UserId,
                Subject = t.Subject,
                Categroy = t.Categroy,
                Game = t.Game,
                Server = t.Server,
                Description = t.Description,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToList();

        return Ok(tickets);
    }

    // Create a new ticket
    [HttpPost]
    [Authorize]
    public IActionResult CreateTicket(TicketDTO ticketDTO)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

        var ticket = new Ticket
        {
            UserId = userId,
            Subject = ticketDTO.Subject,
            Categroy = ticketDTO.Categroy,
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

    // Update a ticket
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

        // Only update editable fields
        ticketToUpdate.Subject = ticketDTO.Subject;
        ticketToUpdate.Description = ticketDTO.Description;
        ticketToUpdate.Game = ticketDTO.Game;
        ticketToUpdate.Server = ticketDTO.Server;
        ticketToUpdate.Categroy = ticketDTO.Categroy;
        ticketToUpdate.UpdatedAt = DateTime.Now;

        _dbContext.SaveChanges();

        return NoContent();
    }

    // Close a ticket
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

    // Delete a ticket
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
