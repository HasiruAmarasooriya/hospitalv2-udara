using HospitalMgrSystem.Model;
using System.Collections.Generic;

namespace HospitalMgrSystemUI.Models
{
    public class ChannelingSheduleDto
    {
        public List<Specialist> Specialists { get; set; }
        public List<Consultant> Consultants { get; set; }
        public ChannelingSchedule ChannelingSchedule { get; set; }
        public List<ChannelingSchedule> ChannelingScheduleList { get; set; }
        public string ConsultantName { get; set; }
    }
}
