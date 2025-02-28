using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A126 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaimBillId",
                table: "ClaimBillItemsData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBillItemsData_ClaimBillId",
                table: "ClaimBillItemsData",
                column: "ClaimBillId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimBillItemsData_ClaimBills_ClaimBillId",
                table: "ClaimBillItemsData",
                column: "ClaimBillId",
                principalTable: "ClaimBills",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimBillItemsData_ClaimBills_ClaimBillId",
                table: "ClaimBillItemsData");

            migrationBuilder.DropIndex(
                name: "IX_ClaimBillItemsData_ClaimBillId",
                table: "ClaimBillItemsData");

            migrationBuilder.DropColumn(
                name: "ClaimBillId",
                table: "ClaimBillItemsData");
        }
    }
}
