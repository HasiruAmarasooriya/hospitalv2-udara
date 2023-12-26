using HospitalMgrSystem.Model;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HospitalMgrSystemUI.Controllers
{
    public class DrugsController : Controller
    {
        private IConfiguration _configuration;

        [BindProperty]
        public Drug myDrug { get; set; }

        [BindProperty]
        public DrugsDto viewDrugs { get; set; }

        [BindProperty]
        public int CategoryID { get; set; }

        [BindProperty]
        public string SearchValue { get; set; }

        public DrugsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            DrugsDto drugsDto = new DrugsDto();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Drugs/");
                    var postObj = httpClient.GetFromJsonAsync<List<Drug>>("GetAllDrugs");
                    postObj.Wait();
                    var result = postObj.Result;

                    drugsDto.ListDrogs = result;

                }
                catch (Exception ex) { }
            }
            return View(drugsDto);
        }

        public ActionResult CreateDrugs()
        {
            DrugsDto drugsDto = new DrugsDto();
            drugsDto.DrugsCategory = GetAllDrugsCategory();
           // drugsDto.DrugsSubCategory = GetAllDrugsSubCategory(CategoryID);

            return PartialView("__PartialAddDrugs", drugsDto);
        }

        private List<DrugsCategory> GetAllDrugsCategory()
        {
            List<DrugsCategory> DrugsCategoryList = new List<DrugsCategory>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Drugs/");
                    var postObj = httpClient.GetFromJsonAsync<List<DrugsCategory>>("GetAllDrugsCategory");
                    postObj.Wait();
                    DrugsCategoryList = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return DrugsCategoryList;
        }

        private List<DrugsSubCategory> GetAllDrugsSubCategory(int Id)
        {
            List<DrugsSubCategory> DrugsSubCategoryList = new List<DrugsSubCategory>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Drugs/");
                    var postObj = httpClient.GetFromJsonAsync<List<DrugsSubCategory>>("GetAllDrugsSubCategoryById?CategoryID=" + Id);
                    postObj.Wait();
                    DrugsSubCategoryList = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return DrugsSubCategoryList;
        }

        public IActionResult AddNewDrugs()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    viewDrugs.Drug.CreateDate = DateTime.Now;
                    viewDrugs.Drug.ModifiedDate = DateTime.Now;

                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Drugs/");
                    var postObj = httpClient.PostAsJsonAsync<Drug>("CreateDrugs", viewDrugs.Drug);
                    postObj.Wait();
                    var res = postObj.Result;
                    var result = res.Content.ReadFromJsonAsync<Drug>().Result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult CreateDrug(int id)
        {
            DrugsDto drugsDto = new DrugsDto();
            drugsDto.DrugsCategory = GetAllDrugsCategory();
           

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
                        httpClient.BaseAddress = new Uri(APIUrl + "Drugs/");
                        var postObj = httpClient.GetFromJsonAsync<Drug>("GetAllDrugByID?id=" + id);
                        postObj.Wait();
                        var res = postObj.Result;
                        drugsDto.Drug = res;
                        drugsDto.DrugsSubCategory = GetAllDrugsSubCategory(drugsDto.Drug.DrugsCategoryId);
                        return PartialView("__PartialAddDrugs", drugsDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
                return PartialView("__PartialAddDrugs", drugsDto);
        }

        public IActionResult DeleteDrug()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    myDrug.ModifiedDate = DateTime.Now;
                    httpClient.BaseAddress = new Uri(APIUrl + "Drugs/");
                    var postObj = httpClient.PostAsJsonAsync<Drug>("DeleteDrug?", myDrug);
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
            DrugsDto drugsDto = new DrugsDto();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Drugs/");
                    var postObj = httpClient.GetFromJsonAsync<List<Drug>>("SearchDrugs?text=" + SearchValue);
                    postObj.Wait();
                    var result = postObj.Result;
                    drugsDto.ListDrogs = result;
                }
                catch (Exception ex) { }
            }
            return View("Index", drugsDto);
        }

    }
}
