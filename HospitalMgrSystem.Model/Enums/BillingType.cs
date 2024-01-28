using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model.Enums
{
    public enum BillingType
    {
        CASHIER, // all services       
        BALENCE, // all  services Balence
        REFUND, // all services refund
        OTHER_IN,// All other in
        OTHER_OUT// All other out
    }
}
