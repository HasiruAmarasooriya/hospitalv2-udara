using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A85 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SMSAPILogin",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemainingCount = table.Column<int>(type: "int", nullable: false),
                    Expiration = table.Column<int>(type: "int", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSAPILogin", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "SMSCampaign",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampaignCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    duplicateNo = table.Column<int>(type: "int", nullable: false),
                    invaliedNo = table.Column<int>(type: "int", nullable: false),
                    maskBlockedUser = table.Column<int>(type: "int", nullable: false),
                    sceduleID = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMSCampaign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SMSCampaign_ChannelingSchedule_sceduleID",
                        column: x => x.sceduleID,
                        principalTable: "ChannelingSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SMSCampaign_sceduleID",
                table: "SMSCampaign",
                column: "sceduleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMSAPILogin");

            migrationBuilder.DropTable(
                name: "SMSCampaign");
        }
    }
}
