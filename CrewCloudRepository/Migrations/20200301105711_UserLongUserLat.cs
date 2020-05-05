using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class UserLongUserLat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "UserLat",
                table: "Restaurant",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "UserLong",
                table: "Restaurant",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserLat",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "UserLong",
                table: "Restaurant");
        }
    }
}
