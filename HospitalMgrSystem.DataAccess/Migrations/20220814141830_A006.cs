using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A006 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultants_Specialists_SpecialistID",
                table: "Consultants");

            migrationBuilder.RenameColumn(
                name: "SpecialistID",
                table: "Consultants",
                newName: "SpecialistId");

            migrationBuilder.RenameIndex(
                name: "IX_Consultants_SpecialistID",
                table: "Consultants",
                newName: "IX_Consultants_SpecialistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultants_Specialists_SpecialistId",
                table: "Consultants",
                column: "SpecialistId",
                principalTable: "Specialists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultants_Specialists_SpecialistId",
                table: "Consultants");

            migrationBuilder.RenameColumn(
                name: "SpecialistId",
                table: "Consultants",
                newName: "SpecialistID");

            migrationBuilder.RenameIndex(
                name: "IX_Consultants_SpecialistId",
                table: "Consultants",
                newName: "IX_Consultants_SpecialistID");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultants_Specialists_SpecialistID",
                table: "Consultants",
                column: "SpecialistID",
                principalTable: "Specialists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
