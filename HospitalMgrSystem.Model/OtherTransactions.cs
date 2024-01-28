using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class OtherTransactions
    {
        public int Id { get; set; }
        public int? SessionID { get; set; }

        [ForeignKey("SessionID")]
        public CashierSession? cashierSession { get; set; }

        public int? ConvenerID { get; set; }

        [ForeignKey("ConvenerID")]
        public User? Convener { get; set; }
        public InvoiceType InvoiceType { get; set; }    
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int? ApprovedByID { get; set; }

        [ForeignKey("ApprovedByID")]
        public User? ApprovedBy { get; set; }
        public CommonStatus Status { get; set; }
        public OtherTransactionsStatus otherTransactionsStatus { get; set; }
        public int? CreateUser { get; set; }
        public int? ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
