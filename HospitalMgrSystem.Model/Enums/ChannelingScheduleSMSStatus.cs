using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model.Enums
{
    public enum ChannelingScheduleSMSStatus
    {
        NOT_YET,
        SESSION_START_SMS_SENT,
        SESSION_END_SMS_SENT,
        SESSION_PENDING_SMS_SENT,
        SESSION_CANCEL_SMS_SENT,
        SESSIOM_TIME_CHANGE_SMS_SENT
    }
}
