using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model.Enums
{
    public enum ChannellingScheduleStatus
    {
        NOT_ACTIVE,
        ACTIVE,
        SESSION_START, 
        SESSION_END,
        SESSION_CANCEL, 
        PENDING,
        ALL
    }
}
