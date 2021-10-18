using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class Added_scanner_to_check_and_time_to_room : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Added",
                table: "Rooms",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ScannerMacAddress",
                table: "CheckTimes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckTimes_ScannerMacAddress",
                table: "CheckTimes",
                column: "ScannerMacAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckTimes_Scanners_ScannerMacAddress",
                table: "CheckTimes",
                column: "ScannerMacAddress",
                principalTable: "Scanners",
                principalColumn: "MacAddress",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckTimes_Scanners_ScannerMacAddress",
                table: "CheckTimes");

            migrationBuilder.DropIndex(
                name: "IX_CheckTimes_ScannerMacAddress",
                table: "CheckTimes");

            migrationBuilder.DropColumn(
                name: "Added",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ScannerMacAddress",
                table: "CheckTimes");
        }
    }
}
