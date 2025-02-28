using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A38 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OPD",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "OPD");
        }
    }
}
