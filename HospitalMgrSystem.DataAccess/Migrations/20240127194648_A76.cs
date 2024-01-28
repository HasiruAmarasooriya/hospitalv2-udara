using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A76 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OtherTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionID = table.Column<int>(type: "int", nullable: true),
                    ConvenerID = table.Column<int>(type: "int", nullable: true),
                    InvoiceType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedByID = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    otherTransactionsStatus = table.Column<int>(type: "int", nullable: false),
                    CreateUser = table.Column<int>(type: "int", nullable: true),
                    ModifiedUser = table.Column<int>(type: "int", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OtherTransactions_CashierSessions_SessionID",
                        column: x => x.SessionID,
                        principalTable: "CashierSessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OtherTransactions_Users_ApprovedByID",
                        column: x => x.ApprovedByID,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OtherTransactions_Users_ConvenerID",
                        column: x => x.ConvenerID,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OtherTransactions_ApprovedByID",
                table: "OtherTransactions",
                column: "ApprovedByID");

            migrationBuilder.CreateIndex(
                name: "IX_OtherTransactions_ConvenerID",
                table: "OtherTransactions",
                column: "ConvenerID");

            migrationBuilder.CreateIndex(
                name: "IX_OtherTransactions_SessionID",
                table: "OtherTransactions",
                column: "SessionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OtherTransactions");
        }
    }
}
