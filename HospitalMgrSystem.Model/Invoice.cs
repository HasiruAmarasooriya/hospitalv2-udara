using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public  class Invoice
    {
        public int Id { get; set; }
        public int? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public int ServiceID { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public InvoiceStatus Status { get; set; }
 
        public int? CreateUser { get; set; }
        public int? ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
