using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NotesApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tag",
                columns: new[] { "Id", "Color", "CreatedDate", "Name", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "cyan", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2651), "Homework", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2651) },
                    { 2, "orange", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2653), "Avenga", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2653) },
                    { 3, "green", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2654), "Healthy", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2655) },
                    { 4, "blue", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2655), "Exercise", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2656) },
                    { 5, "red", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2656), "Urgent", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2657) }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedDate", "FirstName", "LastName", "Password", "UpdatedDate", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2544), "Bob", "Bobsky", "SuperSecret123", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2546), "bob" },
                    { 2, new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2549), "Petko", "Petkovsky", "AlsoSecret456", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2550), "petko" }
                });

            migrationBuilder.InsertData(
                table: "Note",
                columns: new[] { "Id", "CreatedDate", "Priority", "Text", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2694), "High", "Do Homework", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2694), 1 },
                    { 2, new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2696), "Medium", "Drink more water", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2696), 1 },
                    { 3, new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2697), "Low", "Go to the gym", new DateTime(2026, 8, 26, 18, 0, 34, 191, DateTimeKind.Utc).AddTicks(2698), 2 }
                });

            migrationBuilder.InsertData(
                table: "NoteTag",
                columns: new[] { "NoteId", "TagId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 3 },
                    { 3, 4 },
                    { 3, 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NoteTag",
                keyColumns: new[] { "NoteId", "TagId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "NoteTag",
                keyColumns: new[] { "NoteId", "TagId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "NoteTag",
                keyColumns: new[] { "NoteId", "TagId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "NoteTag",
                keyColumns: new[] { "NoteId", "TagId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "NoteTag",
                keyColumns: new[] { "NoteId", "TagId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "Note",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Note",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Note",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tag",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
