using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class SMSTokenRequest
    {
        public List<MobileNumber> msisdn { get; set; }
        public string sourceAddress { get; set; }
        public string message { get; set; }
        public int transaction_id { get; set; }
        public int payment_method { get; set; }
        public string push_notification_url { get; set; }
    }
}
