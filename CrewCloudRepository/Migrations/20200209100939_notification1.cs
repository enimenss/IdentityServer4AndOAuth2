using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class notification1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RestaurantVis",
                table: "RestaurantUserNotifications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UserVis",
                table: "RestaurantUserNotifications",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestaurantVis",
                table: "RestaurantUserNotifications");

            migrationBuilder.DropColumn(
                name: "UserVis",
                table: "RestaurantUserNotifications");
        }
    }
}
