using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class addedForgeinkeytoCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Rooms_RoomID",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "RoomID",
                table: "Cards",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_RoomID",
                table: "Cards",
                newName: "IX_Cards_RoomId");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Rooms_RoomId",
                table: "Cards",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Rooms_RoomId",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Cards",
                newName: "RoomID");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_RoomId",
                table: "Cards",
                newName: "IX_Cards_RoomID");

            migrationBuilder.AlterColumn<int>(
                name: "RoomID",
                table: "Cards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Rooms_RoomID",
                table: "Cards",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
