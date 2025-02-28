using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A122 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrevName",
                table: "Patients");

            migrationBuilder.CreateTable(
                name: "ClaimBillItemsData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScanItemId = table.Column<int>(type: "int", nullable: true),
                    ClaimBillId = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimBillItemsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimBillItemsData_ChannelingItems_ScanItemId",
                        column: x => x.ScanItemId,
                        principalTable: "ChannelingItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimBillItemsData_ClaimBills_ClaimBillId",
                        column: x => x.ClaimBillId,
                        principalTable: "ClaimBills",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimBillItemsData_Users_CreateUser",
                        column: x => x.CreateUser,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimBillItemsData_Users_ModifiedUser",
                        column: x => x.ModifiedUser,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBillItemsData_ClaimBillId",
                table: "ClaimBillItemsData",
                column: "ClaimBillId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBillItemsData_CreateUser",
                table: "ClaimBillItemsData",
                column: "CreateUser");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBillItemsData_ModifiedUser",
                table: "ClaimBillItemsData",
                column: "ModifiedUser");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBillItemsData_ScanItemId",
                table: "ClaimBillItemsData",
                column: "ScanItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimBillItemsData");

            migrationBuilder.AddColumn<string>(
                name: "PrevName",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
