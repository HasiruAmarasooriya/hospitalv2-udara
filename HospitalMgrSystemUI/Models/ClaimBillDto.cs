using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystemUI.Models
{
    public class ClaimBillDto
    {
        public ClaimBill? claimBill { get; set; }
        public Patient? patient { get; set; }
        public List<HospitalMgrSystem.Model.DTO.ClaimBillDto>? claimBillDtos { get; set; }
		public List<Patient>? patientsList { get; set; }
        public List<Consultant>? consultantsList { get; set; }
        public List<Scan>? Scans { get; set; }
        public List<Drug>? Drugs { get; set; }
        public List<ClaimBillItems>? ClaimBillItemsList { get; set; }
		public DateTime dateTime { get; set; }
        public string PatientName { get; set; }
        public string RefNo { get; set; }
        public string ContactNumber { get; set; }
        public string NIC { get; set; }
        public ClaimBillCategoryType ClaimBillCategory { get; set; }
        public string ClaimBillCategoryString { get; set; }
		public int ConsultantId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Cash { get; set; }
        public decimal Balance { get; set; }
        public decimal HospitalFee { get; set; }
        public decimal ConsultantFee { get; set; }
    }
}
