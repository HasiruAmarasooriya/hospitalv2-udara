using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A84 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "col1",
                table: "CashierSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "col10",
                table: "CashierSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "col2",
                table: "CashierSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "col3",
                table: "CashierSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "col4",
                table: "CashierSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "col5",
                table: "CashierSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "col6",
                table: "CashierSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "col7",
                table: "CashierSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "col8",
                table: "CashierSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "col9",
                table: "CashierSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "col1",
                table: "CashierSessions");

            migrationBuilder.DropColumn(
                name: "col10",
                table: "CashierSessions");

            migrationBuilder.DropColumn(
                name: "col2",
                table: "CashierSessions");

            migrationBuilder.DropColumn(
                name: "col3",
                table: "CashierSessions");

            migrationBuilder.DropColumn(
                name: "col4",
                table: "CashierSessions");

            migrationBuilder.DropColumn(
                name: "col5",
                table: "CashierSessions");

            migrationBuilder.DropColumn(
                name: "col6",
                table: "CashierSessions");

            migrationBuilder.DropColumn(
                name: "col7",
                table: "CashierSessions");

            migrationBuilder.DropColumn(
                name: "col8",
                table: "CashierSessions");

            migrationBuilder.DropColumn(
                name: "col9",
                table: "CashierSessions");
        }
    }
}
