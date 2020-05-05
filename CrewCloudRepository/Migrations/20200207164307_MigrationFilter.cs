using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class MigrationFilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Restaurant_RestaurantId1",
                table: "Restaurant");

            migrationBuilder.DropIndex(
                name: "IX_Restaurant_RestaurantId1",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "RestaurantId1",
                table: "Restaurant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RestaurantId1",
                table: "Restaurant",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_RestaurantId1",
                table: "Restaurant",
                column: "RestaurantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Restaurant_RestaurantId1",
                table: "Restaurant",
                column: "RestaurantId1",
                principalTable: "Restaurant",
                principalColumn: "RestaurantId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
