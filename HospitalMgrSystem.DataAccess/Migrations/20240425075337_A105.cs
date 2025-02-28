using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A105 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "ChannelingSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelingSchedule_RoomId",
                table: "ChannelingSchedule",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelingSchedule_Rooms_RoomId",
                table: "ChannelingSchedule",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelingSchedule_Rooms_RoomId",
                table: "ChannelingSchedule");

            migrationBuilder.DropIndex(
                name: "IX_ChannelingSchedule_RoomId",
                table: "ChannelingSchedule");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "ChannelingSchedule");
        }
    }
}
