using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A80 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "shift",
                table: "OPD",
                newName: "shiftID");

            migrationBuilder.CreateIndex(
                name: "IX_OPD_shiftID",
                table: "OPD",
                column: "shiftID");

            migrationBuilder.AddForeignKey(
                name: "FK_OPD_NtShiftSessions_shiftID",
                table: "OPD",
                column: "shiftID",
                principalTable: "NtShiftSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OPD_NtShiftSessions_shiftID",
                table: "OPD");

            migrationBuilder.DropIndex(
                name: "IX_OPD_shiftID",
                table: "OPD");

            migrationBuilder.RenameColumn(
                name: "shiftID",
                table: "OPD",
                newName: "shift");
        }
    }
}
