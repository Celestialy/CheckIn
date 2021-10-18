using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class arhhhhh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckTimes_Cards_CardId",
                table: "CheckTimes");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_CheckTimes_CardId",
                table: "CheckTimes");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "CheckTimes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardId",
                table: "CheckTimes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HasUser = table.Column<bool>(type: "bit", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: true),
                    TimeAdded = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardID);
                    table.ForeignKey(
                        name: "FK_Cards_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckTimes_CardId",
                table: "CheckTimes",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_RoomID",
                table: "Cards",
                column: "RoomID");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckTimes_Cards_CardId",
                table: "CheckTimes",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "CardID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
