using Microsoft.AspNetCore.Identity;

namespace ZombieLynxPortal.Models.DTOs;

public class MessageDTO
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }

    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }

    public string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}



