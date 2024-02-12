using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HospitalMgrSystem.Model;

namespace HospitalMgrSystem.DataAccess
{
    public class HospitalDBContext : DbContext
    {
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
        public DbSet<stockTransaction> stockTransaction { get; set; }
        public DbSet<NightShift> NightShifts { get; set; }
        public DbSet<CashierSession> CashierSessions { get; set; }

        public DbSet<OtherTransactions> OtherTransactions { get; set; }

        public DbSet<NightShiftSession> NtShiftSessions { get; set; }

        public DbSet<SMSAPILogin> SMSAPILogin { get; set; }
        public DbSet<SMSCampaign> SMSCampaign { get; set; }
        public DbSet<SMSmsg> SMSmsg { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=cebdbserver.database.windows.net;Database=KUMUDU; User Id=cebuser; Password=Anubaba@1234";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
