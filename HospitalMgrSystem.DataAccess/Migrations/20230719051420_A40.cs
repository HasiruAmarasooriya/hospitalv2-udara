using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A40 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OPDDrugus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdmissionId = table.Column<int>(type: "int", nullable: false),
                    opdId = table.Column<int>(type: "int", nullable: true),
                    DrugId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateUser = table.Column<int>(type: "int", nullable: false),
                    ModifiedUser = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPDDrugus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OPDDrugus_Drugs_DrugId",
                        column: x => x.DrugId,
                        principalTable: "Drugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OPDDrugus_OPD_opdId",
                        column: x => x.opdId,
                        principalTable: "OPD",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OPDDrugus_DrugId",
                table: "OPDDrugus",
                column: "DrugId");

            migrationBuilder.CreateIndex(
                name: "IX_OPDDrugus_opdId",
                table: "OPDDrugus",
                column: "opdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OPDDrugus");
        }
    }
}
