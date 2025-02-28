using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model.DTO
{
    public class PatientsDataTableDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string NIC { get; set; }
        public SexStatus Sex { get; set; }
        public string MobileNumber { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
