using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OPDDrugus_OPD_opdId",
                table: "OPDDrugus");

            migrationBuilder.DropColumn(
                name: "AdmissionId",
                table: "OPDDrugus");

            migrationBuilder.AlterColumn<int>(
                name: "opdId",
                table: "OPDDrugus",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OPDDrugus_OPD_opdId",
                table: "OPDDrugus",
                column: "opdId",
                principalTable: "OPD",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OPDDrugus_OPD_opdId",
                table: "OPDDrugus");

            migrationBuilder.AlterColumn<int>(
                name: "opdId",
                table: "OPDDrugus",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AdmissionId",
                table: "OPDDrugus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OPDDrugus_OPD_opdId",
                table: "OPDDrugus",
                column: "opdId",
                principalTable: "OPD",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
