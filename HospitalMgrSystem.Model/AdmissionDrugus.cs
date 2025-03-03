using System.ComponentModel.DataAnnotations.Schema;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class AdmissionDrugus
    {
        public int Id { get; set; }
        public int AdmissionId { get; set; }
        public Admission? Admission { get; set; }
        public int DrugId { get; set; }
        public Drug? Drug { get; set; }
        public int Type { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public CommonStatus Status { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public ItemInvoiceStatus itemInvoiceStatus { get; set; }
        public int IsRefund { get; set; }
        [NotMapped] 
        public string DrugName { get; set; }


    }
}
