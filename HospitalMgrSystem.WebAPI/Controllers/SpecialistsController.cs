using HospitalMgrSystem.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialistsController : Controller
    {
        private readonly HospitalMgrSystem.Service.Specialist.ISpecialistsService _specialistsService;

        public SpecialistsController(HospitalMgrSystem.Service.Specialist.ISpecialistsService specialistsService)
        {
            _specialistsService = specialistsService;
        }

        [HttpGet("GetSpecialist")]
        public ActionResult<Specialist> GetSpecialist()
        {
            var objRomms = _specialistsService.GetSpecialist();
            if (objRomms == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(objRomms);
            }
        }
    }
}
