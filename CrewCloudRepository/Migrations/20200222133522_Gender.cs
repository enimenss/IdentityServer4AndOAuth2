using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class Gender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
