using HospitalMgrSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : Controller
    {
        private readonly HospitalMgrSystem.Service.Default.IDefaultService _defaultService;

        public DefaultController(Service.Default.IDefaultService defaultService)
        {
            _defaultService = defaultService;
        }

        [HttpGet("ChannelingScanGetByID")]
        public ActionResult<Scan> ChannelingScanGetByID(int Id)
        {
            var newChanneling = _defaultService.GetScanChannelingFee(Id);
            if (newChanneling == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newChanneling);
            }
        }

        [HttpGet("GetDiscount")]
        public ActionResult<Scan> GetDiscount()
        {
            var discount = _defaultService.getDiscount();
            
            if (discount == null) return NoContent();
            
            return Ok(discount);
        }
    }
}