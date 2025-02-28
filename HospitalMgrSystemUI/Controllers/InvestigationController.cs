using HospitalMgrSystem.Model;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class InvestigationController : Controller
    {
        private IConfiguration _configuration;


        [BindProperty]
        public Investigation myInvestigation { get; set; }

        [BindProperty]
        public InvestigationDto viewInvestigation { get; set; }

        [BindProperty]
        public string SearchValue { get; set; }

        public IActionResult Index()
        {
            InvestigationDto investigationDto = new InvestigationDto();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Investigation/");
                    var postObj = httpClient.GetFromJsonAsync<List<Investigation>>("GetAllInvestigation");
                    postObj.Wait();
                    var result = postObj.Result;

                    investigationDto.InvestigationList = result;

                }
                catch (Exception ex) { }
            }
            return View(investigationDto);
        }

        public InvestigationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private List<InvestigationCategory> GetAllInvestigationCategory()
        {
            List<InvestigationCategory> InvestigationCategoryList = new List<InvestigationCategory>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Investigation/");
                    var postObj = httpClient.GetFromJsonAsync<List<InvestigationCategory>>("GetAllInvestigationCategory");
                    postObj.Wait();
                    InvestigationCategoryList = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return InvestigationCategoryList;
        }

        public ActionResult CreateInvestigation(int Id)
        {
            InvestigationDto investigationDto = new InvestigationDto();
            investigationDto.InvestigationCategory = GetAllInvestigationCategory();

            if (Id > 0)
            {
                using (var httpClient = new HttpClient())
                {

                    try
                    {
                        string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                        //consultantDto.Consultant.Id = id;
                        // consultantDto.Consultant.CreateDate = DateTime.Now;
                        //consultantDto.Consultant.ModifiedDate = DateTime.Now;
                        httpClient.BaseAddress = new Uri(APIUrl + "Investigation/");
                        var postObj = httpClient.GetFromJsonAsync<Investigation>("GetAllInvestigationByID?id=" + Id);
                        postObj.Wait();
                        var res = postObj.Result;
                        investigationDto.Investigation = res;
                        investigationDto.InvestigationSubCategory = GetAllInvestigationSubCategory(investigationDto.Investigation.InvestigationCategoryId);
                        return PartialView("_PartialAddInvestigation", investigationDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
                return PartialView("_PartialAddInvestigation", investigationDto);

        }

        public IActionResult AddNewInvestigation()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    viewInvestigation.Investigation.CreateDate = DateTime.Now;
                    viewInvestigation.Investigation.ModifiedDate = DateTime.Now;

                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Investigation/");
                    var postObj = httpClient.PostAsJsonAsync<Investigation>("CreateInvestigation", viewInvestigation.Investigation);
                    postObj.Wait();
                    var res = postObj.Result;
                    var result = res.Content.ReadFromJsonAsync<Investigation>().Result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        private List<InvestigationSubCategory> GetAllInvestigationSubCategory(int Id)
        {
            List<InvestigationSubCategory> InvestigationSubCategoryList = new List<InvestigationSubCategory>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Investigation/");
                    var postObj = httpClient.GetFromJsonAsync<List<InvestigationSubCategory>>("GetAllInvestigationSubCategoryByID?CategoryID=" + Id);
                    postObj.Wait();
                    InvestigationSubCategoryList = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return InvestigationSubCategoryList;
        }

        public IActionResult DeleteInvestigation()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Investigation/");
                    var postObj = httpClient.PostAsJsonAsync<Investigation>("DeleteInvestigation?", myInvestigation);
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
            InvestigationDto investigationDto = new InvestigationDto();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Investigation/");
                    var postObj = httpClient.GetFromJsonAsync<List<Investigation>>("SearchInvestigation?text=" + SearchValue);
                    postObj.Wait();
                    var result = postObj.Result;
                    investigationDto.InvestigationList = result;
                }
                catch (Exception ex) { }
            }
            return View("Index", investigationDto);
        }


    }
}
