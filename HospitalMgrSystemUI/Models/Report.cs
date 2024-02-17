using HospitalMgrSystem.Model;

namespace HospitalMgrSystemUI.Models
{
    public class Report
    {
        public int paidStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int InvoicedType { get; set; }
        public List<OPD>? listopd { get; set; }
        public List<OPD>? listChanneling { get; set; }
        public List<OPD>? listPartialPaidOPD { get; set; }
        public List<OPD>? listPartialPaidChanneling { get; set; }
        public List<OPD>? listNeedToPayOPD { get; set; }
        public List<OPD>? listNeedToPayChanneling { get; set; }
        public List<OPD>? listNightShiftOPD { get; set; }
        public List<OPD>? listNightShiftChanneling { get; set; }
        public List<OPD>? listNotPaidOPD { get; set; }
        public List<OPD>? listNotPaidChanneling { get; set; }
        public Payment? paymentData { get; set; }
        public List<OPDDrugus>? listopdGrugs { get; set; }

    }
}
