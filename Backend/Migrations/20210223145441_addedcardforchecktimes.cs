using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class addedcardforchecktimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardId",
                table: "CheckTimes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckTimes_CardId",
                table: "CheckTimes",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckTimes_Cards_CardId",
                table: "CheckTimes",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "_card",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckTimes_Cards_CardId",
                table: "CheckTimes");

            migrationBuilder.DropIndex(
                name: "IX_CheckTimes_CardId",
                table: "CheckTimes");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "CheckTimes");
        }
    }
}
