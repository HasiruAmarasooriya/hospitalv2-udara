using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using HospitalMgrSystem.Service.Stock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        [HttpGet("GetAllGRN")]
        public ActionResult<List<GRN>> GetAllGRN()
        {

            var StockService = new StockService();
            var newDrugsCategory = StockService.GetAllGRN();
            if (newDrugsCategory == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newDrugsCategory);
            }
        }
        [HttpGet("GetAllGRPV")]
        public ActionResult<List<GRPVDetailsDto>> GetAllGRPV()
        {

            var StockService = new StockService();
            var newDrugsCategory = StockService.GetAllGRPV();
            if (newDrugsCategory == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newDrugsCategory);
            }
        }
        [HttpGet("GetAllStore")]
        public ActionResult<List<Warehouse>> GetAllStore()
        {

            var StockService = new StockService();
            var newDrugsCategory = StockService.GetAllStore();
            if (newDrugsCategory == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newDrugsCategory);
            }
        }
        [HttpGet("GetBatchbyDrugID")]
       
        public ActionResult<List<GRPVDetailsDto>> GetBatchbyDrugID(int DrugID)
        {

            var StockService = new StockService();
            var newDrugsCategory = StockService.GetBatchbyDrugID(DrugID);
            if (newDrugsCategory == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newDrugsCategory);
            }
        }
    }
}
