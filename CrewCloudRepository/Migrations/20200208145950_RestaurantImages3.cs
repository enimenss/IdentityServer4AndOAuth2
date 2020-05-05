using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class RestaurantImages3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RestaurantImage_Restaurant_RestaurantId",
                table: "RestaurantImage");

            migrationBuilder.DropIndex(
                name: "IX_RestaurantImage_RestaurantId",
                table: "RestaurantImage");

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantId",
                table: "RestaurantImage",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RestaurantId",
                table: "RestaurantImage",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantImage_RestaurantId",
                table: "RestaurantImage",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_RestaurantImage_Restaurant_RestaurantId",
                table: "RestaurantImage",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "RestaurantId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
