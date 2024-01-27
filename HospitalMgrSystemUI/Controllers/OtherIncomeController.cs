using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class OtherIncomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public ActionResult CreateOtherIncome(int id)
        {
            if (id > 0)
            {
                return PartialView("_PartialAddOtherIncome");
            }
            else
                return PartialView("_PartialAddOtherIncome");



        }
    }
}
