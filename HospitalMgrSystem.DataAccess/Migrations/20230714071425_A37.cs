using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OPD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    ConsultantID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    AppoimentNo = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OPD", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OPD_Consultants_ConsultantID",
                        column: x => x.ConsultantID,
                        principalTable: "Consultants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OPD_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OPD_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OPD_ConsultantID",
                table: "OPD",
                column: "ConsultantID");

            migrationBuilder.CreateIndex(
                name: "IX_OPD_PatientID",
                table: "OPD",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_OPD_RoomID",
                table: "OPD",
                column: "RoomID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OPD");
        }
    }
}
