using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ItemID { get; set; }
        public BillingItemsType billingItemsType { get; set; }
        public decimal price { get; set; }
        public decimal qty { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public ItemInvoiceStatus itemInvoiceStatus { get; set; }
        public CommonStatus Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
