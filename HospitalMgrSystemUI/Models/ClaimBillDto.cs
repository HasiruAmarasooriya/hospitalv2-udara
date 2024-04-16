using HospitalMgrSystem.Model;

namespace HospitalMgrSystemUI.Models
{
    public class ClaimBillDto
    {
        public ClaimBill claimBill { get; set; }
        public List<Patient>? patientsList { get; set; }
        public List<Consultant>? consultantsList { get; set; }
        public DateTime dateTime { get; set; }
    }
}
