using System.ComponentModel.DataAnnotations.Schema;
using HospitalMgrSystem.Model.DTO;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class Admission
    {
        public int Id { get; set; }
        public string BHTNumber { get; set; }
        public string DateAdmission { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")] public Room? Room { get; set; }
        public int ConsultantId { get; set; }
        [ForeignKey("ConsultantId")] public Consultant? Consultant { get; set; }
        public int PatientId { get; set; }
        [ForeignKey("PatientId")] public Patient? Patient { get; set; }

        public string Guardian { get; set; }
        public decimal Temp { get; set; }
        public string Pluse { get; set; }
        public string Resp { get; set; }
        public string Weight { get; set; }
        public string BP { get; set; }
        public string Details { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public AdmissionStatus Status { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public InvoiceType invoiceType { get; set; }
        public ItemInvoiceStatus itemInvoiceStatus { get; set; }
        public int IsRefund { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime DischargeDate { get; set; }
        public string? Discription { get; set; }
        [NotMapped] public decimal ConsultantFee { get; set; }
        [NotMapped] public decimal HospitalFee { get; set; }
        [NotMapped] public decimal? TotalRefund { get; set; }
        [NotMapped] public decimal? TotalNeedToRefund { get; set; }
        [NotMapped] public decimal? TotalOldAmount { get; set; }
        [NotMapped] public decimal? TotalAmount { get; set; }
        [NotMapped]public string? PatientName { get; set; }
       
        [NotMapped] public List<HospitalFeeListDto> HospitalFeeList { get; set; }
    }
}
