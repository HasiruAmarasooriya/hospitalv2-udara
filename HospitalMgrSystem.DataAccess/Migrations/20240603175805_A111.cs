using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentsDTO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentsDTO",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentNo = table.Column<int>(type: "int", nullable: false),
                    ConsultantFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConsultantID = table.Column<int>(type: "int", nullable: false),
                    ConsultantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HospitalFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceType = table.Column<int>(type: "int", nullable: false),
                    IsOnOPD = table.Column<int>(type: "int", nullable: false),
                    ItemIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedUser = table.Column<int>(type: "int", nullable: false),
                    OtherFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    SchedulerId = table.Column<int>(type: "int", nullable: false),
                    ShiftID = table.Column<int>(type: "int", nullable: false),
                    SpecialistName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentsDTO", x => x.ID);
                });
        }
    }
}
