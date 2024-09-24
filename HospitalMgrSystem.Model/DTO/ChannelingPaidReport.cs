using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Model.DTO
{
    [Keyless]
    public class ChannelingPaidReport
    {
        public int ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public string DocName { get; set; }
        public string SPName { get; set; }
        public int FullRefundCount { get; set; }
        public int DoctorRefundCount { get; set; }
        public int HospitalRefundCount { get; set; }
        public decimal FullRefundAmount { get; set; }
        public decimal DoctorRefundAmount { get; set; }
        public decimal HospitalRefundAmount { get; set; }
        public decimal DoctorPaidAmount { get; set; }
        public decimal HospitalPaidAmount { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public int BillCount { get; set; }
    }
}
