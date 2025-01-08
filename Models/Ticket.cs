namespace ZombieLynxPortal.Models;

public class Ticket
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Category { get; set; }
    public string Game { get; set; }
    public string Server { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }

    public ICollection<UserTicket> UserTickets { get; set; }
}
