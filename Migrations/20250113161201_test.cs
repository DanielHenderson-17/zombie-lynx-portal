using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZombieLynxPortal.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AdminTickets",
                keyColumns: new[] { "AdminId", "TicketId" },
                keyValues: new object[] { 1, 1 },
                column: "AssignedAt",
                value: new DateTime(2025, 1, 13, 16, 12, 1, 506, DateTimeKind.Utc).AddTicks(6002));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e738bb89-ac12-42df-bcf8-9559a4460f3d", "AQAAAAIAAYagAAAAEFS/bnpDAk44OkP23i+Xcm1jDvvpT9C1nAcmqFgOkFbcb7VccrUhkIDv6CRI2YCkQg==", "1ceaf538-5945-43c1-b59b-846d9644de91" });

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 1, 13, 16, 12, 1, 506, DateTimeKind.Utc).AddTicks(5982));

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "SentAt", "Type" },
                values: new object[] { new DateTime(2025, 1, 13, 16, 12, 1, 506, DateTimeKind.Utc).AddTicks(6028), new List<int> { 0, 1 } });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 1, 13, 16, 12, 1, 506, DateTimeKind.Utc).AddTicks(5856), new DateTime(2025, 1, 13, 16, 12, 1, 506, DateTimeKind.Utc).AddTicks(5858) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
