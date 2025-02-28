using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimBillItemsData_ChannelingItems_ScanItemId",
                table: "ClaimBillItemsData");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimBillItemsData_ClaimBills_ClaimBillId",
                table: "ClaimBillItemsData");

            migrationBuilder.DropIndex(
                name: "IX_ClaimBillItemsData_ClaimBillId",
                table: "ClaimBillItemsData");

            migrationBuilder.DropIndex(
                name: "IX_ClaimBillItemsData_ScanItemId",
                table: "ClaimBillItemsData");

            migrationBuilder.DropColumn(
                name: "ClaimBillId",
                table: "ClaimBillItemsData");

            migrationBuilder.AddColumn<string>(
                name: "RefId",
                table: "ClaimBillItemsData",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefId",
                table: "ClaimBillItemsData");

            migrationBuilder.AddColumn<int>(
                name: "ClaimBillId",
                table: "ClaimBillItemsData",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBillItemsData_ClaimBillId",
                table: "ClaimBillItemsData",
                column: "ClaimBillId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBillItemsData_ScanItemId",
                table: "ClaimBillItemsData",
                column: "ScanItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimBillItemsData_ChannelingItems_ScanItemId",
                table: "ClaimBillItemsData",
                column: "ScanItemId",
                principalTable: "ChannelingItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimBillItemsData_ClaimBills_ClaimBillId",
                table: "ClaimBillItemsData",
                column: "ClaimBillId",
                principalTable: "ClaimBills",
                principalColumn: "Id");
        }
    }
}
