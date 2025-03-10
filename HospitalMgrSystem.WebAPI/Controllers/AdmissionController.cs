using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.Admission;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmissionController : Controller
    {
        private readonly HospitalMgrSystem.Service.Admission.IAdmissionService _admissionService;

        public AdmissionController(HospitalMgrSystem.Service.Admission.IAdmissionService admissionService)
        {
            _admissionService = admissionService;
        }

        [HttpPost("CreateAdmission")]
        public ActionResult<Patient> CreateConsultant(Admission admission)
        {
            var newAdmission = _admissionService.CreateAdmission(admission);
            if (newAdmission == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmission);
            }
        }

        [HttpGet("GetAllAdmission")]
        public ActionResult<List<Admission>> GetAllAdmission()
        {
            var newAdmission = _admissionService.GetAllAdmission();
            if (newAdmission == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmission);
            }
        }

        [HttpGet("GetAdmissionByID")]
        public ActionResult<Admission> GetAdmissionByID(int id)
        {
            var newAdmission = _admissionService.GetAdmissionByID(id);
            if (newAdmission == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmission);
            }
        }

        [HttpPost("DeleteAdmission")]
        public ActionResult<Admission> DeleteAdmission(Admission admission)
        {
            var newAdmission = _admissionService.DeleteAdmission(admission);
            if (newAdmission == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmission);
            }
        }

        [HttpGet("SearchAdmission")]
        public ActionResult<List<Admission>> SearchAdmission(string text)
        {
            var newAdmission = _admissionService.SearchAdmission(text);
            if (newAdmission == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmission);
            }
        }

        [HttpPost("CreateAdmissionDrugus")]
        public ActionResult<AdmissionDrugus> CreateConsultant(AdmissionDrugus admissionDrugus)
        {
            var newAdmissionDrugus = _admissionService.CreateAdmissionDrugus(admissionDrugus);
            if (newAdmissionDrugus == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmissionDrugus);
            }
        }

        [HttpGet("GetAdmissionDrugus")]
        public ActionResult<List<AdmissionDrugus>> GetAdmissionDrugus(int AdmissionID)
        {
            AdmissionService admissionService = new AdmissionService();
            var newAdmissionDrugus = admissionService.GetAdmissionDrugusbyAdmissionID(AdmissionID);
            if (newAdmissionDrugus == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmissionDrugus);
            }
        }

        [HttpPost("DeleteAdmissionDrugus")]
        public ActionResult<Admission> DeleteAdmissionDrugus(AdmissionDrugus admissionDrugus)
        {
            var newAdmissionDrugus = _admissionService.DeleteAdmissionDrugus(admissionDrugus);
            if (newAdmissionDrugus == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmissionDrugus);
            }
        }

        [HttpPost("CreateAdmissionInvestigation")]
        public ActionResult<AdmissionDrugus> CreateAdmissionInvestigation(AdmissionInvestigation admissionInvestigation)
        {
            var newAdmissionInvestigation = _admissionService.CreateAdmissionInvestigation(admissionInvestigation);
            if (newAdmissionInvestigation == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmissionInvestigation);
            }
        }

        [HttpGet("GetAdmissionInvestigation")]
        public ActionResult<List<AdmissionInvestigation>> GetAdmissionInvestigation(int AdmissionId)
        {
            AdmissionService admissionService = new AdmissionService();
            var newAdmissionInvestigation = admissionService.GetAdmissionInvestigationbyAdmissionID(AdmissionId);
            if (newAdmissionInvestigation == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmissionInvestigation);
            }
        }


        [HttpPost("DeleteAdmissionInvestigation")]
        public ActionResult<Admission> DeleteAdmissionInvestigation(AdmissionInvestigation admissionInvestigation)
        {
            var newAdmissionInvestigation = _admissionService.DeleteAdmissionInvestigation(admissionInvestigation);
            if (newAdmissionInvestigation == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmissionInvestigation);
            }
        }

        [HttpPost("CreateAdmissionConsultant")]
        public ActionResult<AdmissionConsultant> CreateAdmissionConsultant(AdmissionConsultant admissionConsultant)
        {
            var newAdmissionConsultant = _admissionService.CreateAdmissionConsultant(admissionConsultant);
            if (newAdmissionConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmissionConsultant);
            }
        }

        [HttpGet("GetAdmissionConsultant")]
        public ActionResult<List<AdmissionConsultant>> GetAdmissionConsultant(int AdmissionId)
        {
            AdmissionService admissionService = new AdmissionService();
            var newAdmissionConsultant = admissionService.GetAdmissionConsultantbyAdmissionID(AdmissionId);
            if (newAdmissionConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmissionConsultant);
            }
        }

        [HttpPost("DeleteAdmissionConsultant")]
        public ActionResult<Admission> DeleteAdmissionConsultant(AdmissionConsultant admissionConsultant)
        {
            var newAdmissionConsultant = _admissionService.DeleteAdmissionConsultant(admissionConsultant);
            if (newAdmissionConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmissionConsultant);
            }
        }


        [HttpPost("CreateAdmissionItems")]
        public ActionResult<AdmissionItems> CreateAdmissionItems(AdmissionItems admissionItems)
        {
            var newadmissionItems = _admissionService.CreateAdmissionItems(admissionItems);
            if (newadmissionItems == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newadmissionItems);
            }
        }


        [HttpGet("GetAdmissionItems")]
        public ActionResult<List<AdmissionItems>> GetAdmissionItems(int AdmissionId, PaymentStatus PayStatus)
        {
            var newAdmissionItems = _admissionService.GetAdmissionItems(AdmissionId, PayStatus);
            if (newAdmissionItems == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmissionItems);
            }
        }

        [HttpPost("DeleteAdmissionItems")]
        public ActionResult<Admission> DeleteAdmissionItems(AdmissionItems admissionItems)
        {
            var newAdmissionItems = _admissionService.DeleteAdmissionItems(admissionItems);
            if (newAdmissionItems == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newAdmissionItems);
            }
        }

    }
}
