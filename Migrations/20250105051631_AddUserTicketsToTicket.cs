using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZombieLynxPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTicketsToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminTickets",
                keyColumns: new[] { "AdminId", "TicketId" },
                keyValues: new object[] { 1, 1 },
                column: "AssignedAt",
                value: new DateTime(2024, 12, 22, 5, 52, 53, 164, DateTimeKind.Utc).AddTicks(500));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5ae3d5a8-6870-438c-b6d7-4d2a0a7bc501", "AQAAAAIAAYagAAAAEODCFY9nGSBnV9OMcX5L1STNnFUxmI2l+3WAmmp6yuXYovZeNGrDZTPpL3kxyemmfA==", "b1c66abd-176f-4c5c-8400-be5666e8c80f" });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2024, 12, 22, 5, 52, 53, 164, DateTimeKind.Utc).AddTicks(479));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "SentAt", "Type" },
                values: new object[] { new DateTime(2024, 12, 22, 5, 52, 53, 164, DateTimeKind.Utc).AddTicks(518), new List<int> { 0, 1 } });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 22, 5, 52, 53, 164, DateTimeKind.Utc).AddTicks(432), new DateTime(2024, 12, 22, 5, 52, 53, 164, DateTimeKind.Utc).AddTicks(433) });
        }
    }
}
