using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A55 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "InvoiceItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreateUser",
                table: "InvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "InvoiceItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedUser",
                table: "InvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "InvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "itemInvoiceStatus",
                table: "InvoiceItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "CreateUser",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "ModifiedUser",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "InvoiceItems");

            migrationBuilder.DropColumn(
                name: "itemInvoiceStatus",
                table: "InvoiceItems");
        }
    }
}
