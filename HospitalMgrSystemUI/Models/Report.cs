using HospitalMgrSystem.Model;

namespace HospitalMgrSystemUI.Models
{
    public class Report
    {
        public int paidStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<OPD>? listopd { get; set; }

    }
}
