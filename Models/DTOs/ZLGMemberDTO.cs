using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [MaxLength(250)]
        public string SteamImgUrl { get; set; }

        [MaxLength(250)]
        public string DiscordImgUrl { get; set; }

        [MaxLength(250)]
        public string EpicImgUrl { get; set; }
        public string IdentityUserId { get; set; }
        public int UserProfileId { get; set; }

        // DTO for related entities
        public UserProfileDTO UserProfile { get; set; }
    }
}
