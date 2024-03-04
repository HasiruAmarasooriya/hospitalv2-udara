using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public int InvoiceID { get; set; }

        [ForeignKey("InvoiceID")]
        public Invoice? invoice { get; set; }
        public decimal CashAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DdebitAmount { get; set; }
        public decimal ChequeAmount { get; set; }
        public decimal GiftCardAmount { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public CashierStatus CashierStatus { get; set; }

        public BillingType BillingType { get; set; }
        public int sessionID { get; set; }

        [ForeignKey("sessionID")]
        public CashierSession? cashierSession { get; set; }
        [NotMapped]
        public decimal TotalAmountOld { get; set; }
        [NotMapped]
        public decimal TotalAmountNeedToPay { get; set; }

        [NotMapped]
        public decimal TotalAmountRefunded { get; set; }

        [NotMapped]
        public decimal TotalNotPaidAmount { get; set; }

        [NotMapped]
        public decimal TotalAmount { get; set; }

        [NotMapped]
        public decimal TotalRefund { get; set; }

        [NotMapped]
        public decimal TotalPaidAmount { get; set; }


    }
}
