using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZombieLynxPortal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateMVP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminTickets",
                keyColumns: new[] { "AdminId", "TicketId" },
                keyValues: new object[] { 1, 1 },
                column: "AssignedAt",
                value: new DateTime(2025, 1, 13, 16, 3, 10, 157, DateTimeKind.Utc).AddTicks(433));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "853727c8-8aa1-4ac1-a8db-62e0bd5f3505", "AQAAAAIAAYagAAAAEL1DS+XEkYH5xPWS1OorxR1U4/ZEHz5gF8Y5c26C0wqQ/wXgQ3suQAqL/+2vk22CPw==", "8caa4bd3-17a5-455e-ac24-b94fcee3f9ca" });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 13, 16, 3, 10, 157, DateTimeKind.Utc).AddTicks(415));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "SentAt", "Type" },
                values: new object[] { new DateTime(2025, 1, 13, 16, 3, 10, 157, DateTimeKind.Utc).AddTicks(464), new List<int> { 0, 1 } });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 13, 16, 3, 10, 157, DateTimeKind.Utc).AddTicks(346), new DateTime(2025, 1, 13, 16, 3, 10, 157, DateTimeKind.Utc).AddTicks(347) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
