using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NIC { get; set; }
        public string MobileNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public int Age { get; set; }
        public int Sex { get; set; }
        public int Religion { get; set; }
        public int Nationality { get; set; }
        public string Address { get; set; }
        public PatientStatus Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
