using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZombieLynxPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddZLGMembersAgainCorrectly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SteamId",
                table: "ZLGMembers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "DiscordId",
                table: "ZLGMembers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DiscordName",
                table: "ZLGMembers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EosId",
                table: "ZLGMembers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EpicName",
                table: "ZLGMembers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SteamName",
                table: "ZLGMembers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AdminTickets",
                keyColumns: new[] { "AdminId", "TicketId" },
                keyValues: new object[] { 1, 1 },
                column: "AssignedAt",
                value: new DateTime(2025, 1, 11, 21, 13, 58, 803, DateTimeKind.Utc).AddTicks(4554));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
                column: "NormalizedName",
                value: "ADMIN");

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

            migrationBuilder.UpdateData(
                table: "ZLGMembers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DiscordId", "DiscordName", "EosId", "EpicName", "SteamName" },
                values: new object[] { "123456789012345678", "AdminDiscord", "eos-admin-id", "AdminEpic", "AdminSteam" });

            migrationBuilder.UpdateData(
                table: "ZLGMembers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DiscordId", "DiscordName", "EosId", "EpicName", "SteamName" },
                values: new object[] { "987654321098765432", "TestDiscord", "eos-test-id", "TestEpic", "TestUser" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordId",
                table: "ZLGMembers");

            migrationBuilder.DropColumn(
                name: "DiscordName",
                table: "ZLGMembers");

            migrationBuilder.DropColumn(
                name: "EosId",
                table: "ZLGMembers");

            migrationBuilder.DropColumn(
                name: "EpicName",
                table: "ZLGMembers");

            migrationBuilder.DropColumn(
                name: "SteamName",
                table: "ZLGMembers");

            migrationBuilder.AlterColumn<string>(
                name: "SteamId",
                table: "ZLGMembers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                table: "AdminTickets",
                keyColumns: new[] { "AdminId", "TicketId" },
                keyValues: new object[] { 1, 1 },
                column: "AssignedAt",
                value: new DateTime(2025, 1, 11, 20, 53, 52, 39, DateTimeKind.Utc).AddTicks(6295));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
                column: "NormalizedName",
                value: "admin");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7ad6ba16-d8a7-4229-9382-03a1e60be6d8", "AQAAAAIAAYagAAAAEEfBLTPR0ivmSZ+/LoCRAreSfjT/oQeluc8NET+onrrxLAdHPhT2lxuwse5LemnMZg==", "257314b0-b938-4dc7-9259-fa0121c88193" });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 11, 20, 53, 52, 39, DateTimeKind.Utc).AddTicks(6274));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "SentAt", "Type" },
                values: new object[] { new DateTime(2025, 1, 11, 20, 53, 52, 39, DateTimeKind.Utc).AddTicks(6315), new List<int> { 0, 1 } });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 11, 20, 53, 52, 39, DateTimeKind.Utc).AddTicks(6217), new DateTime(2025, 1, 11, 20, 53, 52, 39, DateTimeKind.Utc).AddTicks(6219) });
        }
    }
}
