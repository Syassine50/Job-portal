using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffreDmploie.Migrations
{
    /// <inheritdoc />
    public partial class seconf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NomDeLoffre",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomDeLoffre",
                table: "Jobs");
        }
    }
}
