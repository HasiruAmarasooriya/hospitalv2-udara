using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Model;

namespace HospitalMgrSystemUI.Models
{
    public class OPDTbDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string roomName { get; set; }
        public string consaltantName { get; set; }
        public int consaltantId { get; set; }
        public Specialist specialistData { get; set; }
        public string? Description { get; set; }
        public SexStatus Sex { get; set; }
        public string MobileNumber { get; set; }
        public int AppoimentNo { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public CommonStatus Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public int schedularId { get; set; }
        public ChannelingSchedule channelingScheduleData { get; set; }
        public int? isRefund { get; set; }
        public string? refundedItem { get; set; }
    }
}
