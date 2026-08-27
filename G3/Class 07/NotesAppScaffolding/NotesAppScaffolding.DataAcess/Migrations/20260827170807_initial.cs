using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesAppScaffolding.DataAcess.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Users",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Firstname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        Lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
            //        Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
            //        Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
            //        Age = table.Column<int>(type: "int", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Users", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Notes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
            //        Tag = table.Column<int>(type: "int", nullable: true),
            //        Priority = table.Column<int>(type: "int", nullable: false),
            //        UserId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Notes", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Notes_Users",
            //            column: x => x.UserId,
            //            principalTable: "Users",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notes_UserId",
            //    table: "Notes",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "UQ_Users_Username",
            //    table: "Users",
            //    column: "Username",
            //    unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
