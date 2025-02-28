using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class Channeling
    {
        public int Id { get; set; }
        public int PatientID { get; set; }

        [ForeignKey("PatientID")]
        public Patient? Patient { get; set; }

        public int ChannelingScheduleID { get; set; }

        [ForeignKey("ChannelingScheduleID")]
        public ChannelingSchedule? ChannelingSchedule { get; set; }
        public int AppointmentNo { get; set; }
        public CommonStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
