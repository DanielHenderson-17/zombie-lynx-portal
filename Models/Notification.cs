using Microsoft.AspNetCore.Identity;

namespace ZombieLynxPortal.Models;

public class Notification
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public int[] Type { get; set; } // 0 = SMS, 1 = Email, 2 = Discord
    public DateTime SentAt { get; set; }

    public Ticket Ticket { get; set; }
    public ZombieMember ZombieMember { get; set; }
}

