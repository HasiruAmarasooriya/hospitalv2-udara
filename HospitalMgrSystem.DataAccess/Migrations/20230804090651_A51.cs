using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A51 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "itemInvoiceStatus",
                table: "OPDItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "itemInvoiceStatus",
                table: "OPDInvestigations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "itemInvoiceStatus",
                table: "OPDDrugus",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "itemInvoiceStatus",
                table: "OPDItems");

            migrationBuilder.DropColumn(
                name: "itemInvoiceStatus",
                table: "OPDInvestigations");

            migrationBuilder.DropColumn(
                name: "itemInvoiceStatus",
                table: "OPDDrugus");
        }
    }
}
