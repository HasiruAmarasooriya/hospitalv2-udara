using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using System.Collections.Generic;

namespace HospitalMgrSystemUI.Models
{
    public class ChannelingSheduleDto
    {
        public List<Specialist> Specialists { get; set; }
        public List<Consultant> Consultants { get; set; }
        public List<Room> Rooms { get; set; }
        public ChannelingSchedule ChannelingSchedule { get; set; }
        public List<ChannelingSchedule> ChannelingScheduleList { get; set; }
        public string ConsultantName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int SpecialistId { get; set; }
        public ChannellingScheduleStatus channellingScheduleStatus { get; set; }
    }
}