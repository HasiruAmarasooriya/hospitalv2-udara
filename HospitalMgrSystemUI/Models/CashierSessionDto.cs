using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;

namespace HospitalMgrSystemUI.Models
{
    public class CashierSessionDto
    {

        public List<CashierSession> CashierSessions { get; set; }
        public List<CashierSessionDTO> CashierSessionDtos { get; set; }
        public int cashierSessionID { get; set; }
        public CashierSession cashierSession { get; set; }
        public CashierSession CashierPaymentData { get; set; }
        public List<ForwardBookingDataTableDTO>? ForwardBookingData { get; set; }
        public TotalPaidAmountOfForwardBookingDTO? AmountOfForwardBookingDto { get; set; }
		public User user { get; set; }
        public DateTime sessionDate { get; set; }
        public DateTime printDate { get; set; }
        public int sessionId { get; set; }
    }
}
