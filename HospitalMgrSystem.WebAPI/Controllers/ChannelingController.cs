using HospitalMgrSystem.Model;
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