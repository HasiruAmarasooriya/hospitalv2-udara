using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class AdmissionInvestigation
    {
        public int Id { get; set; }
        public int AdmissionId { get; set; }
        public Admission? Admission { get; set; }
        public int InvestigationId { get; set; }
        public Investigation? Investigation { get; set; }
        public int Type { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public CommonStatus Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
