using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OPDInvestigations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    opdId = table.Column<int>(type: "int", nullable: false),
                    InvestigationId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_OPDInvestigations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OPDInvestigations_Investigation_InvestigationId",
                        column: x => x.InvestigationId,
                        principalTable: "Investigation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OPDInvestigations_OPD_opdId",
                        column: x => x.opdId,
                        principalTable: "OPD",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OPDInvestigations_InvestigationId",
                table: "OPDInvestigations",
                column: "InvestigationId");

            migrationBuilder.CreateIndex(
                name: "IX_OPDInvestigations_opdId",
                table: "OPDInvestigations",
                column: "opdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OPDInvestigations");
        }
    }
}
