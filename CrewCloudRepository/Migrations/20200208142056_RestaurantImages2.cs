using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class RestaurantImages2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "UserImage",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "RestaurantImage",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "UserImage");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "RestaurantImage");
        }
    }
}
