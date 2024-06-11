using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystemUI.Models
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string SearchValue { get; set; }
        public string NIC { get; set; }
        public string MobileNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public int Age { get; set; }
        public SexStatus Sex { get; set; }
        public int Religion { get; set; }
        public int Nationality { get; set; }
        public string Address { get; set; }
        public PatientStatus Status { get; set; }
      
    }
}
