using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalMgrSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class A131 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ReStockLevel",
                table: "Warehouse",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "AmountOfForwardBookingDtos",
                columns: table => new
                {
                    TotalPaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "CashierSessionDtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    cashierSessionStatus = table.Column<int>(type: "int", nullable: false),
                    StartingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartBalence = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EndBalence = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    userRole = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Variation = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierSessionDtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChannelingPaidReports",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SPName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullRefundCount = table.Column<int>(type: "int", nullable: false),
                    DoctorRefundCount = table.Column<int>(type: "int", nullable: false),
                    HospitalRefundCount = table.Column<int>(type: "int", nullable: false),
                    FullRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DoctorRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HospitalRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DoctorPaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HospitalPaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BillCount = table.Column<int>(type: "int", nullable: false),
                    DiscountCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ChannelingPaymentSummaryReportDtos",
                columns: table => new
                {
                    TotalPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRefund = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ChannelingRefundReportDtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    SchedularId = table.Column<int>(type: "int", nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefundedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefundedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelingRefundReportDtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClaimBillDtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IssuedCashier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsuntantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialist = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppoimentNo = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimBillDtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrugStoresDetailsDto",
                columns: table => new
                {
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockIn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockOut = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReStockLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ForwardBookingDataTableDtos",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GRPVDetailsDtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DrugId = table.Column<int>(type: "int", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GRNId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LogTranDTO",
                columns: table => new
                {
                    GrpvId = table.Column<int>(type: "int", nullable: false),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    DrugIdRef = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "OtherTransactionsDtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeneficiaryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OtherTransactionsStatus = table.Column<int>(type: "int", nullable: false),
                    ApprovedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherTransactionsDtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientsDataTableDtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    NIC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientsDataTableDtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentSummaryOfDoctorsOpddtos",
                columns: table => new
                {
                    ConsultantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillCount = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PreviousForwardBookingDataDtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpdId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppoimentNo = table.Column<int>(type: "int", nullable: false),
                    DocName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorPaidBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegisteredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousForwardBookingDataDtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestDetailsByIdDto",
                columns: table => new
                {
                    DrugID = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RequestDetailsDto",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    StockRequestItemID = table.Column<int>(type: "int", nullable: false),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RequestItemDetailsDto",
                columns: table => new
                {
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "StockQuantityDto",
                columns: table => new
                {
                    BatchNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockInQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockOutQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "StoresDetailsDto",
                columns: table => new
                {
                    DrugName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StockIn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockOut = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RefundQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailableQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmountOfForwardBookingDtos");

            migrationBuilder.DropTable(
                name: "CashierSessionDtos");

            migrationBuilder.DropTable(
                name: "ChannelingPaidReports");

            migrationBuilder.DropTable(
                name: "ChannelingPaymentSummaryReportDtos");

            migrationBuilder.DropTable(
                name: "ChannelingRefundReportDtos");

            migrationBuilder.DropTable(
                name: "ClaimBillDtos");

            migrationBuilder.DropTable(
                name: "DrugStoresDetailsDto");

            migrationBuilder.DropTable(
                name: "ForwardBookingDataTableDtos");

            migrationBuilder.DropTable(
                name: "GRPVDetailsDtos");

            migrationBuilder.DropTable(
                name: "LogTranDTO");

            migrationBuilder.DropTable(
                name: "OtherTransactionsDtos");

            migrationBuilder.DropTable(
                name: "PatientsDataTableDtos");

            migrationBuilder.DropTable(
                name: "PaymentSummaryOfDoctorsOpddtos");

            migrationBuilder.DropTable(
                name: "PreviousForwardBookingDataDtos");

            migrationBuilder.DropTable(
                name: "RequestDetailsByIdDto");

            migrationBuilder.DropTable(
                name: "RequestDetailsDto");

            migrationBuilder.DropTable(
                name: "RequestItemDetailsDto");

            migrationBuilder.DropTable(
                name: "StockQuantityDto");

            migrationBuilder.DropTable(
                name: "StoresDetailsDto");

            migrationBuilder.DropColumn(
                name: "ReStockLevel",
                table: "Warehouse");
        }
    }
}
