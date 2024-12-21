using Microsoft.AspNetCore.Identity;

namespace ZombieLynxPortal.Models;
public class Ticket
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Subject { get; set; }
    public string Categroy { get; set; }
    public string Game { get; set; }
    public string Server { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int ZombieMemberId { get; set; }
    public ZombieMember ZombieMember { get; set; }
}

