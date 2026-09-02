using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryApi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Isbn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    PageCount = table.Column<int>(type: "int", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Book_Author_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Author",
                columns: new[] { "Id", "Country", "CreatedDate", "FirstName", "LastName", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "United Kingdom", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "George", "Orwell", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "United States", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Isaac", "Asimov", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "United States", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ursula", "Le Guin", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "Israel", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Yuval", "Harari", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "Id", "AuthorId", "CreatedDate", "Genre", "Isbn", "PageCount", "Title", "UpdatedDate", "Year" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fiction", "9780451524935", 328, "1984", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1949 },
                    { 2, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fiction", "9780452284241", 112, "Animal Farm", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1945 },
                    { 3, 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "History", "9780156421171", 232, "Homage to Catalonia", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1938 },
                    { 4, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Science", "9780553293357", 255, "Foundation", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1951 },
                    { 5, 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Science", "9780553382563", 253, "I, Robot", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1950 },
                    { 6, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fantasy", "9780553383041", 183, "A Wizard of Earthsea", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1968 },
                    { 7, 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fantasy", "9780441478125", 304, "The Left Hand of Darkness", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1969 },
                    { 8, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "History", "9780062316097", 443, "Sapiens", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2011 },
                    { 9, 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "History", "9780062464316", 450, "Homo Deus", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2015 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Author_LastName",
                table: "Author",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorId",
                table: "Book",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Author");
        }
    }
}
