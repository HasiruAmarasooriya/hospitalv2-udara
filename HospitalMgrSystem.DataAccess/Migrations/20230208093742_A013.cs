using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_DrugsCategory_DrugsCategoryId",
                table: "Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_DrugsSubCategory_DrugsSubCategoryId",
                table: "Drugs");

            migrationBuilder.AlterColumn<int>(
                name: "DrugsSubCategoryId",
                table: "Drugs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DrugsCategoryId",
                table: "Drugs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_DrugsCategory_DrugsCategoryId",
                table: "Drugs",
                column: "DrugsCategoryId",
                principalTable: "DrugsCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_DrugsSubCategory_DrugsSubCategoryId",
                table: "Drugs",
                column: "DrugsSubCategoryId",
                principalTable: "DrugsSubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_DrugsCategory_DrugsCategoryId",
                table: "Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_DrugsSubCategory_DrugsSubCategoryId",
                table: "Drugs");

            migrationBuilder.AlterColumn<int>(
                name: "DrugsSubCategoryId",
                table: "Drugs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DrugsCategoryId",
                table: "Drugs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_DrugsCategory_DrugsCategoryId",
                table: "Drugs",
                column: "DrugsCategoryId",
                principalTable: "DrugsCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Drugs_DrugsSubCategory_DrugsSubCategoryId",
                table: "Drugs",
                column: "DrugsSubCategoryId",
                principalTable: "DrugsSubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
