using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffreDmploie.Migrations
{
    /// <inheritdoc />
    public partial class migrationuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nomdentreprise",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nometprenom",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nomdentreprise",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nometprenom",
                table: "AspNetUsers");
        }
    }
}
