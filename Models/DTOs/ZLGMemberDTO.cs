namespace ZombieLynxPortal.Models.DTOs
{
    public class ZLGMemberDTO
    {
        public int Id { get; set; }
        public string SteamId { get; set; }
        public string SteamName { get; set; }  // ✅ Added SteamName
        public string EosId { get; set; }
        public string EpicName { get; set; }  // ✅ Added EpicName
        public string DiscordId { get; set; }
        public string DiscordName { get; set; }  // ✅ Added DiscordName
        public string IdentityUserId { get; set; }
        public int UserProfileId { get; set; }

        // DTO for related entities
        public UserProfileDTO UserProfile { get; set; }
    }
}
