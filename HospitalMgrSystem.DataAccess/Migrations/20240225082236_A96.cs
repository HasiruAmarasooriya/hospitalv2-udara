using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A96 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BeneficiaryID",
                table: "OtherTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OtherTransactions_BeneficiaryID",
                table: "OtherTransactions",
                column: "BeneficiaryID");

            migrationBuilder.AddForeignKey(
                name: "FK_OtherTransactions_Users_BeneficiaryID",
                table: "OtherTransactions",
                column: "BeneficiaryID",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OtherTransactions_Users_BeneficiaryID",
                table: "OtherTransactions");

            migrationBuilder.DropIndex(
                name: "IX_OtherTransactions_BeneficiaryID",
                table: "OtherTransactions");

            migrationBuilder.DropColumn(
                name: "BeneficiaryID",
                table: "OtherTransactions");
        }
    }
}
