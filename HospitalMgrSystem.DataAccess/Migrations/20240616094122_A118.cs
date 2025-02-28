using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A118 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaimBills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    ConsultantId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: true),
                    RefNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CsId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimBills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimBills_CashierSessions_CsId",
                        column: x => x.CsId,
                        principalTable: "CashierSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimBills_Consultants_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "Consultants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimBills_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimBills_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBills_ConsultantId",
                table: "ClaimBills",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBills_CsId",
                table: "ClaimBills",
                column: "CsId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBills_InvoiceId",
                table: "ClaimBills",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimBills_PatientID",
                table: "ClaimBills",
                column: "PatientID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimBills");
        }
    }
}
