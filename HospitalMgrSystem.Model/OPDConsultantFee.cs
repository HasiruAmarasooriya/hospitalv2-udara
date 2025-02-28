using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class OPDConsultantFee
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DefaultStatus IsDefault { get; set; }
        public CommonStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
