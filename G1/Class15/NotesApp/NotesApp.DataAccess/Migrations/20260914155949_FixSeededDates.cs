using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixSeededDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Note",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Note",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Note",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 26, 18, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Note",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5077), new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5078) });

            migrationBuilder.UpdateData(
                table: "Note",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5080), new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5081) });

            migrationBuilder.UpdateData(
                table: "Note",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5083), new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5083) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5010), new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5011) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5014), new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5015) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5016), new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5016) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5018), new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5018) });

            migrationBuilder.UpdateData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5020), new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(5020) });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(4854), new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(4856) });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(4862), new DateTime(2026, 9, 14, 15, 53, 17, 729, DateTimeKind.Utc).AddTicks(4862) });
        }
    }
}
