using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class ConsultantController : Controller
    {
        private IConfiguration _configuration;

        [BindProperty]
        public Consultant myConsultant { get; set; }

        [BindProperty]
        public string SearchValue { get; set; }

        [BindProperty]
        public ConsultantDto viewConsultant { get; set; }
        public ConsultantController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            ConsultantDto consultantDto = new ConsultantDto();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    ConsultantService consultantService = new ConsultantService();

                    consultantDto.listConsultants = consultantService.GetAllConsultantByStatus();

                }
                catch (Exception ex) { }
            }
            return View(consultantDto);

        }

        public IActionResult AddNewConsultant()
        {
            using var httpClient = new HttpClient();
            try
            {
                var APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");

                viewConsultant.Consultant.CreateDate = DateTime.Now;
                viewConsultant.Consultant.ModifiedDate = DateTime.Now;
                viewConsultant.Consultant.Status = 0;

                httpClient.BaseAddress = new Uri(APIUrl + "Consultant/");
                var postObj = httpClient.PostAsJsonAsync("CreateConsultant", viewConsultant.Consultant);
                postObj.Wait();

                var res = postObj.Result;
                var result = res.Content.ReadFromJsonAsync<User>().Result;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult CreateConsultant(int id)
        {
            ConsultantDto consultantDto = new ConsultantDto();
            consultantDto.Specialists = LoadSpecialistList();

            if (id > 0)
            {
                using (var httpClient = new HttpClient())
                {

                    try
                    {
                        string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                        //consultantDto.Consultant.Id = id;
                       // consultantDto.Consultant.CreateDate = DateTime.Now;
                        //consultantDto.Consultant.ModifiedDate = DateTime.Now;
                        httpClient.BaseAddress = new Uri(APIUrl + "Consultant/");
                        var postObj = httpClient.GetFromJsonAsync<Consultant>("GetAllConsultantByID?id=" + id);
                        postObj.Wait();
                        var res = postObj.Result;
                        consultantDto.Consultant = res;
                        return PartialView("_PartialConsultantAddEdit", consultantDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
                return PartialView("_PartialConsultantAddEdit", consultantDto);
        }

        private List<Specialist> LoadSpecialistList()
        {
            List<Specialist> Specialist = new List<Specialist>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Specialists/");
                    var postObj = httpClient.GetFromJsonAsync<List<Specialist>>("GetSpecialist");
                    postObj.Wait();
                    Specialist = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return Specialist;
        }


        public IActionResult DeleteConsultant()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    myConsultant.Status = 0;
                    httpClient.BaseAddress = new Uri(APIUrl + "Consultant/");
                    var postObj = httpClient.PostAsJsonAsync<Consultant>("DeleteConsultant?", myConsultant);
                    postObj.Wait();
                    var res = postObj.Result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public IActionResult Search()
        {
            ConsultantDto consultantDto = new ConsultantDto();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Consultant/");
                    var postObj = httpClient.GetFromJsonAsync<List<Consultant>>("SearchConsultant?text=" + SearchValue);
                    postObj.Wait();
                    var result = postObj.Result;
                    consultantDto.listConsultants = result;
                    //foreach (var item in result)
                    //{
                    //    patients.Add(new PatientDto()
                    //    {
                    //        Id = item.Id,
                    //        Address = item.Address,
                    //        FullName = item.FullName,
                    //        Age = item.Age,
                    //        TelephoneNumber = item.TelephoneNumber,
                    //        MobileNumber = item.MobileNumber,
                    //        NIC = item.NIC,
                    //        Sex = (HospitalMgrSystem.Model.Enums.SexStatus)item.Sex
                    //    });
                    //}
                }
                catch (Exception ex) { }
            }
            return View("Index", consultantDto);
        }

    }
}
