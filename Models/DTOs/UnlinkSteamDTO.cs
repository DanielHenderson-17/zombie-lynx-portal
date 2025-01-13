namespace ZombieLynxPortal.Models.DTOs
{
    public class UnlinkSteamDTO
    {
        public string IdentityUserId { get; set; } 
        public string? SteamId { get; set; }
        public string? SteamName { get; set; }
        public string? SteamImgUrl { get; set; }
    }
}
