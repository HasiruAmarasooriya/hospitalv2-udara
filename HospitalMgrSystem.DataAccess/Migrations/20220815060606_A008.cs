using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BHTNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdmission = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    ConsultantId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Guardian = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temp = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pluse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateUser = table.Column<int>(type: "int", nullable: false),
                    ModifiedUser = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admissions_Consultants_ConsultantId",
                        column: x => x.ConsultantId,
                        principalTable: "Consultants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Admissions_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Admissions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_ConsultantId",
                table: "Admissions",
                column: "ConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_PatientId",
                table: "Admissions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Admissions_RoomId",
                table: "Admissions",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admissions");
        }
    }
}
