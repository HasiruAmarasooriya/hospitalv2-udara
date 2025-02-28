using HospitalMgrSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : Controller
    {

        private readonly HospitalMgrSystem.Service.Patients.IPatientService _patientService;

        public PatientController(HospitalMgrSystem.Service.Patients.IPatientService patientService)
        {
            _patientService = patientService;
        }


        [HttpPost("CreatePatient")]
        public ActionResult<Patient> UserLoginDetails(Patient patient)
        {
            var newPayment = _patientService.CreatePatient(patient);
            if (newPayment == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newPayment);
            }
        }

        [HttpGet("GetAllPatients")]
        public ActionResult<List<Patient>> GetAllPatients()
        {
            var newPayment = _patientService.GetAllPatientByStatus();
            if (newPayment == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newPayment);
            }
        }

        [HttpGet("SearchPatient")]
        public ActionResult<List<Patient>> SearchPatient(string text)
        {
            var newPayment = _patientService.SearchPatient(text);
            if (newPayment == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newPayment);
            }
        }

        [HttpGet("GetAllPatientsByID")]
        public ActionResult<Patient> GetAllPatientsByID(int Id)
        {
            var newPayment = _patientService.GetAllPatientByID(Id);
            if (newPayment == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newPayment);
            }
        }

        [HttpPost("DeletePatient")]
        public ActionResult<Patient> DeletePatient(Patient patient)
        {
            var newPayment = _patientService.DeletePatient(patient);
            if (newPayment == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newPayment);
            }
        }
    }
}
