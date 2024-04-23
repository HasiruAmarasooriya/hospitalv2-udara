using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelingController : Controller
    {
        private readonly HospitalMgrSystem.Service.Channeling.IChannelingService _channelingService;

        public ChannelingController(HospitalMgrSystem.Service.Channeling.IChannelingService ChannelingService)
        {
            _channelingService = ChannelingService;
        }

        [HttpPost("CreateChanneling")]
        public ActionResult<Channeling> CreateChannelingShedule(Channeling channeling)
        {
            var newChanneling = _channelingService.CreateChanneling(channeling);
            if (newChanneling == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newChanneling);
            }
        }

        [HttpGet("GetAllConsultantByScheduleDate")]
        public ActionResult<Channeling> GetAllConsultantByScheduleDate(string StartFrom)
        {
            // Parse the string date to a DateTime object
            if (DateTime.TryParseExact(StartFrom, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
            {
                var newChanneling = _channelingService.GetAllConsultantThatHaveSchedulingsByDate(startDate);
                if (newChanneling == null)
                {
                    return NoContent();
                }
                else
                {
                    return Ok(newChanneling);
                }
            }
            else
            {
                // Handle invalid date format
                return BadRequest("Invalid date format. Please provide the date in yyyy-MM-dd format.");
            }
        }


        [HttpGet("ChannelingGetBySheduleId")]
        public ActionResult<Channeling> ChannelingGetBySheduleId(int Id)
        {
            var newChanneling = _channelingService.ChannelingGetBySheduleId(Id);
            if (newChanneling == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newChanneling);
            }
        }

        [HttpPost("DeleteChanneling")]
        public ActionResult<Channeling> DeleteChanneling(Channeling channeling)
        {
            var newChanneling = _channelingService.DeleteChanneling(channeling.Id);
            if (newChanneling == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newChanneling);
            }
        }

    }
}