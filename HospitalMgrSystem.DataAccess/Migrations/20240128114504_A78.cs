using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A78 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillingType",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sessionID",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_sessionID",
                table: "Payments",
                column: "sessionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CashierSessions_sessionID",
                table: "Payments",
                column: "sessionID",
                principalTable: "CashierSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CashierSessions_sessionID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_sessionID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "BillingType",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "sessionID",
                table: "Payments");
        }
    }
}
