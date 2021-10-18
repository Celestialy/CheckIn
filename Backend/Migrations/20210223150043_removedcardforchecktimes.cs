using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class removedcardforchecktimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckTimes_Cards_CardId",
                table: "CheckTimes");

            migrationBuilder.DropIndex(
                name: "IX_CheckTimes_CardId",
                table: "CheckTimes");

            migrationBuilder.AlterColumn<string>(
                name: "CardId",
                table: "CheckTimes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CardId",
                table: "CheckTimes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
