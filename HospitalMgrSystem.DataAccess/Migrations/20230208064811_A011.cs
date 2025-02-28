using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrugCategoryId",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "DrugSubCategoryId",
                table: "Drugs");

            migrationBuilder.AddColumn<int>(
                name: "DrugsCategoryId",
                table: "Drugs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DrugsSubCategoryId",
                table: "Drugs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_DrugsCategoryId",
                table: "Drugs",
                column: "DrugsCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Drugs_DrugsSubCategoryId",
                table: "Drugs",
                column: "DrugsSubCategoryId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_DrugsCategory_DrugsCategoryId",
                table: "Drugs");

            migrationBuilder.DropForeignKey(
                name: "FK_Drugs_DrugsSubCategory_DrugsSubCategoryId",
                table: "Drugs");

            migrationBuilder.DropIndex(
                name: "IX_Drugs_DrugsCategoryId",
                table: "Drugs");

            migrationBuilder.DropIndex(
                name: "IX_Drugs_DrugsSubCategoryId",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "DrugsCategoryId",
                table: "Drugs");

            migrationBuilder.DropColumn(
                name: "DrugsSubCategoryId",
                table: "Drugs");

            migrationBuilder.AddColumn<int>(
                name: "DrugCategoryId",
                table: "Drugs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DrugSubCategoryId",
                table: "Drugs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
