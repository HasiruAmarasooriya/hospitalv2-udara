using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystemUI.Models
{
    public class CashierDto
    {
        public string PreID { get; set; }
        public int customerID { get; set; }
        public int sufID { get; set; }
        public int invoiceID { get; set; }
        public string customerName { get; set; }
        public string consaltantName { get; set; }
        public int patientAge { get; set; }
        public SexStatus patientSex { get; set; }
        public string patientContactNo { get; set; }
        public string? patientNIC { get; set; }
        public InvoiceType invoiceType { get; set; }

        public UserRole userRole { get; set; }
        public int ID { get; set; }
        public decimal subtotal { get; set; }
        public decimal preSubtotal { get; set; }
        public decimal discount { get; set; }
        public bool discountEnabled { get; set; }
        public decimal total { get; set; }
        public decimal preTotal { get; set; }
        public decimal totalPaidAmount { get; set; }
        public decimal totalDueAmount { get; set; }
        public decimal totalPaymentPaidAmount { get; set; }
        public decimal totalPaymentDueAmount { get; set; }
        public decimal cash { get; set; }
        public decimal credit { get; set; }
        public decimal debit { get; set; }
        public decimal cheque { get; set; }
        public decimal giftCard { get; set; }
        public string userPassword { get; set; }

        public decimal hospitalFee { get; set; }
        public decimal consaltantFee { get; set; }

        public decimal refunfAmount { get; set; }
        public string BillingType { get; set; }
        public decimal AvailableDiscount { get; set; }

        public decimal defaultAmountOPD { get; set; }
        public OPD opd { get; set; }
        public Invoice invoice { get; set; }
        public List<OPDDrugus> OPDDrugusList { get; set; }
        public List<OPDDrugus> OPDDrugusListInvoiced { get; set; }
        public List<OPDInvestigation> OPDInvestigationList { get; set; }
        public List<OPDItem> OPDItemList { get; set; }

        public List<BillingItemDto> cashierBillingItemDtoList { get; set; }
        public List<BillingItemDto> cashierBilledItemDtoList { get; set; }
        public List<BillingItemDto> cashierRemoveBillingItemDtoList { get; set; }
        public List<InvoiceItem> InvoiceItemList { get; set; }
        public List<Payment> paymentList { get; set; }
        public Payment payment { get; set; }

        public Channeling channel{ get; set; }
        public ChannelingSchedule ChannelingSchedule { get; set; }
        public string? ItemName { get; set; }
        public int PrintCount { get; set; }
        public OPD? OpdData { get; set; }
        public List<Scan>? ScanList { get; set; }
        public Scan? ScanItem { get; set; }
    }
}
