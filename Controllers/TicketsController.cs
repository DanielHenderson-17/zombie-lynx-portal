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

    // Retrieves all open tickets or tickets assigned to the current user based on role
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

    // Retrieves all closed tickets or tickets assigned to the current user based on role
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

    // Updates a specific ticket to closed status
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

    // Updates a specific ticket to open status
    [HttpPut("{id}/restore")]
    [Authorize]
    public IActionResult RestoreTicket(int id)
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

    // Deletes a specific ticket if it is closed
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

    // Retrieves options for ticket creation form (categories, games, servers)
    [HttpGet("options")]
    [Authorize]
    public IActionResult GetOptions()
    {
        var categories = new[] { "Bug", "Shop Issue", "Connection Issue", "Other" };
        var games = new[] { "Ark:SA", "Ark:SE", "Palworld", "Empyrion", "Minecraft", "Eco" };
        var servers = new[] { "NA-East", "EU-West", "Asia" };
        return Ok(new { categories, games, servers });
    }

    // Retrieves all users (Admin only)
    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetUsers()
    {
        var users = _dbContext.UserProfiles.Select(up => new
        {
            up.Id,
            FullName = $"{up.FirstName} {up.LastName}"
        }).ToList();

        return Ok(users);
    }

    // Creates a new ticket via POST request form data
    // Parse JSON directly from the HTTP request body
    [HttpPost]
    [Authorize]
    public IActionResult CreateTicket([FromBody] CreateTicketDTO createTicketDto)
    {
        if (createTicketDto == null)
        {
            return BadRequest("Invalid ticket data.");
        }

        var identityUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.IdentityUserId == identityUserId);

        if (userProfile == null)
        {
            return BadRequest("User profile not found.");
        }

        if (string.IsNullOrEmpty(createTicketDto.Subject) ||
            string.IsNullOrEmpty(createTicketDto.Category) ||
            string.IsNullOrEmpty(createTicketDto.Game) ||
            string.IsNullOrEmpty(createTicketDto.Server))
        {
            return BadRequest("Missing required fields.");
        }

        var ticket = new Ticket
        {
            Subject = createTicketDto.Subject,
            Category = createTicketDto.Category,
            Game = createTicketDto.Game,
            Server = createTicketDto.Server,
            Description = createTicketDto.Description,
            Status = "Open",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UserProfileId = userProfile.Id
        };

        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            _dbContext.Tickets.Add(ticket);
            _dbContext.SaveChanges();

            _dbContext.UserTickets.Add(new UserTicket
            {
                TicketId = ticket.Id,
                UserProfileId = userProfile.Id,
                AssignedAt = DateTime.UtcNow
            });

            if (createTicketDto.AssignedUserIds != null && createTicketDto.AssignedUserIds.Any())
            {
                foreach (var assignedUserId in createTicketDto.AssignedUserIds)
                {
                    if (_dbContext.UserProfiles.Any(up => up.Id == assignedUserId))
                    {
                        _dbContext.UserTickets.Add(new UserTicket
                        {
                            TicketId = ticket.Id,
                            UserProfileId = assignedUserId,
                            AssignedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            _dbContext.SaveChanges();
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return StatusCode(500, $"Error creating ticket: {ex.Message}");
        }

        return CreatedAtAction(nameof(CreateTicket), new { id = ticket.Id }, new
        {
            ticket.Id,
            ticket.Subject,
            ticket.Category,
            ticket.Game,
            ticket.Server,
            ticket.Description,
            ticket.Status,
            ticket.CreatedAt,
            ticket.UpdatedAt,
            AssignedUsers = _dbContext.UserTickets
                .Where(ut => ut.TicketId == ticket.Id)
                .Select(ut => new
                {
                    ut.UserProfile.FirstName,
                    ut.UserProfile.LastName
                })
                .ToList()
        });
    }

    // Retrieves a ticket by its Id with assigned users
    [HttpGet("{id}")]
    [Authorize]
    public IActionResult GetTicketById(int id)
    {
        var ticket = _dbContext.Tickets
            .Include(t => t.UserTickets)
                .ThenInclude(ut => ut.UserProfile)
            .Where(t => t.Id == id)
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
                AssignedUsers = t.UserTickets.Select(ut => new
                {
                    ut.UserProfile.FirstName,
                    ut.UserProfile.LastName
                }).ToList()
            })
            .FirstOrDefault();

        if (ticket == null)
        {
            return NotFound($"Ticket with ID {id} not found.");
        }

        return Ok(ticket);
    }

    // Assign a user to a ticket by Id (Admin only)
    [HttpPost("{id}/assign-user")]
    [Authorize(Roles = "Admin")]
    public IActionResult AssignUserToTicket(int id, [FromBody] int userId)
    {
        var ticket = _dbContext.Tickets.SingleOrDefault(t => t.Id == id);
        if (ticket == null)
        {
            return NotFound($"Ticket with ID {id} not found.");
        }

        var userProfile = _dbContext.UserProfiles.SingleOrDefault(up => up.Id == userId);
        if (userProfile == null)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        var existingAssignment = _dbContext.UserTickets
            .Any(ut => ut.TicketId == id && ut.UserProfileId == userId);
        if (existingAssignment)
        {
            return BadRequest("User is already assigned to this ticket.");
        }

        var userTicket = new UserTicket
        {
            TicketId = id,
            UserProfileId = userId,
            AssignedAt = DateTime.UtcNow
        };

        _dbContext.UserTickets.Add(userTicket);
        _dbContext.SaveChanges();

        return NoContent();
    }

    // Edit a ticket by Id 
    [HttpPut("{id}/edit")]
    [Authorize]
    public IActionResult EditTicket(int id, [FromBody] EditTicketDTO editTicketDto)
    {
        if (editTicketDto == null)
        {
            return BadRequest("Invalid ticket data.");
        }

        var ticket = _dbContext.Tickets.SingleOrDefault(t => t.Id == id);
        if (ticket == null)
        {
            return NotFound($"Ticket with ID {id} not found.");
        }

        ticket.Subject = editTicketDto.Subject;
        ticket.Category = editTicketDto.Category;
        ticket.Game = editTicketDto.Game;
        ticket.Server = editTicketDto.Server;
        ticket.Description = editTicketDto.Description;
        ticket.UpdatedAt = DateTime.UtcNow;

        _dbContext.SaveChanges();

        // Return the updated ticket as a response
        return Ok(new
        {
            ticket.Id,
            ticket.Subject,
            ticket.Category,
            ticket.Game,
            ticket.Server,
            ticket.Description,
            ticket.UpdatedAt
        });
    }

}
