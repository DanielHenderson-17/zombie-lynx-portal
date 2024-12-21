using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ZombieLynxPortal.Models;
using Microsoft.AspNetCore.Identity;

namespace ZombieLynxPortal.Data;
public class ZombieLynxPortalDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<ZombieMember> ZombieMembers { get; set; } // Added DbSet for ZombieMembers
    public DbSet<Ticket> Tickets { get; set; } // Added DbSet for Tickets
    public DbSet<Message> Messages { get; set; } // Added DbSet for Messages
    public DbSet<AdminTicket> AdminTickets { get; set; } // Added DbSet for AdminTickets
    public DbSet<Notification> Notifications { get; set; } // Added DbSet for Notifications

    public ZombieLynxPortalDbContext(DbContextOptions<ZombieLynxPortalDbContext> context, IConfiguration config) : base(context)
    {
        _configuration = config;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
            Name = "Admin",
            NormalizedName = "admin"
        });

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

        modelBuilder.Entity<UserProfile>().HasData(new UserProfile
        {
            Id = 1,
            IdentityUserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
            FirstName = "Admina",
            LastName = "Strator",
            Address = "101 Main Street",
        });

        // Seed Data for ZombieMembers
        modelBuilder.Entity<ZombieMember>().HasData(new ZombieMember
        {
            Id = 1,
            DiscordId = "123456789",
            EosId = "eos12345",
            SteamId = "steam12345",
            Role = "Admin"
        });

        // Seed Data for Tickets
        modelBuilder.Entity<Ticket>().HasData(new Ticket
        {
            Id = 1,
            UserId = 1,
            ZombieMemberId = 1,
            Subject = "Bug Report",
            Categroy = "Gameplay",
            Game = "Game A",
            Server = "NA-East",
            Description = "My character is stuck!",
            Status = "Open",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });


        // Seed Data for Messages
        modelBuilder.Entity<Message>().HasData(new Message
        {
            Id = 1,
            TicketId = 1,
            UserId = 1,
            Content = "This issue is urgent.",
            CreatedAt = DateTime.UtcNow
        });

        // Seed Data for AdminTickets
        modelBuilder.Entity<AdminTicket>().HasData(new AdminTicket
        {
            AdminId = 1,
            TicketId = 1,
            AssignedAt = DateTime.UtcNow
        });

        // Seed Data for Notifications
        modelBuilder.Entity<Notification>().HasData(new Notification
        {
            Id = 1,
            TicketId = 1,
            UserId = 1,
            Type = new int[] { 0, 1 },
            SentAt = DateTime.UtcNow
        });

        // Relationships
        modelBuilder.Entity<Ticket>()
            .HasOne<ZombieMember>()
            .WithMany()
            .HasForeignKey(t => t.UserId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.ZombieMember)
            .WithMany()
            .HasForeignKey(t => t.ZombieMemberId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Ticket)
            .WithMany()
            .HasForeignKey(m => m.TicketId);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId);

        modelBuilder.Entity<AdminTicket>()
            .HasKey(at => new { at.AdminId, at.TicketId });

        modelBuilder.Entity<Notification>()
            .HasOne<Ticket>()
            .WithMany()
            .HasForeignKey(n => n.TicketId);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Ticket)
            .WithMany()
            .HasForeignKey(n => n.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.ZombieMember)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);


    }
}
