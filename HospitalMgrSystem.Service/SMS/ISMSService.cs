using HospitalMgrSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.SMS
{
    public interface ISMSService
    {
        public  Task<SMSAPILogin> GetAccessToken();
        public Task<string> SendSMSToken(ChannelingSMS channelingSMS);

        public Task<string> SendSMSToken();
    }
}
