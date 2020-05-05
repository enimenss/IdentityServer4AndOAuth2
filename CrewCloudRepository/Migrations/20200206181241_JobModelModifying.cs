using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CrewCloudRepository.Migrations
{
    public partial class JobModelModifying : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFrom",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "DateTo",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Job");

            migrationBuilder.AddColumn<string>(
                name: "RestaurantId1",
                table: "Restaurant",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantId",
                table: "Job",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DailyPaid",
                table: "Job",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Job",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "HourlyPaid",
                table: "Job",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MonthlyPaid",
                table: "Job",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_RestaurantId1",
                table: "Restaurant",
                column: "RestaurantId1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_Restaurant_RestaurantId1",
                table: "Restaurant",
                column: "RestaurantId1",
                principalTable: "Restaurant",
                principalColumn: "RestaurantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Restaurant_RestaurantId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_Restaurant_RestaurantId1",
                table: "Restaurant");

            migrationBuilder.DropIndex(
                name: "IX_Restaurant_RestaurantId1",
                table: "Restaurant");

            migrationBuilder.DropIndex(
                name: "IX_Job_RestaurantId",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "RestaurantId1",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "DailyPaid",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "HourlyPaid",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "MonthlyPaid",
                table: "Job");

            migrationBuilder.AlterColumn<string>(
                name: "RestaurantId",
                table: "Job",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFrom",
                table: "Job",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTo",
                table: "Job",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Job",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
