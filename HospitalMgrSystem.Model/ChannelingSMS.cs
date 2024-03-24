using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class ChannelingSMS
    {
        public List<OPD> channeling { get; set; }
        public OPD channelingForOnePatient { get; set; }
        public ChannelingSchedule channelingSchedule { get; set; }

        public ChannellingScheduleStatus ChannellingScheduleStatus { get; set; }

    }
}
