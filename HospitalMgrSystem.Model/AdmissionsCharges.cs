using System.ComponentModel.DataAnnotations.Schema;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class AdmissionsCharges
    {
        public int Id { get; set; }
        public int AdmissionId { get; set; }
        [ForeignKey("AdmissionId")]
        public Admission? Admission { get; set; }
        public int HospitalFeeId { get; set; }
        [ForeignKey("HospitalFeeId")]
        public AdmissionHospitalFee? Item { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public CommonStatus Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
         public PaymentStatus paymentStatus { get; set; }
        public ItemInvoiceStatus itemInvoiceStatus { get; set; }
        public int IsRefund { get; set; }
        [NotMapped]
        public string ItemName { get; set; }

    }
}
