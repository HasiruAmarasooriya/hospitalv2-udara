using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class OtherTransactions
    {
        public int Id { get; set; }
        public int? SessionID { get; set; }
        public int? Convener { get; set; }
        public InvoiceType InvoiceType { get; set; }    // Invoice Type === OPD ? Service ID = OPD ID : null
        public decimal Amount { get; set; }
        public decimal Description { get; set; }
        public int? ApprovedBy { get; set; }
        public int? Status { get; set; }
        public int? CreateUser { get; set; }
        public int? ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
