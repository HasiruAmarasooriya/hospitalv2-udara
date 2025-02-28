using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A86 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "SMSAPILogin",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SMSAPILogin",
                newName: "id");
        }
    }
}
