using HospitalMgrSystem.Model;

namespace HospitalMgrSystem.Service.SMS
{
    public interface ISMSService
    {
        public  Task<SMSAPILogin> GetAccessToken();
        public Task<string> SendSMSToken(ChannelingSMS channelingSMS);

        public Task<string> SendSMSToken();
        public string generateMessageBodyForChannelingSchedule(ChannelingSMS channelingSMS);
    }
}
