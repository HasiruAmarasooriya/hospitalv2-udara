using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelingSheduleController : Controller
    {
        private readonly Service.ChannelingSchedule.IChannelingSchedule _channelingService;

        public ChannelingSheduleController(HospitalMgrSystem.Service.ChannelingSchedule.IChannelingSchedule channelingService)
        {
            _channelingService = channelingService;
        }

        [HttpPost("CreateChannelingShedule")]
        public ActionResult<ChannelingSchedule> CreateChannelingShedule(ChannelingSchedule channeling)
        {
            var newChanneling = _channelingService.CreateChannelingSchedule(channeling);
            if (newChanneling == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newChanneling);
            }
        }

        [HttpGet("GetAllChannelingScheduleByStatus")]
        public ActionResult<ChannelingSchedule> GetAllChannelingScheduleByStatus()
        {
            var newChanneling = _channelingService.SheduleGetByStatus();
            if (newChanneling == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newChanneling);
            }
        }

        [HttpGet("SheduleGetByConsultantIdandDate")]
        public ActionResult<ChannelingSchedule> SheduleGetByConsultantIdandDate(int ConsaltantId, string date)
        {
            var newChanneling = _channelingService.SheduleGetByConsultantIdandDate(ConsaltantId, date);
            if (newChanneling == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newChanneling);
            }
        }

        [HttpGet("GetAllSheduleGetByConsultantId")]
        public ActionResult<ChannelingSchedule> GetAllSheduleGetByConsultantId(int Id)
        {
            var newChanneling = _channelingService.SheduleGetByConsultantId(Id);
            if (newChanneling == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newChanneling);
            }
        }

        [HttpGet("GetAllSheduleGetByConsultantIdAndSessionStatus")]
        public ActionResult<ChannelingSchedule> GetAllSheduleGetByConsultantIdAndSessionStatus(int Id, ChannellingScheduleStatus channellingScheduleStatus)
        {
            var newChanneling = _channelingService.GetAllSheduleGetByConsultantIdAndSessionStatus(Id, channellingScheduleStatus);
            if (newChanneling == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newChanneling);
            }
        }

        [HttpGet("GetSheduleGetById")]
        public ActionResult<ChannelingSchedule> GetSheduleGetById(int Id)
        {
            var newChanneling = _channelingService.SheduleGetById(Id);
            if (newChanneling == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newChanneling);
            }
        }

        [HttpGet("GetChannelingItemById")]
        public ActionResult<Scan> GetChannelingItemById(int Id)
        {
            Scan chanelingItem = _channelingService.GetChannelingItemById(Id);
            if (chanelingItem == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(chanelingItem);
            }
        }

        [HttpPost("DeleteChannelingShedule")]
        public ActionResult<ChannelingSchedule> DeleteChannelingShedule(ChannelingSchedule channelingSchedule)
        {
            var newChannelingShedule = _channelingService.DeleteChannelingShedule(channelingSchedule.Id);
            if (newChannelingShedule == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newChannelingShedule);
            }
        }


    }
}