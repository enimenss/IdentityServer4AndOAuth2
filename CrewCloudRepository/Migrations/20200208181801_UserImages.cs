using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class UserImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber",
                table: "User",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "User");
        }
    }
}
