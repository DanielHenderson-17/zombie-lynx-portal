using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ZombieLynxPortal.Models;
using Microsoft.AspNetCore.Identity;

namespace ZombieLynxPortal.Data
{
    public class ZombieLynxPortalDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly IConfiguration _configuration;

        // DbSets for all entities
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserTicket> UserTickets { get; set; }
        public DbSet<AdminTicket> AdminTickets { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
        public DbSet<ZLGMember> ZLGMembers { get; set; } // ✅ ZLGMembers Table

        public ZombieLynxPortalDbContext(DbContextOptions<ZombieLynxPortalDbContext> context, IConfiguration config)
            : base(context)
        {
            _configuration = config;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Seed Roles
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
                Name = "Admin",
                NormalizedName = "ADMIN"
            });

            // ✅ Seed Admin User
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                UserName = "Administrator",
                Email = "admina@strator.comx",
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, _configuration["AdminPassword"])
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
                UserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f"
            });

            // ✅ Seed User Profile
            modelBuilder.Entity<UserProfile>().HasData(new UserProfile
            {
                Id = 1,
                IdentityUserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                FirstName = "Admina",
                LastName = "Strator",
                Address = "101 Main Street"
            });

            // ✅ Seed ZLGMembers
            modelBuilder.Entity<ZLGMember>().HasData(
                new ZLGMember
                {
                    Id = 1,
                    SteamId = "76561198021051512",
                    IdentityUserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                    UserProfileId = 1,
                    SteamName = "AdminSteam",
                    SteamImgUrl = "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/adm/adminsteam.jpg",
                    DiscordId = "123456789012345678",
                    DiscordName = "AdminDiscord",
                    DiscordImgUrl = "https://cdn.discordapp.com/avatars/123456789012345678/admin-discord.png",
                    EosId = "eos-admin-id",
                    EpicName = "AdminEpic",
                    EpicImgUrl = "https://static.epicgames.com/admin-epic-avatar.png"
                }
            );

            // ✅ Seed Tickets
            modelBuilder.Entity<Ticket>().HasData(new Ticket
            {
                Id = 1,
                UserProfileId = 1,
                Subject = "Bug Report",
                Category = "Gameplay",
                Game = "Game A",
                Server = "NA-East",
                Description = "My character is stuck!",
                Status = "Open",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            // ✅ Seed Messages
            modelBuilder.Entity<Message>().HasData(new Message
            {
                Id = 1,
                TicketId = 1,
                UserProfileId = 1,
                Content = "This issue is urgent.",
                CreatedAt = DateTime.UtcNow
            });

            // ✅ Seed AdminTickets
            modelBuilder.Entity<AdminTicket>().HasData(new AdminTicket
            {
                AdminId = 1,
                TicketId = 1,
                AssignedAt = DateTime.UtcNow
            });

            // ✅ Seed Notifications
            modelBuilder.Entity<Notification>().HasData(new Notification
            {
                Id = 1,
                TicketId = 1,
                UserProfileId = 1,
                Type = new List<int> { 0, 1 },
                SentAt = DateTime.UtcNow
            });

            // ✅ Relationships
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.UserProfile)
                .WithMany()
                .HasForeignKey(t => t.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Ticket)
                .WithMany()
                .HasForeignKey(m => m.TicketId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.UserProfile)
                .WithMany()
                .HasForeignKey(m => m.UserProfileId);

            modelBuilder.Entity<UserTicket>()
                .HasKey(ut => new { ut.UserProfileId, ut.TicketId });

            modelBuilder.Entity<AdminTicket>()
                .HasKey(at => new { at.AdminId, at.TicketId });

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Ticket)
                .WithMany()
                .HasForeignKey(n => n.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.UserProfile)
                .WithMany()
                .HasForeignKey(n => n.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ ZLGMember Relationships
            modelBuilder.Entity<ZLGMember>()
                .HasOne(z => z.UserProfile)
                .WithMany()
                .HasForeignKey(z => z.UserProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ZLGMember>()
                .HasOne(z => z.IdentityUser)
                .WithMany()
                .HasForeignKey(z => z.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
