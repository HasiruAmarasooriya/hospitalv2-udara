using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Investigation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Investigation",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
