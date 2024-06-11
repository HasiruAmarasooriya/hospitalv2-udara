using HospitalMgrSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugsController : Controller
    {
        private readonly HospitalMgrSystem.Service.Drugs.IDrugsService _drugsService;

        public DrugsController(HospitalMgrSystem.Service.Drugs.IDrugsService drugsService)
        {
            _drugsService = drugsService;
        }

        [HttpGet("GetAllDrugsCategory")]
        public ActionResult<List<DrugsCategory>> GetAllDrugsCategory()
        {
            var newDrugsCategory = _drugsService.GetAllDrugsCategory();
            if (newDrugsCategory == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newDrugsCategory);
            }
        }

        [HttpGet("GetAllDrugsSubCategoryById")]
        public ActionResult<List<DrugsSubCategory>> GetAllDrugsSubCategoryById(int CategoryID)
        {
            var newDrugsCategory = _drugsService.DrugsSubCategoryByID(CategoryID);
            if (newDrugsCategory == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newDrugsCategory);
            }
        }

        [HttpPost("CreateDrugs")]
        public ActionResult<Patient> CreateDrugs(Drug drug)
        {
            var newConsultant = _drugsService.CreateDrugs(drug);
            if (newConsultant == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newConsultant);  
            }
        }

        [HttpGet("GetAllDrugs")]
        public ActionResult<List<Drug>> GetAllDrug()
        {
            var newDrug = _drugsService.GetAllDrugsByStatus();
            if (newDrug == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newDrug);
            }
        }


        [HttpGet("GetAllDrugByID")]
        public ActionResult<Drug> GetAllDrugByID(int Id)
        {
            var newDrug = _drugsService.GetAllDrugByID(Id);
            if (newDrug == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newDrug);
            }
        }

        [HttpPost("DeleteDrug")]
        public ActionResult<Drug> DeletePatient(Drug drug)
        {
            var newDrug = _drugsService.DeleteDrug(drug);
            if (newDrug == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newDrug);
            }
        }


        [HttpGet("SearchDrugs")]
        public ActionResult<List<Drug>> SearchDrugs(string text)
        {
            var newConsultant = _drugsService.SearchDrug(text);
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
