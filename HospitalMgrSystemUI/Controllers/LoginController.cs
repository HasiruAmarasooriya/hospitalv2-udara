using HospitalMgrSystem.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HospitalMgrSystemUI.Controllers
{
    public class LoginController : Controller
    {
        [BindProperty]
        public User myUser { get; set; }
        private IConfiguration _configuration;

        public LoginController(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult LoginUser(string eete) 
        //{
        //    using (var httpClient = new HttpClient())
        //    {

        //        try
        //        {
        //            string APIUrl =  _configuration.GetValue<string>("MainAPI:APIURL");
        //            myUser.CreateDate = DateTime.Now;
        //            myUser.ModifiedDate = DateTime.Now;
        //            httpClient.BaseAddress = new Uri( APIUrl + "User/");
        //            var postObj = httpClient.PostAsJsonAsync<User>("UserLoginDetails", myUser);
        //            postObj.Wait();
        //            var res = postObj.Result;
        //            var result = res.Content.ReadFromJsonAsync<User>().Result;
        //            if (result != null && result.Id != 0)
        //            {
        //                string redirectUrl = string.Format("/Home/Index");
        //                return Redirect(redirectUrl);
        //            }
        //            else {
        //                return RedirectToAction("Index");
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            return RedirectToAction("Index");
        //        }

        //    }
        //}

        public IActionResult LoginUser(string eete)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    myUser.CreateDate = DateTime.Now;
                    myUser.ModifiedDate = DateTime.Now;
                    httpClient.BaseAddress = new Uri(APIUrl + "User/");

                    // Assuming myUser.UserName is the property that holds the username
                    var postObj = httpClient.PostAsJsonAsync<User>("UserLoginDetails", myUser);
                    postObj.Wait();
                    var res = postObj.Result;
                    var result = res.Content.ReadFromJsonAsync<User>().Result;

                    if (result != null && result.Id != 0)
                    {
                        // Save username as a cookie
                        Response.Cookies.Append("UserNameCookie", result.UserName);
                        Response.Cookies.Append("UserIdCookie", result.Id.ToString());
                        Response.Cookies.Append("UserRoleCookie", result.userRole.ToString());
                        string redirectUrl = "/Home/Index";
                        return Redirect(redirectUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception
                    return RedirectToAction("Index");
                }
            }
        }

    }
}
