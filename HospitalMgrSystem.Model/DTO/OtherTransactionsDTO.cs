using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model.DTO
{
    public class OtherTransactionsDTO
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string BeneficiaryName { get; set; }
        public string Description { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreateDate { get; set; }
        public OtherTransactionsStatus OtherTransactionsStatus { get; set; }
        public string ApprovedBy { get; set; }
    }
}
