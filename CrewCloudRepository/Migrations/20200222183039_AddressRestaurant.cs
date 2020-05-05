using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class AddressRestaurant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Restaurant");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Restaurant",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Restaurant");

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Restaurant",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
