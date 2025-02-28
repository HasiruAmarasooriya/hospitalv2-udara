using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public  class Invoice
    {
        public int Id { get; set; }
        public int? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public int ServiceID { get; set; }      // OPD ID, Admission Id, Channeling Id .....
        public InvoiceType InvoiceType { get; set; }    // Invoice Type === OPD ? Service ID = OPD ID : null
        public PaymentStatus paymentStatus { get; set; }
        public InvoiceStatus Status { get; set; }
 
        public int? CreateUser { get; set; }
        public int? ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int PrintCount { get; set; }
        public int IsDiscountAdded { get; set; }
    }
}
