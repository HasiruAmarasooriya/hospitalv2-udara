using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public enum ChannellingScheduleStatus
    {
        NOT_ACTIVE,
        SESSION_START,
        SESSION_END,
        SESSION_CANCEL,
        PENDING
    }
}
