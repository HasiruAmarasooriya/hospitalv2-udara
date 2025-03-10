using HospitalMgrSystem.Model;


namespace HospitalMgrSystemUI.Models
{
    public class AddmisionCounsultantDto
    {
        public int AdmissionId { get; set; }
        public int ConsultantId { get; set; }
        public decimal HospitalFee { get; set; }
        public decimal DocFee { get; set; }
        public decimal Amount { get; set; }

    }
}
