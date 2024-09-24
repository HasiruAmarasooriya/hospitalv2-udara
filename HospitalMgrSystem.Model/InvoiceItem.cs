using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }

        [ForeignKey("InvoiceId")] public Invoice? invoice { get; set; }
        public int ItemID { get; set; }
        public BillingItemsType billingItemsType { get; set; }
        public decimal price { get; set; }
        public decimal qty { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; } // Total < OPDDrugs + HospitalFee ? PaidStatus = NEED_TO_PAY : PaidStatus = NOT_PAID
        public ItemInvoiceStatus itemInvoiceStatus { get; set; } // Billed
        public CommonStatus Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public decimal PrevPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}