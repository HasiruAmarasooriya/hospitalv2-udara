using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalMgrSystem.DataAccess.Migrations
{
    public partial class A019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Investigation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvestigationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvestigationCategoryId = table.Column<int>(type: "int", nullable: false),
                    InvestigationSubCategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateUser = table.Column<int>(type: "int", nullable: false),
                    ModifiedUser = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investigation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investigation_InvestigationCategory_InvestigationCategoryId",
                        column: x => x.InvestigationCategoryId,
                        principalTable: "InvestigationCategory",
                        principalColumn: "InvestigationCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Investigation_InvestigationSubCategory_InvestigationSubCategoryId",
                        column: x => x.InvestigationSubCategoryId,
                        principalTable: "InvestigationSubCategory",
                        principalColumn: "InvestigationSubCategoryId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Investigation_InvestigationCategoryId",
                table: "Investigation",
                column: "InvestigationCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Investigation_InvestigationSubCategoryId",
                table: "Investigation",
                column: "InvestigationSubCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Investigation");
        }
    }
}
