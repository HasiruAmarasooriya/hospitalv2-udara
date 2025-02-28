using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A61 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Consultants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Consultants",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
