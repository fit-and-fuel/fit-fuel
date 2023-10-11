using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fit_and_fuel.Migrations
{
    /// <inheritdoc />
    public partial class readnote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ReadNotfication",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Appoitments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Time",
                value: new DateTime(2023, 10, 11, 23, 39, 22, 726, DateTimeKind.Local).AddTicks(1509));

            migrationBuilder.UpdateData(
                table: "Appoitments",
                keyColumn: "Id",
                keyValue: 2,
                column: "Time",
                value: new DateTime(2023, 10, 12, 0, 39, 22, 726, DateTimeKind.Local).AddTicks(1520));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b2a2e3b2-e097-4f43-9960-90bd2d4f4558", "AQAAAAIAAYagAAAAEL6lRdsmu4qxlpROU3KzBtJUFeSbyVQZaCIUpy2ymdHnT/ZX50zmJIIY4Fy/RZn6zQ==", "ff2ff20c-e57e-42f1-9d4b-108c1f19e81c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fa28df14-0ebf-47a5-a50a-ef95b7525543", "AQAAAAIAAYagAAAAEG1zZTbvNPQuKixqZ/0vU9MJY6sTJY1JOwhDkEaiI+1g1n6Rc1scMSRxa419SQvHJQ==", "8898e7c2-7a84-40f9-a55f-cf820cde5481" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "81d2fbbe-f79e-440b-982b-3b5002df1c3b", "AQAAAAIAAYagAAAAEMuP4aoxM1DXGyB8aJ8KnfsYprKR0okCDVhA8ukJ6L1rgYSvj+ADAK1vAmYKtbK/2w==", "73906890-eb87-4589-bce3-19a4fb0b4942" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Time",
                value: new DateTime(2023, 10, 11, 22, 39, 22, 726, DateTimeKind.Local).AddTicks(1455));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Time",
                value: new DateTime(2023, 10, 11, 22, 39, 22, 726, DateTimeKind.Local).AddTicks(1470));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadNotfication",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Appoitments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Time",
                value: new DateTime(2023, 10, 10, 12, 59, 5, 938, DateTimeKind.Local).AddTicks(6044));

            migrationBuilder.UpdateData(
                table: "Appoitments",
                keyColumn: "Id",
                keyValue: 2,
                column: "Time",
                value: new DateTime(2023, 10, 10, 13, 59, 5, 938, DateTimeKind.Local).AddTicks(6058));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e52cb70d-d009-4fa3-b6bb-b92c1eb000da", "AQAAAAIAAYagAAAAEG3iljj+bcz9Y5xibWV4BtNh0tPOhQgcG7SGv/MEUy06UYWfV1eRyvKYXC8xoMvF1A==", "7eea1770-8df8-49ba-bddc-27987f4e1853" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "12d3211d-488f-4237-ab31-03174c38705c", "AQAAAAIAAYagAAAAEA5yHAhp/AidXd7imoz4UJVgSnBhbiX17S0IGhtOVqFGXuofkje9CcRa/64/2cBCTA==", "b7022264-a7f0-4ae1-8f3d-2404c7cb6a8a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4bf0a67d-b220-491f-a452-2c6b5ae386f6", "AQAAAAIAAYagAAAAEHFvKvN0WFf9Zsg3NtRMKuTWtzA5ffNzo8RwDnGgpK3y04N9Nv3vMmUpxMAU/eloxQ==", "72167936-d027-4d2b-af5b-17ae7b2a62e4" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Time",
                value: new DateTime(2023, 10, 10, 11, 59, 5, 938, DateTimeKind.Local).AddTicks(5911));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Time",
                value: new DateTime(2023, 10, 10, 11, 59, 5, 938, DateTimeKind.Local).AddTicks(5925));
        }
    }
}
