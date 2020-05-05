using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class RestaurantImages4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Restaurant_RestaurantId",
                table: "Job");

            migrationBuilder.DropIndex(
                name: "IX_Job_RestaurantId",
                table: "Job");

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantId",
                table: "Job",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RestaurantId",
                table: "Job",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Job_RestaurantId",
                table: "Job",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Restaurant_RestaurantId",
                table: "Job",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "RestaurantId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
