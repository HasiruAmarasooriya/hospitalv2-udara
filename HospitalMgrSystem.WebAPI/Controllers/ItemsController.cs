using HospitalMgrSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : Controller
    {
        private readonly HospitalMgrSystem.Service.Item.IItemService _itemService;

        public ItemsController(HospitalMgrSystem.Service.Item.IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("GetAllItemCategory")]
        public ActionResult<List<ItemCategory>> GetAllItemCategory()
        {
            var newItemCategory = _itemService.GetAllItemCategory();
            if (newItemCategory == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newItemCategory);
            }
        }

        [HttpGet("GetAllItemSubCategoryByID")]
        public ActionResult<List<ItemSubCategory>> GetAllItemSubCategoryByID(int CategoryID)
        {
            var newItemSubCategory = _itemService.GetAllItemSubCategoryByID(CategoryID);
            if (newItemSubCategory == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newItemSubCategory);
            }
        }

        [HttpPost("CreateItem")]
        public ActionResult<Item> CreateDrugs(Item item)
        {
            var newitem = _itemService.CreateItem(item);
            if (newitem == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newitem);
            }
        }


        [HttpGet("GetAllInvestigation")]
        public ActionResult<List<Item>> GetAllItem()
        {
            var newItem = _itemService.GetAllItemByStatus();
            if (newItem == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newItem);
            }
        }

        [HttpGet("GetItemByID")]
        public ActionResult<Item> GetAllItemByID(int Id)
        {
            var newItem = _itemService.GetAllItemByID(Id);
            if (newItem == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newItem);
            }
        }
        [HttpPost("DeleteItem")]
        public ActionResult<Item> DeleteItem(Item item)
        {
            var newItem = _itemService.DeleteItem(item);
            if (newItem == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newItem);
            }
        }

        [HttpGet("SearchItem")]
        public ActionResult<List<Item>> SearchItem(string text)
        {
            var newItem = _itemService.SearchItem(text);
            if (newItem == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newItem);
            }
        }

    }
}
