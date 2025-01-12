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
        public string? SteamId { get; set; } 

        [MaxLength(100)]
        public string? SteamName { get; set; } 
        [MaxLength(250)]
        public string? SteamImgUrl { get; set; }

        [MaxLength(100)]
        public string? EosId { get; set; }

        [MaxLength(100)]
        public string? EpicName { get; set; }

        [MaxLength(250)]
        public string? EpicImgUrl { get; set; }

        [MaxLength(100)]
        public string? DiscordId { get; set; }

        [MaxLength(100)]
        public string? DiscordName { get; set; }

        [MaxLength(250)]
        public string? DiscordImgUrl { get; set; }

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
