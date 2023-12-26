using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystemUI.Models
{
    public class OPDSchedulerDto
    {
        public List<Consultant>? Consultants { get; set; }
        public OPDScheduler? OPDSchedule { get; set; }
        public List<OPDScheduler>? OPDSchedulerList { get; set; }
        public int opdShedularID { get; set; }
        public DateTime cDate { get; set; }
        public int opdSchefulerIDMo { get; set; }
        public int opdSchefulerIDDa { get; set; }
        public int opdSchefulerIDNi { get; set; }
        public OPDScheduleStatus OPDSchedulerStatusMo { get; set; }
        public OPDScheduleStatus OPDSchedulerStatusDa { get; set; }
        public OPDScheduleStatus OPDSchedulerStatusNi { get; set; }

        public DateTime? startTimeMo { get; set; }
        public DateTime? startTimeDay { get; set; }
        public DateTime? startTimeNi { get; set; }

        public DateTime? endTimeMo { get; set; }
        public DateTime? endTimeDay { get; set; }
        public DateTime? endTimeNi { get; set; }
        public int activeMo { get; set; }
        public int activeDa { get; set; }
        public int activeNi { get; set; }

        public int OPDSheduleMoID { get; set; }
        public int OPDSheduleDaID { get; set; }
        public int OPDSheduleNiID { get; set; }

        public int DrMoID { get; set; }
        public int DrDaID { get; set; }
        public int DrNiID { get; set; }
       
        public OPDSession OPDSessionMo { get; set; }
        public OPDSession OPDSessionDa { get; set; }
        public OPDSession OPDSessionNi { get; set; }
    }
}
