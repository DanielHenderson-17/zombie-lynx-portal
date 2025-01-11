using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZombieLynxPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddZLGMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZLGMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SteamId = table.Column<string>(type: "text", nullable: false),
                    IdentityUserId = table.Column<string>(type: "text", nullable: false),
                    UserProfileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZLGMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZLGMembers_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZLGMembers_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AdminTickets",
                keyColumns: new[] { "AdminId", "TicketId" },
                keyValues: new object[] { 1, 1 },
                column: "AssignedAt",
                value: new DateTime(2025, 1, 11, 20, 53, 52, 39, DateTimeKind.Utc).AddTicks(6295));

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

            migrationBuilder.InsertData(
                table: "ZLGMembers",
                columns: new[] { "Id", "IdentityUserId", "SteamId", "UserProfileId" },
                values: new object[,]
                {
                    { 1, "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f", "76561198021051512", 1 },
                    { 2, "941e60e9-d226-4567-8b9a-56928ffbb160", "76561198012345678", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZLGMembers_IdentityUserId",
                table: "ZLGMembers",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ZLGMembers_UserProfileId",
                table: "ZLGMembers",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZLGMembers");

            migrationBuilder.UpdateData(
                table: "AdminTickets",
                keyColumns: new[] { "AdminId", "TicketId" },
                keyValues: new object[] { 1, 1 },
                column: "AssignedAt",
                value: new DateTime(2025, 1, 5, 5, 16, 30, 822, DateTimeKind.Utc).AddTicks(1243));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6aeec7b1-e370-46cc-aa59-9fa136bc4857", "AQAAAAIAAYagAAAAEMqmrxAi4H0YaFBBCONtw3OlmpK8H6kdu21F7oPUZEXOXGewDqF4AfDAAoK4o+oFCQ==", "0dbb0a98-16ec-48e6-8c22-f277bf8157e4" });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 5, 5, 16, 30, 822, DateTimeKind.Utc).AddTicks(1227));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "SentAt", "Type" },
                values: new object[] { new DateTime(2025, 1, 5, 5, 16, 30, 822, DateTimeKind.Utc).AddTicks(1260), new List<int> { 0, 1 } });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 5, 5, 16, 30, 822, DateTimeKind.Utc).AddTicks(1195), new DateTime(2025, 1, 5, 5, 16, 30, 822, DateTimeKind.Utc).AddTicks(1196) });
        }
    }
}
