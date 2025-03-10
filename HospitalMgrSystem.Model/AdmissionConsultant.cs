using System.ComponentModel.DataAnnotations.Schema;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class AdmissionConsultant
    {
        public int Id { get; set; }
        public int AdmissionId { get; set; }
        [ForeignKey("AdmissionId")]public Admission? Admission { get; set; }
        public int ConsultantId { get; set; }
        [ForeignKey("ConsultantId")]public Consultant? Consultant { get; set; }
        public int Type { get; set; }
        public decimal HospitalFee { get; set; }
        public decimal DoctorFee { get; set; }
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
        public string ConsultantName { get; set; }
        [NotMapped]
        public decimal discount { get; set; }
    }
}
