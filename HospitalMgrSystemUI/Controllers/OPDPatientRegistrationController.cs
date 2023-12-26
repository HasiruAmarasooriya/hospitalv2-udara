using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class OPDPatientRegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult CreateOPDPatientRegistration()
        {
            return PartialView("_PartialAddOPDPatientRegistration");
        }

    }
}
