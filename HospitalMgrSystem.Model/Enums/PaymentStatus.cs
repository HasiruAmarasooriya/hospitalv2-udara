using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model.Enums
{
    public enum PaymentStatus
    {
        PAID,
        PARTIAL_PAID,
        NEED_TO_PAY,
        OPD,
        NOT_PAID
    }
}
