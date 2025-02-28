using HospitalMgrSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestigationController : Controller
    {
        private readonly HospitalMgrSystem.Service.Investigation.IInvestigationService _investigationService;

        public InvestigationController(HospitalMgrSystem.Service.Investigation.IInvestigationService investigationService)
        {
            _investigationService = investigationService;
        }

        [HttpGet("GetAllInvestigationCategory")]
        public ActionResult<List<InvestigationCategory>> GetAllInvestigationCategory()
        {
            var newInvestigationCategory = _investigationService.GetAllInvestigationCategory();
            if (newInvestigationCategory == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newInvestigationCategory);
            }
        }

        [HttpGet("GetAllInvestigationSubCategoryByID")]
        public ActionResult<List<InvestigationSubCategory>> GetAllInvestigationSubCategoryByID(int CategoryID)
        {
            var newInvestigationSubCategory = _investigationService.GetAllInvestigationSubCategoryByID(CategoryID);
            if (newInvestigationSubCategory == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newInvestigationSubCategory);
            }
        }

        [HttpPost("CreateInvestigation")]
        public ActionResult<Investigation> CreateDrugs(Investigation investigation)
        {
            var newInvestigation = _investigationService.CreateInvestigation(investigation);
            if (newInvestigation == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newInvestigation);
            }
        }

        [HttpGet("GetAllInvestigation")]
        public ActionResult<List<Investigation>> GetAllDrug()
        {
            var newInvestigation = _investigationService.GetAllInvestigationByStatus();
            if (newInvestigation == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newInvestigation);
            }
        }

        [HttpGet("GetAllInvestigationByID")]
        public ActionResult<Investigation> GetAllInvestigationByID(int Id)
        {
            var newInvestigation = _investigationService.GetAllInvestigationByID(Id);
            if (newInvestigation == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newInvestigation);
            }
        }


        [HttpPost("DeleteInvestigation")]
        public ActionResult<Investigation> DeleteInvestigation(Investigation investigation)
        {
            var newInvestigation = _investigationService.DeleteInvestigation(investigation);
            if (newInvestigation == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newInvestigation);
            }
        }

        [HttpGet("SearchInvestigation")]
        public ActionResult<List<Investigation>> SearchInvestigation(string text)
        {
            var newInvestigation = _investigationService.SearchInvestigation(text);
            if (newInvestigation == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newInvestigation);
            }
        }
    }
}
