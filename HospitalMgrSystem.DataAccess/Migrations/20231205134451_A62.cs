using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A62 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Specialists",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "Drugs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "isStock",
                table: "Drugs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OPDScheduler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsultantId = table.Column<int>(type: "int", nullable: false),
                    OPDSchedulerStatus = table.Column<int>(type: "int", nullable: false),
                    OPDSession = table.Column<int>(type: "int", nullable: false),
                    startTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isActiveSession = table.Column<int>(type: "int", nullable: false),
                    isPaid = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateUser = table.Column<int>(type: "int", nullable: false),
                    ModifiedUser = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPDScheduler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OPDScheduler_Consultants_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "Consultants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stockTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    itemID = table.Column<int>(type: "int", nullable: false),
                    drugIDId = table.Column<int>(type: "int", nullable: true),
                    TransactionMethods = table.Column<int>(type: "int", nullable: false),
                    qty = table.Column<int>(type: "int", nullable: false),
                    CreateUser = table.Column<int>(type: "int", nullable: false),
                    ModifiedUser = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stockTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stockTransaction_Drugs_drugIDId",
                        column: x => x.drugIDId,
                        principalTable: "Drugs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OPDScheduler_ConsultantId",
                table: "OPDScheduler",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_stockTransaction_drugIDId",
                table: "stockTransaction",
                column: "drugIDId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OPDScheduler");

            migrationBuilder.DropTable(
                name: "stockTransaction");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "isStock",
                table: "Drugs");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Specialists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
