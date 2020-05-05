using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class AddCityToRestaurant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Restaurant");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Restaurant",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Restaurant",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Restaurant");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Restaurant",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
