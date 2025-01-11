using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZombieLynxPortal.Models
{
    public class ZLGMember
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string SteamId { get; set; }

        [MaxLength(100)]
        public string SteamName { get; set; }  // ✅ Added SteamName

        [MaxLength(100)]
        public string EosId { get; set; }

        [MaxLength(100)]
        public string EpicName { get; set; }  // ✅ Added EpicName

        [MaxLength(100)]
        public string DiscordId { get; set; }

        [MaxLength(100)]
        public string DiscordName { get; set; }  // ✅ Added DiscordName

        [Required]
        public string IdentityUserId { get; set; }

        [Required]
        public int UserProfileId { get; set; }

        // Navigation Properties
        [ForeignKey("UserProfileId")]
        public UserProfile UserProfile { get; set; }

        [ForeignKey("IdentityUserId")]
        public IdentityUser IdentityUser { get; set; }
    }
}
