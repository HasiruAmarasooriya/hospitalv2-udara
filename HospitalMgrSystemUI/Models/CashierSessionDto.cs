using HospitalMgrSystem.Model;

namespace HospitalMgrSystemUI.Models
{
    public class CashierSessionDto
    {

        public List<CashierSession> CashierSessions { get; set; }
        public int cashierSessionID { get; set; }
        public CashierSession cashierSession { get; set; }
        public CashierSession CashierPaymentData { get; set; }
        public User user { get; set; }
        public DateTime sessionDate { get; set; }
        public int sessionId { get; set; }
    }
}
