using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OffreDmploie.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResumeContentType",
                table: "Candidatures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ResumeFile",
                table: "Candidatures",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResumeFileName",
                table: "Candidatures",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeContentType",
                table: "Candidatures");

            migrationBuilder.DropColumn(
                name: "ResumeFile",
                table: "Candidatures");

            migrationBuilder.DropColumn(
                name: "ResumeFileName",
                table: "Candidatures");
        }
    }
}
