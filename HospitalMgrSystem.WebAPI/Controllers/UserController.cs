using Microsoft.AspNetCore.Mvc;
using HospitalMgrSystem.Model;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly HospitalMgrSystem.Service.User.IUserService _userService;

        public UserController(HospitalMgrSystem.Service.User.IUserService userService) 
        {
            _userService = userService;
        }

        [HttpPost("UserLoginDetails")]
        public ActionResult<User> UserLoginDetails(User user)
        {
            var newPayment = _userService.GetUserLogin(user);
            if (newPayment == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(newPayment);
            }
        }
    }
}
