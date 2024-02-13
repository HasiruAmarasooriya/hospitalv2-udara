using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class ChannelingSchedule
    {
        public int Id { get; set; }
        public int NoOfAppointment { get; set; }
        public DateTime DateTime { get; set; }
        public int ConsultantId { get; set; }

        [ForeignKey("ConsultantId")]
        public Consultant? Consultant { get; set; }
        public decimal ConsultantFee { get; set; }
        public decimal HospitalFee { get; set; }
        public decimal OtherFee { get; set; }
        public CommonStatus Status { get; set; }
        public ChannellingScheduleStatus scheduleStatus { get; set; }
    }
}
