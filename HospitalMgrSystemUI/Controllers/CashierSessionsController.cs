using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class CashierSessionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public ActionResult CreateCashierSessionse(int id)
        {
            if (id > 0)
            {
                return PartialView("_PartialAddCashierSession");
            }
            else
                return PartialView("_PartialAddCashierSession");



        }
    }
}
