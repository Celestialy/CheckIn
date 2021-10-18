using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class added_rooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomsID",
                table: "Cards",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomName = table.Column<string>(nullable: true),
                    ScannerMacAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rooms_Scanners_ScannerMacAddress",
                        column: x => x.ScannerMacAddress,
                        principalTable: "Scanners",
                        principalColumn: "MacAddress",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_RoomsID",
                table: "Cards",
                column: "RoomsID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ScannerMacAddress",
                table: "Rooms",
                column: "ScannerMacAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Rooms_RoomsID",
                table: "Cards",
                column: "RoomsID",
                principalTable: "Rooms",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Rooms_RoomsID",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Cards_RoomsID",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "RoomsID",
                table: "Cards");
        }
    }
}
