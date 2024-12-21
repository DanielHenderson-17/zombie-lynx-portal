using Microsoft.AspNetCore.Identity;

namespace ZombieLynxPortal.Models;
public class ZombieMember
{
    public int Id { get; set; }
    public string DiscordId { get; set; }
    public string EosId { get; set; }
    public string SteamId { get; set; }
    public string Role { get; set; }
}
