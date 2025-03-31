using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.User;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HospitalMgrSystemUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            
            var userNameCookie = HttpContext.Request.Cookies["UserNameCookie"];
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            int userID = Convert.ToInt32(userIdCookie);
            if (!string.IsNullOrEmpty(userNameCookie))
            {

                User user = GetUserById(userID);
                return View();
            }
            else
            {

                return Redirect("/Login");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private User GetUserById(int id)
        {
            User user = new User();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    user = new UserService().GetUserByID(id);

                }
                catch (Exception ex) { }
            }
            return user;
        }
		[HttpGet]
		public JsonResult GetEvents()
		{
			var events = new List<object>
	{
		new { title = "Doctor Appointment", start = DateTime.Now.ToString("yyyy-MM-dd") },
		new { title = "Patient Surgery", start = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd") },
		new { title = "Medicine Delivery", start = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd") }
	};

			return Json(events);
		}


	}
}
