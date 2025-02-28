using HospitalMgrSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantController : Controller
    {
        private readonly HospitalMgrSystem.Service.Consultant.IConsultantService _consultantService;

        public ConsultantController(HospitalMgrSystem.Service.Consultant.IConsultantService consultantService)
        {
            _consultantService = consultantService;
        }


        [HttpPost("CreateConsultant")]
        public ActionResult<Patient> CreateConsultant(Consultant consultant)
        {
            var newConsultant = _consultantService.CreateConsultant(consultant);
            if (newConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newConsultant);
            }
        }

        [HttpGet("GetAllConsultant")]
        public ActionResult<List<Consultant>> GetAllConsultant()
        {
            var newConsultant = _consultantService.GetAllConsultantByStatus();
            if (newConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newConsultant);
            }
        }

        [HttpGet("ConsultantGetBySpecialistId")]
        public ActionResult<List<Consultant>> ConsultantGetBySpecialistId(int id)
        {
            var newConsultant = _consultantService.ConsultantGetBySpecialistId(id);
            if (newConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newConsultant);
            }
        }

        [HttpGet("GetAllConsultantByID")]
        public ActionResult<Consultant> GetAllPatientsByID(int Id)
        {
            var newConsultant = _consultantService.GetAllConsultantByID(Id);
            if (newConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newConsultant);
            }
        }


        [HttpPost("DeleteConsultant")]
        public ActionResult<Consultant> DeletePatient(Consultant consultant)
        {
            var newConsultant = _consultantService.DeleteConsultant(consultant);
            if (newConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newConsultant);
            }
        }

        [HttpGet("SearchConsultant")]
        public ActionResult<List<Consultant>> SearchConsultant(string text)
        {
            var newConsultant = _consultantService.SearchConsultant(text);
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
