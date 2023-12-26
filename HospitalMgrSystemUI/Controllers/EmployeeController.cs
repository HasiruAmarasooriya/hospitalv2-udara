using HospitalMgrSystem.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;
using HospitalMgrSystemUI.Models;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Employee;
using HospitalMgrSystem.Service.OPD;

namespace HospitalMgrSystemUI.Controllers
{
    public class EmployeeController : Controller
    {
  

        private IConfiguration _configuration;

        [BindProperty]
        public EmployeeDetail myEmployee { get; set; }

        [BindProperty]
        public EmployeeDto viewEmployee { get; set; }



        [BindProperty]
        public string SearchValue { get; set; }

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    EmployeeDto employeeDto = new EmployeeDto();
                    employeeDto.employees = new EmployeeService().GetAllEmployeeByStatus();
                    return View(employeeDto);
                }

            }
            catch (Exception ex)
            {

                return View();
            }
           
        }




        public IActionResult CreateNewEmployee()
        {
            EmployeeDetail employee = new EmployeeDetail();
            using (var httpClient = new HttpClient())
            {

                try
                {
                    employee = new EmployeeService().CreateEmployeen(viewEmployee.employee);
                }
                catch (Exception ex)
                {

                }
                return RedirectToAction("Index");
                
            }
        }

        public ActionResult CreateEmployee(int id)
        {
            EmployeeDto employeeDto = new EmployeeDto();

            if (id > 0)
            {
                using (var httpClient = new HttpClient())
                {

                    try
                    {

                        employeeDto.employee = new EmployeeService().GetAEmployeenByID(id);
                        return PartialView("_PartialAddEmployee", employeeDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
                return PartialView("_PartialAddEmployee", employeeDto);
        }

        public IActionResult DeleteEmployee()
        {
            EmployeeDto employeeDto = new EmployeeDto();
            using (var httpClient = new HttpClient())
            {

                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    myEmployee.ModifiedDate = DateTime.Now;
                    employeeDto.employee = new EmployeeService().DeleteEmployee(myEmployee);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }




    }
}
