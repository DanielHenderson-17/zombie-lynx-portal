namespace ZombieLynxPortal.Models;

public class Notification
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }

    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }

    public List<int> Type { get; set; }
    public DateTime SentAt { get; set; }
}
