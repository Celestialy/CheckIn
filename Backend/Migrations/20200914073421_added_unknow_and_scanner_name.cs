using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class added_unknow_and_scanner_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Scanners",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Unknown",
                table: "Cards",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Scanners");

            migrationBuilder.DropColumn(
                name: "Unknown",
                table: "Cards");
        }
    }
}
