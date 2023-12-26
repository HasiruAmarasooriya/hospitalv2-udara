using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrugsCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugsCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrugsSubCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrugsCategoryId = table.Column<int>(type: "int", nullable: true),
                    DrugsSubCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugsSubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrugsSubCategory_DrugsCategory_DrugsCategoryId",
                        column: x => x.DrugsCategoryId,
                        principalTable: "DrugsCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrugsSubCategory_DrugsCategoryId",
                table: "DrugsSubCategory",
                column: "DrugsCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugsSubCategory");

            migrationBuilder.DropTable(
                name: "DrugsCategory");
        }
    }
}
