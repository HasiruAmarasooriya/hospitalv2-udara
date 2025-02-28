using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A016 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugsSubCategory_DrugsCategory_DrugsCategoryId",
                table: "DrugsSubCategory");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DrugsSubCategory",
                newName: "DrugsSubCategoryId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DrugsCategory",
                newName: "DrugsCategoryId");

            migrationBuilder.AlterColumn<int>(
                name: "DrugsCategoryId",
                table: "DrugsSubCategory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DrugsSubCategory_DrugsCategory_DrugsCategoryId",
                table: "DrugsSubCategory",
                column: "DrugsCategoryId",
                principalTable: "DrugsCategory",
                principalColumn: "DrugsCategoryId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrugsSubCategory_DrugsCategory_DrugsCategoryId",
                table: "DrugsSubCategory");

            migrationBuilder.RenameColumn(
                name: "DrugsSubCategoryId",
                table: "DrugsSubCategory",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DrugsCategoryId",
                table: "DrugsCategory",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "DrugsCategoryId",
                table: "DrugsSubCategory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_DrugsSubCategory_DrugsCategory_DrugsCategoryId",
                table: "DrugsSubCategory",
                column: "DrugsCategoryId",
                principalTable: "DrugsCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
