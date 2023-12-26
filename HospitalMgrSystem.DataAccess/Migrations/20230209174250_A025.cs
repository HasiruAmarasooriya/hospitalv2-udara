using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A025 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
              name: "InvoiceType",
              table: "Admissions");

            migrationBuilder.DropColumn(
              name: "Discount",
              table: "Admissions");

            migrationBuilder.DropColumn(
              name: "AdmissionId",
              table: "Admissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
