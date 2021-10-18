using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class renamed_rooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Rooms_RoomsID",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_RoomsID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "RoomsID",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "RoomID",
                table: "Cards",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_RoomID",
                table: "Cards",
                column: "RoomID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Rooms_RoomID",
                table: "Cards",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Rooms_RoomID",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_RoomID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "RoomID",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "RoomsID",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_RoomsID",
                table: "Cards",
                column: "RoomsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Rooms_RoomsID",
                table: "Cards",
                column: "RoomsID",
                principalTable: "Rooms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
