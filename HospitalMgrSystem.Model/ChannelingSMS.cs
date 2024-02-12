using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class ChannelingSMS
    {
        public List<Channeling> channelings { get; set; }
        public ChannelingSchedule channelingSchedule { get; set; }

        public ChannellingScheduleStatus ChannellingScheduleStatus { get; set; }

    }
}
