using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class addedCardidToCheckTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckTimes_Cards_CardID",
                table: "CheckTimes");

            migrationBuilder.RenameColumn(
                name: "CardID",
                table: "CheckTimes",
                newName: "CardId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckTimes_CardID",
                table: "CheckTimes",
                newName: "IX_CheckTimes_CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckTimes_Cards_CardId",
                table: "CheckTimes",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "CardID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckTimes_Cards_CardId",
                table: "CheckTimes");

            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "CheckTimes",
                newName: "CardID");

            migrationBuilder.RenameIndex(
                name: "IX_CheckTimes_CardId",
                table: "CheckTimes",
                newName: "IX_CheckTimes_CardID");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckTimes_Cards_CardID",
                table: "CheckTimes",
                column: "CardID",
                principalTable: "Cards",
                principalColumn: "CardID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
