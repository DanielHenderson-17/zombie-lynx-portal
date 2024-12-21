using Microsoft.AspNetCore.Identity;

namespace ZombieLynxPortal.Models.DTOs;

public class AdminTicketDTO
{
    public int AdminId { get; set; }
    public int TicketId { get; set; }
    public DateTime AssignedAt { get; set; }

    public ZombieMember Admin { get; set; }
    public Ticket Ticket { get; set; }
}
