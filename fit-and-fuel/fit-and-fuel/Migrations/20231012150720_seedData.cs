using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fit_and_fuel.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Appoitments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Time",
                value: new DateTime(2023, 10, 12, 19, 7, 20, 284, DateTimeKind.Local).AddTicks(9715));

            migrationBuilder.UpdateData(
                table: "Appoitments",
                keyColumn: "Id",
                keyValue: 2,
                column: "Time",
                value: new DateTime(2023, 10, 12, 20, 7, 20, 284, DateTimeKind.Local).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "63cb881a-8ce3-46e0-b96b-ff4eee0392f5", "AQAAAAIAAYagAAAAEBOSf4nTKo4gT577vntPARDi/OHIvr3SRHqLNWGIqy/coAoUhq7qK4gYVQddysgGwg==", "ffba7e22-d15c-4db2-96c2-6ee383406ad7" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "294a109b-b171-45bb-b5ef-5865db0c1c28", "AQAAAAIAAYagAAAAEAuQjFSn11c6prvDC3fznHKFCXV9lD+vEH9Iv7ME40dOAsxKGVr2JyxQTfoW4iB8kQ==", "52709e15-4e99-4c0f-98ea-7766365bb2f1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c88cc58b-175c-4c3f-9184-b836ee3fe2d8", "AQAAAAIAAYagAAAAEENY4eWE1YSzVbREANqefHlsQmOKRdN29VdaSGrx0tN4ka5UnD29glLay5+4mZxsSw==", "f7ffccff-0a2c-4b5a-8640-36c0c16e5d6a" });

            migrationBuilder.InsertData(
                table: "Meals",
                columns: new[] { "Id", "Calories", "Completion", "DayId", "Description", "Name" },
                values: new object[,]
                {
                    { 3, 500, false, 2, "Grilled meat salad", "Lunch" },
                    { 4, 200, false, 2, "egss and milk", "BreakFast" }
                });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Time",
                value: new DateTime(2023, 10, 12, 18, 7, 20, 284, DateTimeKind.Local).AddTicks(9655));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Time",
                value: new DateTime(2023, 10, 12, 18, 7, 20, 284, DateTimeKind.Local).AddTicks(9673));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "Id",
                keyValue: 4);

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
    }
}
