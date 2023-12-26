using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class OPDScheduler
    {
        public int Id { get; set; }
        public int ConsultantId { get; set; }

        [ForeignKey("ConsultantId")]
        public Consultant? Consultant { get; set; }
        public OPDScheduleStatus OPDSchedulerStatus { get; set; }
        public OPDSession OPDSession { get; set; }

        public DateTime cDate { get; set; }

        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int isActiveSession { get; set; }
        public int isPaid { get; set; }
        public CommonStatus Status { get; set; }

        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
