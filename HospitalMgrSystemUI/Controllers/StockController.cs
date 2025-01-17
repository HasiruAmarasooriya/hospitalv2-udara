using System;
using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.Cashier;
using HospitalMgrSystem.Service.ChannelingSchedule;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Drugs;
using HospitalMgrSystem.Service.OPD;
using HospitalMgrSystem.Service.Report;
using HospitalMgrSystem.Service.Stock;
using HospitalMgrSystemUI.Models;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace HospitalMgrSystemUI.Controllers
{
    public class StockController : Controller
    {
        private IConfiguration _configuration;
        [BindProperty] public WarehouseDto warehouseDto { get; set; }
        public IActionResult Index()
        {
            if (warehouseDto == null)
            {
                warehouseDto = new WarehouseDto();
            }
           
            var stockService = new StockService();
            warehouseDto.supplierList = stockService.GetAllGRN();
            //warehouseDto.grpvList = GetAllGRPV();
            warehouseDto.DrugsCategory = GetAllDrugsCategory();
            var requestDetails = stockService.GetAllStockRequestDetails();
            warehouseDto.RqeuestList = requestDetails
            .GroupBy(rd => rd.RequestID)
             .Select(group => new RequestDetailsDto
            {
             RequestID = group.Key,
             CreatedBy = group.First().CreatedBy,
             CreateDate = group.First().CreateDate,
             Items = group.Select(item => new RequestItemDetailsDto
             {
                 RequestID = item.StockRequestItemID,
                 DrugName = item.DrugName,
                 Quantity = item.Quantity
             }).ToList()
         })
         .ToList();
            var (mainStoresList, opdStoresList, addminStoresList) = GetOPDDrugDetails();
            warehouseDto.MainStore = mainStoresList;
            warehouseDto.OPDStore = opdStoresList;
            warehouseDto.AddmisionStore = addminStoresList;
            var (oPDtran, admissionTan) = DrugDetailsbydate();
            warehouseDto.OPDTrans = oPDtran;
            warehouseDto.AddmissionTrans = admissionTan;
            return View(warehouseDto);
        }
        public StockController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult CreateItem()
        {
            GRPVDto GRPV = new GRPVDto();
            var stockService = new StockService();
            GRPV.drugList = GetAllDrugs();
            GRPV.supplierList = GetAllGRN();
            var requestDetails = stockService.GetAllStockRequestDetails();
            GRPV.DrugsCategory = GetAllDrugsCategory();
            GRPV.RqeuestList = requestDetails
            .GroupBy(rd => rd.RequestID)
             .Select(group => new RequestDetailsDto
             {
                 RequestID = group.Key,
                 CreatedBy = group.First().CreatedBy,
                 CreateDate = group.First().CreateDate,
                 Items = group.Select(item => new RequestItemDetailsDto
                 {
                     RequestID = item.StockRequestItemID,
                     DrugName = item.DrugName,
                     Quantity = item.Quantity
                 }).ToList()
             })
         .ToList();
            return PartialView("_PartialAddItem", GRPV);
        }
       
        [HttpPost]
        public JsonResult AddItem([FromBody] GRPVDto grpvDto)
        {
            var userId = HttpContext.Request.Cookies["UserIdCookie"];
            if (userId != null)
            {
                grpvDto.CreateUser = Convert.ToInt32(userId);
                grpvDto.ModifiedUser = Convert.ToInt32(userId);
            }
            var drugService = new DrugsService();
            var stockService = new StockService();
            Drug drug = new Drug();

            foreach (var item in grpvDto.Items)
            {
                drug = drugService.GetAllDrugByID(item.DrugID);
                if (drug.SNo == "New Insert")

                {
                    var drugs = new Drug {
                        Id = item.DrugID,
                        SNo = "GRN",
                        DrugName = drug.DrugName,
                        Price = item.Price + ((decimal)item.Price / 100) * 30,
                        ModifiedUser = item.ModifiedUser,
                        DrugsCategoryId = item.Category,
                        DrugsSubCategoryId = item.SubCategory,
                        billingItemsType = BillingItemsType.Drugs,
                        Qty = item.Qty,
                        isStock = 0,
                        BatchNumber = item.BatchNumber,
                        IsDiscountAvailable = true,
                        ModifiedDate = DateTime.Now,
                        CreateUser = item.CreateUser,
                        Status = 0 // Or any default status
                    };
                    drugService.CreateDrugs(drugs);
                    var addgrrn = new GRPV
                    {
                        DrugId = item.DrugID,
                        GRNId = item.GRNId,
                        BatchNumber = item.BatchNumber,
                        SN = item.SerialNumber,
                        Qty = item.Qty,
                        Price = item.Price,
                        SellPrecentage = 30,
                        ExpiryDate = item.ExpireDate,
                        ProductDate = item.ProductDate,
                        CreateUser = item.CreateUser,
                        ModifiedUser = item.ModifiedUser,
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };
                    stockService.AddGRPV(addgrrn);

                }
                else {
                    var drugs = new Drug
                    {
                        Id = item.DrugID,
                        SNo = drug.SNo,
                        DrugName = drug.DrugName,
                        Price = item.Price + ((decimal)item.Price / 100) * 30,
                        ModifiedUser = drug.ModifiedUser,
                        DrugsCategoryId = drug.DrugsCategoryId,
                        DrugsSubCategoryId = drug.DrugsSubCategoryId,
                        billingItemsType = drug.billingItemsType,
                        Qty = drug.Qty,
                        isStock = drug.isStock,
                        BatchNumber = item.BatchNumber,
                        IsDiscountAvailable = drug.IsDiscountAvailable,
                        ModifiedDate = DateTime.Now,
                        CreateUser = drug.CreateUser,
                        Status = drug.Status // Or any default status
                    };
                    drugService.CreateDrugs(drugs);
                    var grpv = new GRPV
                    {
                    DrugId = item.DrugID,
                    GRNId = item.GRNId,
                    BatchNumber = item.BatchNumber,
                    SN = item.SerialNumber,
                    Qty = item.Qty,
                    Price = item.Price,
                    SellPrecentage = 30,
                    ExpiryDate = item.ExpireDate,
                    ProductDate = item.ProductDate,
                    CreateUser = item.CreateUser,
                    ModifiedUser = item.ModifiedUser,
                    CreateDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                    };
                 stockService.AddGRPV(grpv);
                }
            }
            //stockService.PurchaseReqestSStatusUpdate(item.RequestId);
            return Json(new { success = true, message = "Items successfully added!" });
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

        

        public IActionResult CreateTranfer()
        {
            ItemTranferDto itemTranfer = new ItemTranferDto();
            itemTranfer.Warehouses = GetAllStore();
            itemTranfer.GRPV = GetAllGRPV();

            return PartialView("_PartialAddTranfer", itemTranfer);
        }
        public IActionResult CreatePurchaseRquest()
        {
            PurchaseRequestDto itemTranfer = new PurchaseRequestDto();
            itemTranfer.drugList = GetAllDrugs();
            itemTranfer.supplierList = GetAllGRN();

            return PartialView("_PartialAddPR", itemTranfer);
        }
        [HttpPost]
        public IActionResult AddPurchaseRequest([FromBody] RequestDto stockRequestDto)
        {
            var userId = HttpContext.Request.Cookies["UserIdCookie"];
            var StockService = new StockService();
            if (stockRequestDto == null || stockRequestDto.Items == null || !stockRequestDto.Items.Any())
            {
                return Json(new { success = false, message = "Invalid request data." });
            }

            try
            {
                var stockRequest = new StockRequest
                {
                    CreateUser = Convert.ToInt32(userId),
                    ModifiedUser = Convert.ToInt32(userId),
                    CreateDate = DateTime.Now,
                    Items = stockRequestDto.Items.Select(i => new StockRequestItem
                    {
                        DrugID = i.DrugID,
                        Quntity = i.Quntity,
                        CreateUser = i.CreateUser,
                        ModifiedUser = i.ModifiedUser,
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    }).ToList()
                };
                
                var result = StockService.AddPurchaseRequest(stockRequest);
                return Json(new { success = true, message = "Purchase request added successfully!" });


                
            }
            catch (Exception ex)
            {
                // Log the exception (use a proper logger in production)
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, message = "An error occurred while saving data." });

            }
        }
        [HttpPost]
        public IActionResult CheckAndAddDrug([FromBody] DrugRequestDto drugRequest)
        {
            if (string.IsNullOrEmpty(drugRequest?.DrugName))
            {
                return Json(new { success = false, message = "Drug name is required." });
            }

            try
            {
                var normalizedDrugName = drugRequest.DrugName.Trim().ToUpper();
                var drugService = new DrugsService();

                // Check if the drug exists
                var existingDrug = drugService.GetDrugByName(normalizedDrugName);

                if (existingDrug != null)
                {
                    // Drug exists
                    return Json(new
                    {
                        success = true,
                        data = new { drugId = existingDrug.Id, drugName = existingDrug.DrugName }
                    });
                }

                // Drug does not exist, add it
                var newDrug = new Drug
                {
                    SNo = "New Insert",
                    DrugName = normalizedDrugName,
                    Price = 0,
                    CreateUser = 0,
                    ModifiedUser = 0,
                    DrugsCategoryId = 19,
                    DrugsSubCategoryId = 22,
                    billingItemsType = BillingItemsType.OTHER,
                    Qty = 0,
                    isStock = 0,
                    IsDiscountAvailable = true,
                    CreateDate = DateTime.Now,
                    Status = 0
                };
                drugService.CreateDrugs(newDrug);

                return Json(new
                {
                    success = true,
                    data = new { drugId = newDrug.Id, drugName = newDrug.DrugName }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error occurred: " + ex.Message });
            }
        }


        public IActionResult CreateGRN()
        {
            return PartialView("_PartialAddGRN");
        }
        public IActionResult CreateStore()
        {
            return PartialView("_PartialAddStore");
        }
        public IActionResult AddGRN(GRN stockDto)
        {
            var StockService = new StockService();
            var userId = HttpContext.Request.Cookies["UserIdCookie"];

            if (userId != null)
            {
                stockDto.CreateUser = Convert.ToInt32(userId);
                stockDto.ModifiedUser = Convert.ToInt32(userId);
            }



            if (ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
                return PartialView("_PartialAddGRN", stockDto);
            }
            StockService.AddGRN(stockDto);
            return RedirectToAction("Index");

        }
        public IActionResult AddStore(Warehouse stockDto)
        {
            var StockService = new StockService();
            var userId = HttpContext.Request.Cookies["UserIdCookie"];

            if (userId != null)
            {
                stockDto.CreateUser = Convert.ToInt32(userId);
                stockDto.ModifiedUser = Convert.ToInt32(userId);
            }

            stockDto.CreateDate = DateTime.Now;
            stockDto.ModifiedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
                return PartialView("_PartialAddStore", stockDto);
            }
            StockService.AddStore(stockDto);
            return RedirectToAction("Index");

        }
        private List<Drug> GetAllDrugs()
        {
            List<Drug> DrugsList = new List<Drug>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Drugs/");
                    var postObj = httpClient.GetFromJsonAsync<List<Drug>>("GetAllDrugs");
                    postObj.Wait();
                    DrugsList = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return DrugsList;
        }
        private List<GRN> GetAllGRN()
        {
            List<GRN> GRNList = new List<GRN>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Stock/");
                    var postObj = httpClient.GetFromJsonAsync<List<GRN>>("GetAllGRN");
                    postObj.Wait();
                    GRNList = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return GRNList;
        }
        private List<GRPVDetailsDto> GetAllGRPV()
        {
            List<GRPVDetailsDto> GRNList = new List<GRPVDetailsDto>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Stock/");
                    var postObj = httpClient.GetFromJsonAsync<List<GRPVDetailsDto>>("GetAllGRPV");
                    postObj.Wait();
                    GRNList = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return GRNList;
        }
        private List<Warehouse> GetAllStore()
        {
            List<Warehouse> GRNList = new List<Warehouse>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Stock/");
                    var postObj = httpClient.GetFromJsonAsync<List<Warehouse>>("GetAllStore");
                    postObj.Wait();
                    GRNList = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return GRNList;
        }
        public IActionResult AddTranfer(ItemTranferDto tranferDto)
        {
            var StockService = new StockService();
            var userId = HttpContext.Request.Cookies["UserIdCookie"];

            if (userId != null)
            {
                tranferDto.CreateUser = Convert.ToInt32(userId);
                tranferDto.ModifiedUser = Convert.ToInt32(userId);
            }



            if (ModelState.IsValid)
            {

                return PartialView("_PartialAddGRN", tranferDto);
            }
            StockService.AddTransaction(
                tranferDto.DrugId,
                tranferDto.Qty,
                tranferDto.FromWarehouses,
                tranferDto.ToWarehouse,
                tranferDto.CreateUser,
                tranferDto.GRPVId,
                tranferDto.GRNId,
                tranferDto.BatchNumber

                );
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> GetBatchDetails(int drugId)
        {
            List<ItemTranferDto> BatchList = new List<ItemTranferDto>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Stock/");

                    // Making async call to fetch the data
                    BatchList = await httpClient.GetFromJsonAsync<List<ItemTranferDto>>($"GetBatchbyDrugID?drugId={drugId}");
                }
                catch (Exception ex)
                {
                    // Log the exception if necessary
                    Console.WriteLine($"Error fetching batch details: {ex.Message}");
                    return BadRequest("Error retrieving batch details.");
                }
            }

            return Json(BatchList);
        }
       
    
        public IActionResult GetAllStockRequests()
        {
            try
            {
                // Get all request details
                var requestDetails = new StockService().GetAllStockRequestDetails();

                // Group the details by RequestID for easier rendering in the view
                var groupedRequests = requestDetails
                    .GroupBy(rd => rd.RequestID)
                    .Select(group => new RequestDetailsDto
                    {
                        RequestID = group.Key,
                        CreatedBy = group.First().CreatedBy,
                        CreateDate = group.First().CreateDate,
                        Items = group.Select(item => new RequestItemDetailsDto
                        {   RequestID = item.RequestID,
                            DrugName = item.DrugName,
                            Quantity = item.Quantity
                        }).ToList()
                    })
                    .ToList();

                return View(groupedRequests); // Pass the grouped data to the view
            }
            catch (Exception ex)
            {
                // Log error and return an empty list
                return View(new List<RequestDetailsDto>());
            }
        }
        [HttpPost]
        public ActionResult PrintRequest(int requestId)
        {
            try
            {
               
                var requestData = new StockService().GetStockRequestDetails(requestId);

                if (requestData == null || !requestData.Any())
                {
                    return NotFound($"No stock request found for Request ID: {requestId}");
                }

                var groupedpintdata = requestData
                    .GroupBy(rd => rd.RequestID)
                    .Select(group => new RequestDetailsDto
                    {
                        RequestID = group.Key,
                        CreatedBy = group.First().CreatedBy,
                        CreateDate = group.First().CreateDate,
                        Items = group.Select(item => new RequestItemDetailsDto
                        {
                            RequestID = item.RequestID,
                            DrugName = item.DrugName,
                            Quantity = item.Quantity
                        }).ToList()
                    })
                    .ToList();
				var warehouseDto = new WarehouseDto
				{
					RqeuestList = groupedpintdata
				};
				// Pass only the relevant data to the view
				return View("_PartialViewRequest", warehouseDto);
            }
            catch (Exception ex)
            {
                // Log the exception here (optional)
                // Redirect to an error page or back to the main page with an error message
                return RedirectToAction("Index", new { error = ex.Message });
            }
        }
        public (List<MainStoresDto>, List<OPDStoresDto>, List<AddmisionStoresDto>) GetOPDDrugDetails()
        {
            int MainIn = (int)StoreTranMethod.Main_In;
            int MainOut = (int)StoreTranMethod.Main_Out;
            int OPDIn = (int)StoreTranMethod.OPD_IN;
            int OPDOut = (int)StoreTranMethod.OPD_Out;
            int AddminIn = (int)StoreTranMethod.Addmission_In;
            int AddminOut = (int)StoreTranMethod.Addmission_Out;
            var maindrugList = new StockService().GetStoresDetailsByTranType(MainIn, MainOut);
            var mainStoresList = maindrugList.Select(drug => new MainStoresDto
            {
                DrugName = drug.DrugName,
                BatchNumber = drug.BatchNumber,
                StockIn = drug.StockIn,
                StockOut = drug.StockOut,
                AvailableQuantity = drug.AvailableQuantity,
                Price = drug.Price,
                Amount = drug.Amount,
                SupplierName = drug.SupplierName,
                ExpiryDate = drug.ExpiryDate
            }).ToList();
            var opdrugList = new StockService().GetStoresDetailsByTranType(OPDIn, OPDOut);
            var opdStoresList = opdrugList.Select(drug => new OPDStoresDto
            {
                DrugName = drug.DrugName,
                BatchNumber = drug.BatchNumber,
                StockIn = drug.StockIn,
                StockOut = drug.StockOut,
                AvailableQuantity = drug.AvailableQuantity,
                Price = drug.Price,
                Amount = drug.Amount,
                SupplierName = drug.SupplierName,
                ExpiryDate = drug.ExpiryDate
            }).ToList();
            var AddminrugList = new StockService().GetStoresDetailsByTranType(AddminIn, AddminOut);
            var AddminStoresList = AddminrugList.Select(drug => new AddmisionStoresDto
            {
                DrugName = drug.DrugName,
                BatchNumber = drug.BatchNumber,
                StockIn = drug.StockIn,
                StockOut = drug.StockOut,
                AvailableQuantity = drug.AvailableQuantity,
                Price = drug.Price,
                Amount = drug.Amount,
                SupplierName = drug.SupplierName,
                ExpiryDate = drug.ExpiryDate
            }).ToList();

            return (mainStoresList, opdStoresList, AddminStoresList);
        }
        public (List<OPDStoresDetaisDto>, List<AddmisionStoresDetailsDto>) DrugDetailsbydate()
        {
            
            int OPDIn = (int)StoreTranMethod.OPD_IN;
            int OPDOut = (int)StoreTranMethod.OPD_Out;
            int OPDRefund = (int)StoreTranMethod.OPD_Refund;
            int AddminIn = (int)StoreTranMethod.Addmission_In;
            int AddminOut = (int)StoreTranMethod.Addmission_Out;
            int AddminRefund = (int)StoreTranMethod.Addmission_Refund;
            DateTime Start =  DateTime.Today;
            DateTime End = Start.AddDays(1).AddMilliseconds(-1); 
            var opdrugList = new StockService().GetStoresDetailsByTranTypeByDate(OPDIn, OPDOut,OPDRefund,Start,End);
            var opdStoresList = opdrugList.Select(drug => new OPDStoresDetaisDto
            {
                DrugName = drug.DrugName,
                BatchNumber = drug.BatchNumber,
                StockIn = drug.StockIn,
                StockOut = drug.StockOut,
                RefundQty = drug.RefundQty,
                AvailableQuantity = drug.AvailableQuantity,
                Price = drug.Price,
                Amount = drug.Amount,
                RefNumber =drug.RefNumber,
                ExpiryDate = drug.ExpiryDate
            }).ToList();
            var AddminrugList = new StockService().GetStoresDetailsByTranTypeByDate(AddminIn, AddminOut, AddminRefund, Start, End);
            var AddminStoresList = AddminrugList.Select(drug => new AddmisionStoresDetailsDto
            {
                DrugName = drug.DrugName,
                BatchNumber = drug.BatchNumber,
                StockIn = drug.StockIn,
                StockOut = drug.StockOut,
                RefundQty= drug.RefundQty,
                AvailableQuantity = drug.AvailableQuantity,
                Price = drug.Price,
                Amount = drug.Amount,
                RefNumber = drug.RefNumber,
                ExpiryDate = drug.ExpiryDate
            }).ToList();

            return (opdStoresList, AddminStoresList);
        }

        [HttpPost]
        public JsonResult RequestDetails(int requestId)
        {
            try
            {
                var requestData = new StockService().GetReqestDetailsByRequestID(requestId);
                var suppliers = GetAllGRN();

                if (requestData == null || !requestData.Any())
                {
                    return Json(new { success = false, message = $"No stock request found for Request ID: {requestId}" });
                }

                return Json(new { success = true, data = requestData, suppliers });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }

}
