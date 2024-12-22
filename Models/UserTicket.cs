namespace ZombieLynxPortal.Models;

public class UserTicket
{
    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }

    public DateTime AssignedAt { get; set; } = DateTime.Now;
}
