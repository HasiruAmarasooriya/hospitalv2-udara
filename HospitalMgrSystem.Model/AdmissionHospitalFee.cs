using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class AdmissionHospitalFee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DefaultStatus IsDefault { get; set; }
        public CommonStatus Status { get; set; }
        public AdmissionPerDayStatus PerDayStatus { get; set; }
        public string Description { get; set; }

        public int CreateBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
