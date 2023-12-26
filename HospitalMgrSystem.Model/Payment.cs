using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public int InvoiceID { get; set; }
        public decimal CashAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DdebitAmount { get; set; }
        public decimal ChequeAmount { get; set; }
        public decimal GiftCardAmount { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
