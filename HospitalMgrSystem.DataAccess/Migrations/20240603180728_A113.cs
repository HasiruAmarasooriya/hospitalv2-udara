using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A113 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShiftID",
                table: "AppointmentsDTO",
                newName: "shiftID");

            migrationBuilder.RenameColumn(
                name: "InvoiceType",
                table: "AppointmentsDTO",
                newName: "invoiceType");

            migrationBuilder.RenameColumn(
                name: "SchedulerId",
                table: "AppointmentsDTO",
                newName: "schedularId");

            migrationBuilder.RenameColumn(
                name: "CreateUser",
                table: "AppointmentsDTO",
                newName: "CreatedUser");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "shiftID",
                table: "AppointmentsDTO",
                newName: "ShiftID");

            migrationBuilder.RenameColumn(
                name: "invoiceType",
                table: "AppointmentsDTO",
                newName: "InvoiceType");

            migrationBuilder.RenameColumn(
                name: "schedularId",
                table: "AppointmentsDTO",
                newName: "SchedulerId");

            migrationBuilder.RenameColumn(
                name: "CreatedUser",
                table: "AppointmentsDTO",
                newName: "CreateUser");
        }
    }
}
