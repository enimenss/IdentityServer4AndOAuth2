using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class HandlingImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantId",
                table: "Image",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Image",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Image_RestaurantId",
                table: "Image",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_UserId",
                table: "Image",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Restaurant_RestaurantId",
                table: "Image",
                column: "RestaurantId",
                principalTable: "Restaurant",
                principalColumn: "RestaurantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Image_User_UserId",
                table: "Image",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Restaurant_RestaurantId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Image_User_UserId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_RestaurantId",
                table: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Image_UserId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Image");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantId",
                table: "Image",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
