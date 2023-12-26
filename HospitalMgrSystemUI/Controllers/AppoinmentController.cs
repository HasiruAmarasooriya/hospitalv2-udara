using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class AppoinmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult CreateAppoinment()
        {
            return PartialView("_PartialAddAppoinment");
        }

    }
}
