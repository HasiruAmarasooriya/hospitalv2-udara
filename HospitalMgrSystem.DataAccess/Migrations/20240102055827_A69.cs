using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A69 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "shift",
                table: "OPD",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NightShifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsNightShift = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NightShifts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NightShifts");

            migrationBuilder.DropColumn(
                name: "shift",
                table: "OPD");
        }
    }
}
