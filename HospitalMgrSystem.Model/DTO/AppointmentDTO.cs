using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model.DTO
{

    public class AppointmentDTO
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public int ConsultantID { get; set; }
        public int RoomID { get; set; }
        public int AppoimentNo { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Status { get; set; }
        public decimal ConsultantFee { get; set; }
        public decimal HospitalFee { get; set; }
        public decimal OtherFee { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int IsOnOPD { get; set; }
        public int CreatedUser { get; set; }
        public int ModifiedUser { get; set; }
        public int shiftID { get; set; }
        public int invoiceType { get; set; }
        public int schedularId { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }

        public string MobileNumber { get; set; }
        public string ConsultantName { get; set; }
        public string SpecialistName { get; set; }
        public string? ItemIds { get; set; }
        public DateTime CSDate { get; set; }
        public ChannellingScheduleStatus CSStatus { get; set; }

    }
}
