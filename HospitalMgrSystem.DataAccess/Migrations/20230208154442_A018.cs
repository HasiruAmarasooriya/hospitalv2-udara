using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A018 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestigationCategory",
                columns: table => new
                {
                    InvestigationCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Investigation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationCategory", x => x.InvestigationCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "InvestigationSubCategory",
                columns: table => new
                {
                    InvestigationSubCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestigationCategoryId = table.Column<int>(type: "int", nullable: false),
                    InvestigationSubCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestigationSubCategory", x => x.InvestigationSubCategoryId);
                    table.ForeignKey(
                        name: "FK_InvestigationSubCategory_InvestigationCategory_InvestigationCategoryId",
                        column: x => x.InvestigationCategoryId,
                        principalTable: "InvestigationCategory",
                        principalColumn: "InvestigationCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestigationSubCategory_InvestigationCategoryId",
                table: "InvestigationSubCategory",
                column: "InvestigationCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestigationSubCategory");

            migrationBuilder.DropTable(
                name: "InvestigationCategory");
        }
    }
}
