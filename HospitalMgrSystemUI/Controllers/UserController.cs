using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.User;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class UserController : Controller
    {

        private IConfiguration _configuration;

        [BindProperty]
        public User myUser { get; set; }

        [BindProperty]
        public UserDto viewUser { get; set; }



        [BindProperty]
        public string SearchValue { get; set; }

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    UserDto userDto = new UserDto();
                    userDto.userList = new UserService().GetAllUsers();
                    //employeeDto.employees = new EmployeeService().GetAllEmployeeByStatus();
                    return View(userDto);
                }

            }
            catch (Exception ex)
            {

                return View();
            }

        }

        public IActionResult CreateNewUser()
        {
            var userIdCookie = Convert.ToInt32(HttpContext.Request.Cookies["UserIdCookie"]);

            using (var httpClient = new HttpClient())
            {
                // Validate if the logged user is an Admin
                if (UserService.ValidateUserAdmin(userIdCookie))
                {
                    try
                    {
                        User user = new UserService().CreateUser(viewUser.user);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");

            }
        }

        public IActionResult ChangeUserPassword()
        {
            var userIdCookie = Convert.ToInt32(HttpContext.Request.Cookies["UserIdCookie"]);
            using (var httpClient = new HttpClient())
            {
                if (UserService.ValidateUserAdmin(userIdCookie))
                {
                    try
                    {
                        User user = new UserService().ChangeUserPw(viewUser.user);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");
            }
        }
        public ActionResult CreateUser(int id)
        {
            UserDto userDto = new UserDto();

            if (id > 0)
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        userDto.user = new UserService().GetUserByID(id);
                        return PartialView("_PartialAddUser", userDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                return PartialView("_PartialAddUser", userDto);
            }
        }


        public ActionResult changePwUser(int id)
        {
            UserDto userDto = new UserDto();

            if (id > 0)
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        userDto.user = new UserService().GetUserByID(id);
                        userDto.conformPassowrd = userDto.user.Password;
                        return PartialView("_PartialChangeUserPassword", userDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                return PartialView("_PartialAddUser", userDto);
            }
        }

        public IActionResult DeleteUser()
        {
            UserDto userDto = new UserDto();
            var userIdCookie = Convert.ToInt32(HttpContext.Request.Cookies["UserIdCookie"]);

            using (var httpClient = new HttpClient())
            {
                if (UserService.ValidateUserAdmin(userIdCookie))
                {
                    try
                    {
                        string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                        myUser.ModifiedDate = DateTime.Now;
                        userDto.user = new UserService().DeleteUser(myUser);
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");
            }
        }
    }
}
