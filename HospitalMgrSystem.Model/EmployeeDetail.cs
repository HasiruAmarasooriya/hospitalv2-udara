using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class EmployeeDetail
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? NIC { get; set; }
        public string? MobileNumber { get; set; }
        public string? TelephoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public SexStatus? Sex { get; set; }
        public Religion? Religion { get; set; }
        public int Nationality { get; set; }
        public string? Address { get; set; }
        public CommonStatus Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
