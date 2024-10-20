using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HospitalMgrSystemUI.Controllers
{
    public class LoginController : Controller
    {
        [BindProperty]
        public User myUser { get; set; }
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser()
        {
            var user = _userService.GetUserLogin(myUser);

            if (user != null && user.Id != 0)
            {

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.userRole.ToString()),
            new Claim("UserId", user.Id.ToString())
        };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                HttpContext.Response.Cookies.Append("UserNameCookie", user.UserName);
                HttpContext.Response.Cookies.Append("UserIdCookie", user.Id.ToString());
                HttpContext.Response.Cookies.Append("UserRoleCookie", user.userRole.ToString());

                string redirectUrl = "/Home/Index";
                return Redirect(redirectUrl);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Response.Cookies.Delete("UserNameCookie");
            HttpContext.Response.Cookies.Delete("UserIdCookie");
            HttpContext.Response.Cookies.Delete("UserRoleCookie");

            if (HttpContext.Response.Headers.ContainsKey("Cache-Control"))
            {
                HttpContext.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            }
            else
            {
                HttpContext.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate");
            }

            if (HttpContext.Response.Headers.ContainsKey("Pragma"))
            {
                HttpContext.Response.Headers["Pragma"] = "no-cache";
            }
            else
            {
                HttpContext.Response.Headers.Add("Pragma", "no-cache");
            }

            return RedirectToAction("Index", "Login");
        }


    }
}
