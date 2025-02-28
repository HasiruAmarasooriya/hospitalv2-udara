using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;

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
        public DateTime PreviousDateTime { get; set; }
        public int SpecialistId { get; set; }
        public ChannellingScheduleStatus channellingScheduleStatus { get; set; }
        public List<Scan> channelingItems { get; set; }
        public string VideoId { get; set; }
        public string VideoUrl { get; set; }
    }
}