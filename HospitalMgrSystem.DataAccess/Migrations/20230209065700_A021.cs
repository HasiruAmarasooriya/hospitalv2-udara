using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemCategory",
                columns: table => new
                {
                    ItemCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategory", x => x.ItemCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ItemSubCategory",
                columns: table => new
                {
                    ItemSubCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemCategoryId = table.Column<int>(type: "int", nullable: false),
                    ItemSubCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSubCategory", x => x.ItemSubCategoryId);
                    table.ForeignKey(
                        name: "FK_ItemSubCategory_ItemCategory_ItemCategoryId",
                        column: x => x.ItemCategoryId,
                        principalTable: "ItemCategory",
                        principalColumn: "ItemCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemSubCategory_ItemCategoryId",
                table: "ItemSubCategory",
                column: "ItemCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemSubCategory");

            migrationBuilder.DropTable(
                name: "ItemCategory");
        }
    }
}
