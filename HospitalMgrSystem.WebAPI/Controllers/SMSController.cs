using HospitalMgrSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SMSController : Controller
    {
        private readonly HospitalMgrSystem.Service.SMS.ISMSService _SMSService;

        public SMSController(HospitalMgrSystem.Service.SMS.ISMSService sMSService)
        {
            _SMSService = sMSService;
        }

        [HttpPost("GetAccessToken")]
        public async Task<IActionResult> GetAccessToken()
        {
            var newConsultant = await _SMSService.GetAccessToken();
            if (newConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newConsultant);
            }
        }

        [HttpPost("SendSMS")]
        public ActionResult<string> SendSMS(ChannelingSMS channelingSMS)
        {
            var newConsultant = _SMSService.SendSMSToken(channelingSMS);
            if (newConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newConsultant);
            }
        }

        [HttpPost("SendSMSV2")]
        public ActionResult<string> SendSMSV2()
        {
            var newConsultant = _SMSService.SendSMSToken();
            if (newConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newConsultant);
            }
        }

    }
}
