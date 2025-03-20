using Microsoft.EntityFrameworkCore;
using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using Microsoft.Extensions.Configuration;

namespace HospitalMgrSystem.DataAccess
{
	public static class ConnectionStrings
	{
		public static string DEVELOPMENT_DATABASE { get; } =
			"Data Source=172.201.169.155;Initial Catalog=KUMUDU1;User ID=kumudu;Password=z/api)/c>iTKB4#%lbN;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;";
		//new Production database
		public static string PRODUCTION_DATABASE { get; } =
            "Data Source=mssql-194208-0.cloudclusters.net,19006;Initial Catalog=Kumudu_Pod;User ID=kumudu_user;Password=HuQAGh72;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;";
    //Old Poduction Data base
		//    public static string PRODUCTION_DATABASE { get; } =
    //        "Data Source=172.201.169.155;Initial Catalog=KUMUDU;User ID=kumudu;Password=z/api)/c>iTKB4#%lbN;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;";
    }


	public class HospitalDBContext : DbContext
	{
		public static IConfiguration Configuration { get; set; }

		public DbSet<User> Users { get; set; }
		public DbSet<Patient> Patients { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<Specialist> Specialists { get; set; }
		public DbSet<Consultant> Consultants { get; set; }
		public DbSet<Admission> Admissions { get; set; }
		public DbSet<Drug> Drugs { get; set; }
		public DbSet<DrugsCategory> DrugsCategory { get; set; }
		public DbSet<DrugsSubCategory> DrugsSubCategory { get; set; }
		public DbSet<Investigation> Investigation { get; set; }
		public DbSet<InvestigationCategory> InvestigationCategory { get; set; }
		public DbSet<InvestigationSubCategory> InvestigationSubCategory { get; set; }
		public DbSet<Item> Item { get; set; }
		public DbSet<ItemCategory> ItemCategory { get; set; }
		public DbSet<ItemSubCategory> ItemSubCategory { get; set; }
		public DbSet<AdmissionDrugus> AdmissionDrugus { get; set; }
		public DbSet<AdmissionInvestigation> AdmissionInvestigations { get; set; }
		public DbSet<AdmissionConsultant> AdmissionConsultants { get; set; }
		public DbSet<AdmissionItems> AdmissionItems { get; set; }
		public DbSet<ChannelingSchedule> ChannelingSchedule { get; set; }
		public DbSet<OPD> OPD { get; set; }
		public DbSet<OPDDrugus> OPDDrugus { get; set; }
		public DbSet<OPDInvestigation> OPDInvestigations { get; set; }
		public DbSet<OPDItem> OPDItems { get; set; }
		public DbSet<Invoice> Invoices { get; set; }
		public DbSet<InvoiceItem> InvoiceItems { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<Channeling> Channels { get; set; }
		public DbSet<EmployeeDetail> employeeDetails { get; set; }

		public DbSet<HospitalFee> HospitalFee { get; set; }
		public DbSet<OPDConsultantFee> OPDConsultantFee { get; set; }

		public DbSet<OPDScheduler> OPDScheduler { get; set; }
		
		public DbSet<NightShift> NightShifts { get; set; }
		public DbSet<CashierSession> CashierSessions { get; set; }

		public DbSet<OtherTransactions> OtherTransactions { get; set; }

		public DbSet<NightShiftSession> NtShiftSessions { get; set; }

		public DbSet<SMSAPILogin> SMSAPILogin { get; set; }
		public DbSet<SMSCampaign> SMSCampaign { get; set; }
		public DbSet<SMSmsg> SMSmsg { get; set; }
		public DbSet<Scan> ChannelingItems { get; set; }
		public DbSet<ClaimBill> ClaimBills { get; set; }

		public DbSet<SMSActivation> sMSActivations { get; set; }
		public DbSet<ClaimBillItems> ClaimBillItemsData { get; set; }
		public DbSet<Discount> Discounts { get; set; }
		public DbSet<Video> Video { get; set; }
		public DbSet<Warehouse> Warehouse { get; set; }
		public DbSet<GRN> GRN { get; set; }
		public DbSet<GRPV> GRPV {  get; set; }
        public DbSet<stockTransaction> stockTransaction { get; set; }
        public DbSet<StockRequest> StockRequest { get; set; }
        public DbSet<StockRequestItem> StockRequestItem { get; set; }
        public DbSet<AdmissionHospitalFee> AdmissionHospitalFees { get; set; }
		public DbSet<AdmissionsCharges> AdmissionsCharges { get; set; }

        #region DTOs

        public DbSet<AppointmentDTO> AppointmentsDTO { get; set; }
		public DbSet<ReportOpdXrayOtherPaidDto> ReportOpdXrayOtherPaidDtos { get; set; }
		public DbSet<ReportOpdXrayOtherRefundDTO> ReportOpdXrayOtherRefundDTOs { get; set; }
		public DbSet<PaymentSummaryOpdXrayOtherDTO> PaymentSummaryOpdXrayOtherDtos { get; set; }
		public DbSet<ReportOpdXrayOtherDrugs> ReportOpdXrayOtherDrugsDtos { get; set; }
		public DbSet<OpdOtherXrayDataTableDto> OpdOtherXrayDataTableDtos { get; set; }
		public DbSet<ClaimBillDto> ClaimBillDtos { get; set; }
		public DbSet<PaymentSummaryOfDoctorsOPDDTO> PaymentSummaryOfDoctorsOpddtos { get; set; }
		public DbSet<ForwardBookingDataTableDTO> ForwardBookingDataTableDtos { get; set; }
		public DbSet<TotalPaidAmountOfForwardBookingDTO> AmountOfForwardBookingDtos { get; set; }
		public DbSet<OtherTransactionsDTO> OtherTransactionsDtos { get; set; }
		public DbSet<PatientsDataTableDTO> PatientsDataTableDtos { get; set; }
		public DbSet<CashierSessionDTO> CashierSessionDtos { get; set; }
		public DbSet<ChannelingPaidReport> ChannelingPaidReports { get; set; }
		public DbSet<PreviousForwardBookingDataDto> PreviousForwardBookingDataDtos { get; set; }
		public DbSet<ChannelingRefundReportDto> ChannelingRefundReportDtos { get; set; }
		public DbSet<ChannelingPaymentSummaryReportDto> ChannelingPaymentSummaryReportDtos { get; set; }
        public DbSet<DiscountTableReport> DiscountTableReports { get; set; }
       
        public DbSet<GRPVDetailsDto> GRPVDetailsDtos { get; set; }
        public DbSet<LogTranDTO> LogTranDTO { get; set; }
        public DbSet<RequestDetailsDto> RequestDetailsDto { get; set; }
        public DbSet<RequestItemDetailsDto> RequestItemDetailsDto { get; set; }
        public DbSet<DrugStoresDetailsDto> DrugStoresDetailsDto { get; set; }
        public DbSet<StockQuantityDto> StockQuantityDto { get; set; }
        public DbSet<RequestDetailsByIdDto> RequestDetailsByIdDto { get; set; }
        public DbSet<StoresDetailsDto> StoresDetailsDto { get; set; }
        public DbSet<HospitalFeeListDto> HospitalFeeList { get; set; }
     
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(ConnectionStrings.PRODUCTION_DATABASE);
		}
        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
           {
               base.OnModelCreating(modelBuilder);

               #region Ignoring DTOs

               modelBuilder.Ignore<ClaimBillDto>();
               modelBuilder.Ignore<PaymentSummaryOfDoctorsOPDDTO>();
               modelBuilder.Ignore<ForwardBookingDataTableDTO>();
               modelBuilder.Ignore<TotalPaidAmountOfForwardBookingDTO>();
               modelBuilder.Ignore<OtherTransactionsDTO>();
               modelBuilder.Ignore<PatientsDataTableDTO>();
               modelBuilder.Ignore<CashierSessionDTO>();
               modelBuilder.Ignore<ChannelingPaidReport>();
               modelBuilder.Ignore<PreviousForwardBookingDataDto>();
               modelBuilder.Ignore<ChannelingRefundReportDto>();
               modelBuilder.Ignore<ChannelingPaymentSummaryReportDto>();
               modelBuilder.Ignore<GRPVDetailsDto>();
               modelBuilder.Ignore<LogTranDTO>();
               modelBuilder.Ignore<RequestDetailsDto>();
               modelBuilder.Ignore<RequestItemDetailsDto>();
               modelBuilder.Ignore<DrugStoresDetailsDto>();
               modelBuilder.Ignore<StockQuantityDto>();
               modelBuilder.Ignore<RequestDetailsByIdDto>();
               modelBuilder.Ignore<StoresDetailsDto>();
		       modelBuilder.Ignore<HospitalFeeListDto>();
              
               #endregion
           }*/
    }
}