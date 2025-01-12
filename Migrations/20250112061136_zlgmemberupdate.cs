using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZombieLynxPortal.Migrations
{
    /// <inheritdoc />
    public partial class zlgmemberupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscordImgUrl",
                table: "ZLGMembers",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EpicImgUrl",
                table: "ZLGMembers",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SteamImgUrl",
                table: "ZLGMembers",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AdminTickets",
                keyColumns: new[] { "AdminId", "TicketId" },
                keyValues: new object[] { 1, 1 },
                column: "AssignedAt",
                value: new DateTime(2025, 1, 12, 6, 11, 36, 294, DateTimeKind.Utc).AddTicks(6944));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "02d35709-7106-4f3a-9208-11cb2396fe53", "AQAAAAIAAYagAAAAEONOz9/lcaFK3fVMPOuz0JdLCCjCybuI5P9RpnHIO+ax91YDtIp0rHmGo5UWft74Ng==", "059e7dd8-e1f7-4b38-86a4-3d2b2ce4c613" });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 12, 6, 11, 36, 294, DateTimeKind.Utc).AddTicks(6923));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "SentAt", "Type" },
                values: new object[] { new DateTime(2025, 1, 12, 6, 11, 36, 294, DateTimeKind.Utc).AddTicks(6966), new List<int> { 0, 1 } });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 12, 6, 11, 36, 294, DateTimeKind.Utc).AddTicks(6865), new DateTime(2025, 1, 12, 6, 11, 36, 294, DateTimeKind.Utc).AddTicks(6866) });

            migrationBuilder.UpdateData(
                table: "ZLGMembers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DiscordImgUrl", "EpicImgUrl", "SteamImgUrl" },
                values: new object[] { "https://cdn.discordapp.com/avatars/123456789012345678/admin-discord.png", "https://static.epicgames.com/admin-epic-avatar.png", "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/adm/adminsteam.jpg" });

            migrationBuilder.UpdateData(
                table: "ZLGMembers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DiscordImgUrl", "EpicImgUrl", "SteamImgUrl" },
                values: new object[] { "https://cdn.discordapp.com/avatars/987654321098765432/test-discord.png", "https://static.epicgames.com/test-epic-avatar.png", "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/avatars/test/testuser.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordImgUrl",
                table: "ZLGMembers");

            migrationBuilder.DropColumn(
                name: "EpicImgUrl",
                table: "ZLGMembers");

            migrationBuilder.DropColumn(
                name: "SteamImgUrl",
                table: "ZLGMembers");

            migrationBuilder.UpdateData(
                table: "AdminTickets",
                keyColumns: new[] { "AdminId", "TicketId" },
                keyValues: new object[] { 1, 1 },
                column: "AssignedAt",
                value: new DateTime(2025, 1, 11, 21, 13, 58, 803, DateTimeKind.Utc).AddTicks(4554));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "70dc7011-5ff1-4bd2-acb0-af4744fc53bf", "AQAAAAIAAYagAAAAEOG9ldRVsgMsmJfeS16CDqK2NuApDveXdwnkEzluxPRYd0BOXORycqD2S532dHIByA==", "5adc2ed9-2bdd-4349-aaff-e7326885b8f8" });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 11, 21, 13, 58, 803, DateTimeKind.Utc).AddTicks(4533));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "SentAt", "Type" },
                values: new object[] { new DateTime(2025, 1, 11, 21, 13, 58, 803, DateTimeKind.Utc).AddTicks(4576), new List<int> { 0, 1 } });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 11, 21, 13, 58, 803, DateTimeKind.Utc).AddTicks(4470), new DateTime(2025, 1, 11, 21, 13, 58, 803, DateTimeKind.Utc).AddTicks(4472) });
        }
    }
}
