using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class changed_oid_to_userID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Oid",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Cards",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "Oid",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
