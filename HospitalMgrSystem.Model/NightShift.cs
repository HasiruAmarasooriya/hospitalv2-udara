using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class NightShift
    {
        public int Id { get; set; }
        public Shift IsNightShift { get; set; }
    }
}
