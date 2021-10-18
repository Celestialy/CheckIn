using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class CardHasUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasUser",
                table: "Cards",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasUser",
                table: "Cards");
        }
    }
}
