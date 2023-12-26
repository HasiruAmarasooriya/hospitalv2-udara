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

namespace HospitalMgrSystemUI.Controllers
{
    public class ItemsController : Controller
    {
        private IConfiguration _configuration;

        [BindProperty]
        public ItemDto viewItem { get; set; }

        [BindProperty]
        public Item myItem { get; set; }

        [BindProperty]
        public string SearchValue { get; set; }


        public ItemsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ItemDto itemDto = new ItemDto();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Items/");
                    var postObj = httpClient.GetFromJsonAsync<List<Item>>("GetAllInvestigation");
                    postObj.Wait();
                    var result = postObj.Result;

                    itemDto.ItemList = result;

                }
                catch (Exception ex) { }
            }
            return View(itemDto);
        }

        private List<ItemCategory> GetAllItemCategory()
        {
            List<ItemCategory> ItemCategoryList = new List<ItemCategory>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Items/");
                    var postObj = httpClient.GetFromJsonAsync<List<ItemCategory>>("GetAllItemCategory");
                    postObj.Wait();
                    ItemCategoryList = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return ItemCategoryList;
        }
        public ActionResult CreateItem(int Id)
        {
            ItemDto itemDto = new ItemDto();
            itemDto.ItemCategory = GetAllItemCategory();

            if (Id > 0)
            {
                using (var httpClient = new HttpClient())
                {

                    try
                    {
                        string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                        httpClient.BaseAddress = new Uri(APIUrl + "Items/");
                        var postObj = httpClient.GetFromJsonAsync<Item>("GetItemByID?id=" + Id);
                        postObj.Wait();
                        var res = postObj.Result;
                        itemDto.Item = res;
                        itemDto.ItemSubCategory = GetAllItemSubCategory(itemDto.Item.ItemCategoryId);
                        return PartialView("_PartialIAddtems", itemDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
                return PartialView("_PartialIAddtems", itemDto);
        }

        private List<ItemSubCategory> GetAllItemSubCategory(int Id)
        {
            List<ItemSubCategory> ItemSubCategoryList = new List<ItemSubCategory>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Items/");
                    var postObj = httpClient.GetFromJsonAsync<List<ItemSubCategory>>("GetAllItemSubCategoryByID?CategoryID=" + Id);
                    postObj.Wait();
                    ItemSubCategoryList = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return ItemSubCategoryList;
        }

        public IActionResult AddNewIem()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    viewItem.Item.CreateDate = DateTime.Now;
                    viewItem.Item.ModifiedDate = DateTime.Now;

                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Items/");
                    var postObj = httpClient.PostAsJsonAsync<Item>("CreateItem", viewItem.Item);
                    postObj.Wait();
                    var res = postObj.Result;
                    var result = res.Content.ReadFromJsonAsync<Item>().Result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public IActionResult DeleteItem()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    myItem.ModifiedDate = DateTime.Now;
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Items/");
                    var postObj = httpClient.PostAsJsonAsync<Item>("DeleteItem?", myItem);
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
            ItemDto itemDto = new ItemDto();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Items/");
                    var postObj = httpClient.GetFromJsonAsync<List<Item>>("SearchItem?text=" + SearchValue);
                    postObj.Wait();
                    var result = postObj.Result;
                    itemDto.ItemList = result;
                }
                catch (Exception ex) { }
            }
            return View("Index", itemDto);
        }

    }
}
