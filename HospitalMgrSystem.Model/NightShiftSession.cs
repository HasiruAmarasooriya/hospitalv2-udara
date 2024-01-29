using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class NightShiftSession
    {
        public int Id { get; set; }
        public int userID { get; set; }

        [ForeignKey("userID")]
        public User? User { get; set; }

        public DateTime StartingTime { get; set; }
        public DateTime EndTime { get; set; }

        public ShiftSessionStatus shiftSessionStatus { get; set; }

        public Shift shift { get; set; }

        public CommonStatus Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
