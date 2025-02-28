using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A106 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "isSystemDr",
                table: "Consultants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EndCardBalence",
                table: "CashierSessions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSystemDr",
                table: "Consultants");

            migrationBuilder.DropColumn(
                name: "EndCardBalence",
                table: "CashierSessions");
        }
    }
}
