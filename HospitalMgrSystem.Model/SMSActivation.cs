using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class SMSActivation
    {
        public int Id { get; set; }
        public SMSStatus isActivate { get; set; }
    }
}
