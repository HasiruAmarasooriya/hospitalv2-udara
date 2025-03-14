using System.Reflection.Metadata;
using System.Text.Json;
using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.Admission;
using HospitalMgrSystem.Service.Cashier;
using HospitalMgrSystem.Service.CashierSession;
using HospitalMgrSystem.Service.ChannelingSchedule;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Drugs;
using HospitalMgrSystem.Service.OPD;
using HospitalMgrSystem.Service.OtherTransactions;
using HospitalMgrSystem.Service.Patients;
using HospitalMgrSystem.Service.Stock;
using HospitalMgrSystem.Service.User;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using static iTextSharp.text.pdf.AcroFields;

namespace HospitalMgrSystemUI.Controllers
{
    public class AdmissionController : Controller
    {
        private IConfiguration _configuration;

        [BindProperty]
        public AdmissionDto _AdmissionDto { get; set; }

        [BindProperty]
        public Admission myAdmission { get; set; }
        [BindProperty]
        public string SearchValue { get; set; }

        public AdmissionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            AdmissionDto admissionDto = new AdmissionDto();
            admissionDto.listAdmission = LoadAdmission().OrderByDescending(a => a.Id).ToList();
            return View(admissionDto);
        }

        public ActionResult CreateAdmission(int id)
        {
            AdmissionDto admissionDto = new AdmissionDto();
            admissionDto.Rooms = LoadRoomList();
            admissionDto.Consultants = LoadConsultants();
          
            admissionDto.HospitalFees = LoadHospitalFee();
            AdmissionService service = new AdmissionService();
            PatientService patient = new PatientService();
            Patient patientName = new Patient();
            if (id > 0)
            {
                using (var httpClient = new HttpClient())
                {

                    try
                    {
                       
                        admissionDto.Admissions = service.GetAdmissionByID(id);
                        patientName = patient.GetAllPatientByID(admissionDto.Admissions.PatientId);
                        admissionDto.Admissions.PatientName = patientName.FullName;
                        admissionDto.Charges = service.GetAdmissionHospitalFeesbyId(id);
                        return PartialView("_PartialAddAdmission", admissionDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            else
                return PartialView("_PartialAddAdmission", admissionDto);
        }
       
        public ActionResult CreateAdmissionWithQr(int Id)
        {
            AdmissionService service = new AdmissionService();
            AdmissionDto admissionDto = new AdmissionDto();
            var response = service.GetAdmissionByID(Id);
            admissionDto.Admissions = response;
            var patient = new PatientService().GetAllPatientByID(response.PatientId);
            admissionDto.AdmissionDrugusList = GetAdmissionDrugus(Id);
            admissionDto.AdmissionInvestigationList = GetAdmissionInvestigation(Id);
            admissionDto.AdmissionConsultantList = GetAdmissionConsultant(Id);
            admissionDto.AdmissionItemsList = GetAdmissionItems(Id);
            admissionDto.Charges = service.GetAdmissionHospitalFeesbyId(Id);
            var drugAmount = admissionDto.AdmissionDrugusList.Sum(d => d.Amount);
            var invAmount = admissionDto.AdmissionInvestigationList.Sum(d => d.Amount);
            var consAmount = admissionDto.AdmissionConsultantList.Sum(d => d.Amount);
            var itemAmount = admissionDto.AdmissionItemsList.Sum(d => d.Amount);
            var hospitalfee = admissionDto.Charges.Sum(d => d.Amount);
            admissionDto.TotalAmount = drugAmount + invAmount + consAmount + itemAmount+hospitalfee;
            admissionDto.AdmissionsId = Id;
            admissionDto.name = patient.FullName;
            admissionDto.age = patient.Age;
            admissionDto.sex = patient.Sex;
            admissionDto.phone = patient.TelephoneNumber;
            admissionDto.CreatedUserName = patient.CreateUser;


            return PartialView("_PartialQR", admissionDto);
        }
        [HttpGet]
        public IActionResult DischargeAdmission(int id)
        {
            try
            {
                if (id ==0)
                {
                    return Json(new { success = false, message = "AdmissionDto not Selected" });
                }


                // Call the AddAdmission method
                bool result = CreateAdmissionDischarge(id);

                if (result)
                {
                    return Json(new { success = true, message = "Discharge  successfully." },new JsonSerializerOptions { PropertyNamingPolicy = null });
                }
                else
                {
                    return Json(new { success = false, message = "Admission Alredy Discharrged" }, new JsonSerializerOptions { PropertyNamingPolicy = null });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while Discharrge the admission." }, new JsonSerializerOptions { PropertyNamingPolicy = null });
            }
        }
        public bool CreateAdmissionDischarge(int Id)
        {
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            AdmissionService service = new AdmissionService();
            AdmissionDto admissionDto = new AdmissionDto();
            List<CashierSession> mtList = new List<CashierSession>();
            var response = service.GetAdmissionByID(Id);
            if(response.Discription != "Discharged" || response.DischargeDate == DateTime.MinValue)
            {
                response.DischargeDate = DateTime.Now;
                response.Discription = "Discharged";
                response.Id = Id;
                Admission admissionService = new AdmissionService().CreateAdmission(response);



                mtList = new CashierSessionService().GetACtiveCashierSessions(Convert.ToInt32(userIdCookie));
                var counsultat = service.GetAdmissionConsultantbyAdmissionID(Id);

                if (counsultat != null && counsultat.Count > 0)
                {
                    foreach (var items in counsultat)
                    {


                        OtherTransactions otherTransactions = new OtherTransactions();

                        otherTransactions.SessionID = mtList[0].Id;
                        otherTransactions.ConvenerID = Convert.ToInt32(userIdCookie);
                        otherTransactions.InvoiceType = InvoiceType.ADM_DOCTOR_PAYMENT;
                        otherTransactions.Amount = items.DoctorFee;
                        otherTransactions.Description = "DOCTOR_PAYMENT";
                        otherTransactions.ApprovedByID = Convert.ToInt32(userIdCookie);
                        otherTransactions.Status = CommonStatus.Active;
                        otherTransactions.otherTransactionsStatus = OtherTransactionsStatus.Cashier_Out;
                        otherTransactions.CreateUser = Convert.ToInt32(userIdCookie);
                        otherTransactions.ModifiedUser = Convert.ToInt32(userIdCookie);
                        otherTransactions.CreateDate = DateTime.Now;
                        otherTransactions.ModifiedDate = DateTime.Now;
                        otherTransactions.BeneficiaryID = items.ConsultantId;
                        otherTransactions.SchedularId = Id;

                        OtherTransactions savedOtherTransaction =
                            new OtherTransactionsService().CreateOtherTransactions(otherTransactions);

                        Invoice invoice = new Invoice();

                        invoice.CustomerID = items.ConsultantId;
                        invoice.CustomerName = items.ConsultantName;
                        invoice.InvoiceType = InvoiceType.ADM_DOCTOR_PAYMENT;
                        invoice.paymentStatus = PaymentStatus.PAID;
                        invoice.Status = InvoiceStatus.New;
                        invoice.CreateUser = Convert.ToInt32(userIdCookie);
                        invoice.ModifiedUser = Convert.ToInt32(userIdCookie);
                        invoice.CreateDate = DateTime.Now;
                        invoice.ModifiedDate = DateTime.Now;
                        invoice.ServiceID = savedOtherTransaction.Id;


                        Invoice savedInvoice = new CashierService().AddInvoice(invoice);

                        InvoiceItem invoiceItem = new InvoiceItem();

                        invoiceItem.InvoiceId = savedInvoice.Id;
                        invoiceItem.ItemID = 2;
                        invoiceItem.billingItemsType = BillingItemsType.OTHER;
                        invoiceItem.price = otherTransactions.Amount;
                        invoiceItem.qty = 1;
                        invoiceItem.Discount = 0;
                        invoiceItem.Total = invoiceItem.price * invoiceItem.qty;
                        invoiceItem.CreateDate = DateTime.Now;
                        invoiceItem.CreateUser = Convert.ToInt32(userIdCookie);
                        invoiceItem.ModifiedDate = DateTime.Now;
                        invoiceItem.ModifiedUser = Convert.ToInt32(userIdCookie);
                        invoiceItem.Status = CommonStatus.Active;
                        invoiceItem.itemInvoiceStatus = ItemInvoiceStatus.BILLED;

                        InvoiceItem savedInvoiceItem = new CashierService().AddSingleInvoiceItem(invoiceItem);

                        Payment payment = new Payment();

                        payment.InvoiceID = savedInvoice.Id;
                        payment.CashAmount = 0 - invoiceItem.Total;
                        payment.CreditAmount = 0;
                        payment.DdebitAmount = 0;
                        payment.ChequeAmount = 0;
                        payment.GiftCardAmount = 0;
                        payment.CreateUser = Convert.ToInt32(userIdCookie);
                        payment.ModifiedUser = Convert.ToInt32(userIdCookie);
                        payment.CreateDate = DateTime.Now;
                        payment.ModifiedDate = DateTime.Now;
                        payment.CashierStatus = CashierStatus.CashierOut;
                        payment.BillingType = BillingType.OTHER_OUT;
                        payment.sessionID = mtList[0].Id;

                        Payment savedPayment = new CashierService().AddPayments(payment);
                    }
                }


                return true;
            }
            else
            {
                return false;
            }
            
        }
        public ActionResult AddDrugusToAdmission(int Id)
        {
            AdmissionDto admissionDto = new AdmissionDto();
            admissionDto.Drugs = DrugsSearch();
            admissionDto.AdmissionsId = Id;
            admissionDto.AdmissionDrugusList = GetAdmissionDrugus(Id);

            return PartialView("_PartialAddDrugus", admissionDto);
        }

        public ActionResult AddInvestigationToAdmission(int Id)
        {
            AdmissionDto admissionDto = new AdmissionDto();
            admissionDto.Investigations = GetInvestigations();
            admissionDto.AdmissionsId = Id;
            admissionDto.AdmissionInvestigationList = GetAdmissionInvestigation(Id);

            return PartialView("_PartialInvestigation", admissionDto);
        }

        public ActionResult AddConsultantToAdmission(int Id)
        {
            AdmissionDto admissionDto = new AdmissionDto();
            admissionDto.Consultants = SearchConsultant();
            admissionDto.AdmissionsId = Id;
            admissionDto.AdmissionConsultantList = GetAdmissionConsultant(Id);

            return PartialView("_PartialConsultant", admissionDto);
        }

        public ActionResult AddItemsToAdmission(int Id)
        {
            AdmissionDto admissionDto = new AdmissionDto();
            admissionDto.Items = SearchItems();
            admissionDto.AdmissionsId = Id;
            admissionDto.AdmissionItemsList = GetAdmissionItems(Id);

            return PartialView("_PartialAddItems", admissionDto);
        }

        public IActionResult DeleteAdmission()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.PostAsJsonAsync<Admission>("DeleteAdmission?", myAdmission);
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
        
        private List<Room> LoadRoomList()
        {
            List<Room> rooms = new List<Room>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Room/");
                    var postObj = httpClient.GetFromJsonAsync<List<Room>>("GetRooms");
                    postObj.Wait();
                    rooms = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return rooms;
        }


        private List<Consultant> LoadConsultants()
        {
            List<Consultant> consultants = new List<Consultant>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Consultant/");
                    var postObj = httpClient.GetFromJsonAsync<List<Consultant>>("GetAllConsultant");
                    postObj.Wait();
                    consultants = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return consultants;
        }

        private List<Patient> LoadPatient()
        {
            List<Patient> consultants = new List<Patient>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Patient/");
                    var postObj = httpClient.GetFromJsonAsync<List<Patient>>("GetAllPatients");
                    postObj.Wait();
                    consultants = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return consultants;
        }
        private List<AdmissionHospitalFee> LoadHospitalFee() 
        { 
            List<AdmissionHospitalFee> hospitalFee = new List<AdmissionHospitalFee>();
            var adminservice = new AdmissionService();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    hospitalFee = adminservice.GetAdmissionHospitalChagers();
                }
                catch (Exception ex)
                {
                }
            }
            return hospitalFee; 
        }
        private List<Admission> LoadAdmission()
        {
            List<Admission> consultants = new List<Admission>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    consultants = new AdmissionService().GetAllAdmission();

                }
                catch (Exception ex) { }
            }
            return consultants;
        }
        [HttpPost]
        public IActionResult AddNewAdmission([FromBody] AdmissionDto admissionDto)
        {
            try
            {
                if (admissionDto == null)
                {
                    return Json(new { success = false, message = "AdmissionDto is null." });
                }

                // Deserialize the HospitalFeeList
                if (admissionDto.HospitalFeeList == null)
                {
                    admissionDto.HospitalFeeList = new List<HospitalFeeListDto>();
                }

                // Call the AddAdmission method
                bool result = AddAdmission(admissionDto);

                if (result)
                {
                    return Json(new { success = true, message = "Admission saved successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save admission." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while saving the admission." });
            }
        }
        private bool AddAdmission(AdmissionDto admissionDto)
        {
            
            try
            {
                var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
                int userId = Convert.ToInt32(userIdCookie);
                Patient patient = new Patient();
                Patient savedpatient = new Patient();
                PatientService patientService = new PatientService();
                ConsultantService consultantService = new ConsultantService();
                var selectedConsultant = new AdmissionConsultant();
                var savedAdmission = new Admission();

                if (admissionDto.Admissions.Id != 0 )
                {
                   
                    savedAdmission = new AdmissionService().GetAdmissionByID(admissionDto.Admissions.Id);
                    if(savedAdmission.paymentStatus == PaymentStatus.PAID || savedAdmission.paymentStatus ==PaymentStatus.PARTIAL_PAID)
                    {
                        throw new Exception("Admission Allredy Paid");
                    }
                    if (savedAdmission != null)
                    {
                        // Update existing admission
                        savedAdmission.Id = admissionDto.Admissions.Id;
                        savedAdmission.PatientId = admissionDto.Admissions.PatientId;
                        savedAdmission.ConsultantId = admissionDto.Admissions.ConsultantId;
                        savedAdmission.ModifiedUser = userId;
                        savedAdmission.ModifiedDate = DateTime.Now;
                        new AdmissionService().CreateAdmission(savedAdmission);

                       List<AdmissionConsultant> ConsultantList = new AdmissionService().GetAdmissionConsultant(admissionDto.Admissions.Id, PaymentStatus.PAID);
                        foreach (var item in ConsultantList)
                        {
                            var consultant = new AdmissionConsultant
                            {
                                Id = item.Id,
                                AdmissionId = savedAdmission.Id,
                                ConsultantId = item.ConsultantId,
                                HospitalFee = item.HospitalFee,
                                DoctorFee = item.DoctorFee,
                                Amount = (selectedConsultant.HospitalFee) + (selectedConsultant.DoctorFee),
                                paymentStatus = PaymentStatus.NOT_PAID,
                                itemInvoiceStatus = ItemInvoiceStatus.Add,
                                CreateUser = Convert.ToInt32(userIdCookie),
                                ModifiedUser = Convert.ToInt32(userIdCookie),
                                CreateDate = DateTime.Now,
                                ModifiedDate = DateTime.Now
                            };
                            new AdmissionService().CreateAdmissionConsultant(consultant);
                        }
                        

                        if (admissionDto.HospitalFeeList != null && admissionDto.HospitalFeeList.Any())
                        {
                            foreach (var fee in admissionDto.HospitalFeeList)
                            {
                                var admissionCharge = new AdmissionsCharges
                                {
                                    AdmissionId = savedAdmission.Id,
                                    HospitalFeeId = fee.FeeId,
                                    Qty = fee.Qty,
                                    Price = fee.Price,
                                    Amount = fee.Price * fee.Qty,
                                    Status = CommonStatus.Active,
                                    CreateUser = Convert.ToInt32(userIdCookie),
                                    ModifiedUser = Convert.ToInt32(userIdCookie),
                                    CreateDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now,
                                    paymentStatus = PaymentStatus.NOT_PAID,
                                    itemInvoiceStatus = ItemInvoiceStatus.Add,
                                    IsRefund = 0
                                };
                                new AdmissionService().CreateAdmissionCharge(admissionCharge);
                            }
                        }
                    }
                }
                else
                {
                    patient.Id = admissionDto.Patient.Id;
                    patient.FullName = admissionDto.Patient.FullName;
                    patient.CreateDate = DateTime.Now;
                    patient.ModifiedDate = DateTime.Now;
                    patient.Status = PatientStatus.New;
                    patient.NIC = admissionDto.Patient.NIC;
                    patient.Address = admissionDto.Patient.Address;
                    patient.Days = admissionDto.Patient.Days;
                    patient.Months = admissionDto.Patient.Months;
                    patient.Age = admissionDto.Patient.Age;
                    patient.MobileNumber = admissionDto.Patient.MobileNumber;
                    patient.Sex = admissionDto.Patient.Sex;
                    savedpatient = patientService.CreatePatient(patient);


                    admissionDto.Admissions.PatientId = savedpatient.Id;
                    admissionDto.Admissions.CreateUser = userId;
                    admissionDto.Admissions.ModifiedUser = userId;
                    admissionDto.Admissions.CreateDate = DateTime.Now;
                    admissionDto.Admissions.ModifiedDate = DateTime.Now;
                    admissionDto.Admissions.invoiceType = InvoiceType.ADM;
                    admissionDto.Admissions.paymentStatus = PaymentStatus.NOT_PAID;
                    admissionDto.Admissions.itemInvoiceStatus = ItemInvoiceStatus.Add;
                    savedAdmission = new AdmissionService().CreateAdmission(admissionDto.Admissions);
                        if (savedAdmission == null || savedAdmission.Id == 0)
                        {
                            throw new Exception("Failed to save the admission.");
                        }

                        Consultant saveconsultant = new Consultant();
                   

                    saveconsultant = consultantService.GetAllConsultantByID(admissionDto.Admissions.ConsultantId);
                        AdmissionConsultant new_consultant = new AdmissionConsultant

                        {
                            AdmissionId = savedAdmission.Id,
                            ConsultantId = admissionDto.Admissions.ConsultantId,
                            HospitalFee = saveconsultant.HospitalFee ?? 0,
                            DoctorFee = saveconsultant.DoctorFee ?? 0,
                            Amount = (saveconsultant.HospitalFee ?? 0) + (saveconsultant.DoctorFee ?? 0),
                            paymentStatus = PaymentStatus.NOT_PAID,
                            itemInvoiceStatus = ItemInvoiceStatus.Add,
                            CreateUser = Convert.ToInt32(userIdCookie),
                            ModifiedUser = Convert.ToInt32(userIdCookie),
                            CreateDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        new AdmissionService().CreateAdmissionConsultant(new_consultant);




                        if (admissionDto.HospitalFeeList != null && admissionDto.HospitalFeeList.Any())
                        {
                            foreach (var fee in admissionDto.HospitalFeeList)
                            {
                                var admissionCharge = new AdmissionsCharges
                                {
                                    AdmissionId = savedAdmission.Id,
                                    HospitalFeeId = fee.FeeId,
                                    Qty = fee.Qty,
                                    Price = fee.Price,
                                    Amount = fee.Price * fee.Qty,
                                    Status = CommonStatus.Active,
                                    CreateUser = Convert.ToInt32(userIdCookie),
                                    ModifiedUser = Convert.ToInt32(userIdCookie),
                                    CreateDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now,
                                    paymentStatus = PaymentStatus.NOT_PAID,
                                    itemInvoiceStatus = ItemInvoiceStatus.Add,
                                    IsRefund = 0
                                };
                                new AdmissionService().CreateAdmissionCharge(admissionCharge);
                            }
                        }
                    
                   
                }

               

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                return false;
            }
        }

        //private bool AddAdmission()
        //{
        //    try { 
        //    var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
        //    _AdmissionDto.Admissions.CreateUser = Convert.ToInt32(userIdCookie);
        //    _AdmissionDto.Admissions.ModifiedUser = Convert.ToInt32(userIdCookie);
        //    _AdmissionDto.Admissions.CreateDate = DateTime.Now;
        //    _AdmissionDto.Admissions.ModifiedDate = DateTime.Now;
        //    _AdmissionDto.Admissions.paymentStatus = PaymentStatus.NOT_PAID;
        //    _AdmissionDto.Admissions.invoiceType = InvoiceType.ADM;
        //        var savedAdmission = new Admission();
        //        if (_AdmissionDto.AdmissionsId != 0)
        //        {
        //            savedAdmission = new AdmissionService().GetAdmissionByID(_AdmissionDto.AdmissionsId);
        //        }
        //        else
        //        {
        //            savedAdmission = new AdmissionService().
        //            (_AdmissionDto.Admissions);
        //        }
            
        //        if (savedAdmission == null || savedAdmission.Id == 0)
        //        {
        //            throw new Exception("Failed to save the admission.");
        //        }
        //        var selectedconsaltnt = new ConsultantService().GetAllConsultantByID(_AdmissionDto.Admissions.ConsultantId);
        //    var consultant = new AdmissionConsultant
        //    {
        //        AdmissionId = savedAdmission.Id,
        //        ConsultantId = _AdmissionDto.Admissions.ConsultantId,
        //        HospitalFee = selectedconsaltnt.HospitalFee ?? 0,
        //        DoctorFee = selectedconsaltnt.DoctorFee ?? 0,
        //        Amount = (selectedconsaltnt.HospitalFee ?? 0) + (selectedconsaltnt.DoctorFee ?? 0),
        //        paymentStatus = PaymentStatus.NOT_PAID,
        //        itemInvoiceStatus = ItemInvoiceStatus.Add,
        //        CreateUser = Convert.ToInt32(userIdCookie),
        //        ModifiedUser = Convert.ToInt32(userIdCookie),
        //        CreateDate = DateTime.Now,
        //        ModifiedDate = DateTime.Now
        //    };
        //    new AdmissionService().CreateAdmissionConsultant(consultant);
           
        //        if (_AdmissionDto.HospitalFeeList != null && _AdmissionDto.HospitalFeeList.Any())
        //        {
        //            foreach (var fee in _AdmissionDto.HospitalFeeList)
        //            {
        //                var admissionCharge = new AdmissionsCharges
        //                {
        //                    AdmissionId = savedAdmission.Id,
        //                    HospitalFeeId = fee.FeeId,
        //                    Qty = 1,
        //                    Price = fee.Price,
        //                    Amount = fee.Price,
        //                    Status = CommonStatus.Active,
        //                    CreateUser = Convert.ToInt32(userIdCookie),
        //                    ModifiedUser = Convert.ToInt32(userIdCookie),
        //                    CreateDate = DateTime.Now,
        //                    ModifiedDate = DateTime.Now,
        //                    paymentStatus = PaymentStatus.NOT_PAID,
        //                    itemInvoiceStatus = ItemInvoiceStatus.Add,
        //                    IsRefund = 0
        //                };
        //                new AdmissionService().CreateAdmissionCharge(admissionCharge);
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public IActionResult Search()
        {
            AdmissionDto admissionDto = new AdmissionDto();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.GetFromJsonAsync<List<Admission>>("SearchAdmission?text=" + SearchValue);
                    postObj.Wait();
                    var result = postObj.Result;
                    admissionDto.listAdmission = result;

                }
                catch (Exception ex) { }
            }
            return View("Index", admissionDto);
        }


        #region Drugs Management 
        public List<Drug> DrugsSearch()
        {
            List<Drug> drugs = new List<Drug>();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Drugs/");
                    var postObj = httpClient.GetFromJsonAsync<List<Drug>>("GetAllDrugs");
                    postObj.Wait();
                    var result = postObj.Result;
                    drugs = result;

                }
                catch (Exception ex) { }
            }
            return drugs;
        }
        [HttpPost]
        public async Task<IActionResult> AddNewDrugs([FromBody] List<AdmissionDrugus> drugus)
        {
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            
            var adminservice = new AdmissionService();
            try
            {
                foreach (var drug in drugus)
                {
                    var existingDrug = adminservice.GetAdmissionDrugusbyId(drug.DrugId, drug.AdmissionId);
                    AdmissionDrugus adminDrug;
                    var TranLog = new DrugsService().GetDrugDetailsById(drug.DrugId);
                    decimal Qty = drug.Qty;

                    if (TranLog == null)
                    {
                        var stockTran = new stockTransaction
                        {
                            BillId = drug.AdmissionId,
                            DrugIdRef = drug.DrugId,
                            Qty = -Qty,
                            TranType = StoreTranMethod.Addmission_Out,
                            RefNumber = "ADM_" + drug.AdmissionId,
                            Remark = "ADM_Drug_Issue",
                            BatchNumber = 0.ToString(),
                            CreateUser = Convert.ToInt32(userIdCookie),
                            ModifiedUser = Convert.ToInt32(userIdCookie),
                            CreateDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        new StockService().LogTransaction(stockTran);

                    }
                    else
                    {
                        var stockTran = new stockTransaction
                        {
                            BillId = drug.AdmissionId,
                            DrugIdRef = drug.DrugId,
                            Qty = -Qty,
                            TranType = StoreTranMethod.Addmission_Out,
                            RefNumber = "ADM_" + drug.AdmissionId,
                            Remark = "ADM_Drug_Issue",
                            BatchNumber = TranLog.BatchNumber,
                            CreateUser = Convert.ToInt32(userIdCookie),
                            ModifiedUser = Convert.ToInt32(userIdCookie),
                            CreateDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        new StockService().LogTransaction(stockTran);


                    }
                    if (existingDrug == null)
                    {
                        adminDrug = new AdmissionDrugus
                        {

                            AdmissionId = drug.AdmissionId,
                            DrugId = drug.DrugId,
                            Price = drug.Price,
                            Qty = drug.Qty,
                            Amount = drug.Price * drug.Qty,
                            paymentStatus = PaymentStatus.NOT_PAID,
                            itemInvoiceStatus = ItemInvoiceStatus.Add,
                            CreateUser = Convert.ToInt32(userIdCookie),
                            ModifiedUser = Convert.ToInt32(userIdCookie),
                            CreateDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                    }
                    else
                    {
                        adminDrug = new AdmissionDrugus
                        {
                            Id = existingDrug.Id, // Retaining existing ID
                            AdmissionId = drug.AdmissionId,
                            DrugId = drug.DrugId,
                            Price = drug.Price,
                            Qty = drug.Qty,
                            Amount = drug.Price * drug.Qty,
                            paymentStatus = PaymentStatus.NOT_PAID,
                            itemInvoiceStatus = ItemInvoiceStatus.Add,
                            CreateUser = Convert.ToInt32(userIdCookie),
                            ModifiedUser = Convert.ToInt32(userIdCookie),
                            CreateDate = DateTime.Now,
                            ModifiedDate = DateTime.Now

                        };
                    }
                    new AdmissionService().CreateAdmissionDrugus(adminDrug);

                   
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception (consider using logging framework)
                return RedirectToAction("Index");
            }
        }


        private List<AdmissionDrugus> GetAdmissionDrugus(int admissionId)
        {
            List<AdmissionDrugus> admissionDrugus = new List<AdmissionDrugus>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.GetFromJsonAsync<List<AdmissionDrugus>>($"GetAdmissionDrugus?AdmissionId={admissionId}");
                    postObj.Wait();
                    admissionDrugus = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return admissionDrugus;
        }

        public IActionResult DeleteAdmissionDrugus()
        {
            var AdmService = new AdmissionService();
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            using (var httpClient = new HttpClient())
            {
               var AdmId = AdmService.GetAdmissionDrugusbyrowId(myAdmission.Id);
                var TranLog = new DrugsService().GetDrugDetailsById(AdmId.DrugId);
                decimal Qty = AdmId.Qty;
                myAdmission.ModifiedDate = DateTime.Now;
                myAdmission.ModifiedUser = Convert.ToInt32(userIdCookie);
                if (TranLog == null)
                {
                    var stockTran = new stockTransaction
                    {
                        BillId = myAdmission.Id,
                        DrugIdRef = AdmId.DrugId,
                        Qty = Qty,
                        TranType = StoreTranMethod.Addmission_Out,
                        RefNumber = "ADM_" + AdmId.AdmissionId,
                        Remark = "ADM_Drug_Issue",
                        BatchNumber = 0.ToString(),
                        CreateUser = Convert.ToInt32(userIdCookie),
                        ModifiedUser = Convert.ToInt32(userIdCookie),
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };
                    new StockService().LogTransaction(stockTran);

                }
                else
                {
                    var stockTran = new stockTransaction
                    {
                        BillId = AdmId.AdmissionId,
                        DrugIdRef = AdmId.DrugId,
                        Qty = Qty,
                        TranType = StoreTranMethod.Addmission_Refund,
                        RefNumber = "ADM_" + AdmId.AdmissionId,
                        Remark = "ADM_Drug_Delete",
                        BatchNumber = TranLog.BatchNumber,
                        CreateUser = Convert.ToInt32(userIdCookie),
                        ModifiedUser = Convert.ToInt32(userIdCookie),
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };
                    new StockService().LogTransaction(stockTran);


                }

                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.PostAsJsonAsync<Admission>("DeleteAdmissionDrugus?", myAdmission);
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
        #endregion

        #region Investigation Management
        public List<Investigation> GetInvestigations()
        {
            List<Investigation> Investigation = new List<Investigation>();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Investigation/");
                    var postObj = httpClient.GetFromJsonAsync<List<Investigation>>("GetAllInvestigation");
                    postObj.Wait();
                    var result = postObj.Result;

                    Investigation = result;

                }
                catch (Exception ex) { }
            }
            return Investigation;
        }
        [HttpPost]
        public async Task<IActionResult> AddNewInvestigation([FromBody] List<AdmissionInvestigation> investigations)
        {
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            var adminservice = new AdmissionService();
                try
                {
                    foreach (var item in investigations) { 
                    var existingInvestigation = adminservice.GetAdmissionInvestigationById(item.InvestigationId, item.AdmissionId);
                        AdmissionInvestigation adminInvestigation;
                        if (existingInvestigation != null)
                        {
                           adminInvestigation = new AdmissionInvestigation
                           {
                               Id = existingInvestigation.Id,
                               AdmissionId = item.AdmissionId,
                               InvestigationId = item.InvestigationId,
                               Qty = item.Qty,
                               Price = item.Price,
                               Amount = item.Qty * item.Price,
                               paymentStatus = PaymentStatus.NOT_PAID,
                               itemInvoiceStatus = ItemInvoiceStatus.Add,
                               CreateUser = Convert.ToInt32(userIdCookie),
                               ModifiedUser = Convert.ToInt32(userIdCookie),
                               CreateDate = DateTime.Now,
                               ModifiedDate = DateTime.Now

                           };
                            
                        }
                        else
                        {
                            adminInvestigation = new AdmissionInvestigation
                            {
                                AdmissionId = item.AdmissionId,
                                InvestigationId = item.InvestigationId,
                                Qty = item.Qty,
                                Price = item.Price,
                                Amount = item.Qty * item.Price,
                                paymentStatus = PaymentStatus.NOT_PAID,
                                itemInvoiceStatus = ItemInvoiceStatus.Add,
                                CreateUser = Convert.ToInt32(userIdCookie),
                                ModifiedUser = Convert.ToInt32(userIdCookie),
                                CreateDate = DateTime.Now,
                                ModifiedDate = DateTime.Now
                            };
                        }
                    new AdmissionService().CreateAdmissionInvestigation(adminInvestigation);
                }
     
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            
        }

        private List<AdmissionInvestigation> GetAdmissionInvestigation(int admissionId)
        {
            List<AdmissionInvestigation> admissionInvestigations = new List<AdmissionInvestigation>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");

                    // Pass admissionId as a query parameter
                    var postObj = httpClient.GetFromJsonAsync<List<AdmissionInvestigation>>($"GetAdmissionInvestigation?AdmissionId={admissionId}");
                    postObj.Wait();
                    admissionInvestigations = postObj.Result;
                }
                catch (Exception ex)
                {
                    // Handle exceptions if needed
                }
            }
            return admissionInvestigations;
        }


        public IActionResult DeleteAdmissionInvestigation()
        { var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            myAdmission.ModifiedDate = DateTime.Now;
            myAdmission.ModifiedUser = Convert.ToInt32(userIdCookie);
            using (var httpClient = new HttpClient())
            {

                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.PostAsJsonAsync<Admission>("DeleteAdmissionInvestigation?", myAdmission);
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
        #endregion

        #region Consultant Management
        public List<Consultant> SearchConsultant() 
        {
            List<Consultant> consultants = new List<Consultant>();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Consultant/");
                    var postObj = httpClient.GetFromJsonAsync<List<Consultant>>("GetAllConsultant");
                    postObj.Wait();
                    var result = postObj.Result;

                    consultants = result;

                }
                catch (Exception ex) { }
            }
            return consultants;
        }

        [HttpPost]
        public IActionResult AddNewConsultant([FromBody] List<AddmisionCounsultantDto> consultants)
        {
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            try
            {
                AdmissionService adminservice = new AdmissionService();
                foreach (var consultant in consultants)
                {
                    var existing = adminservice.GetAdmissionConsultantbyId(consultant.ConsultantId, consultant.AdmissionId);
                    if(existing.AdmissionId != 0)
                    {
                        var adminconsultant = new AdmissionConsultant
                        {
                            Id =existing.Id,
                            AdmissionId = existing.AdmissionId,
                            ConsultantId = existing.ConsultantId,
                            HospitalFee = consultant.HospitalFee + existing.HospitalFee,
                            DoctorFee = consultant.DocFee + existing.DoctorFee,
                            Amount = consultant.HospitalFee + consultant.DocFee + existing.DoctorFee+existing.HospitalFee,
                            paymentStatus = PaymentStatus.NOT_PAID,
                            itemInvoiceStatus = ItemInvoiceStatus.Add,
                            CreateUser = Convert.ToInt32(userIdCookie),
                            ModifiedUser = Convert.ToInt32(userIdCookie),
                            CreateDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        //consultant.AdmissionId = consultant.AdmissionId;
                        //consultant.Amount = consultant.HospitalFee + consultant.DocFee;
                        //Save each consultant to the database (e.g., via an API call)
                        new AdmissionService().CreateAdmissionConsultant(adminconsultant);
                    }
                    else
                    {
                        var adminconsultant = new AdmissionConsultant
                        {
                            AdmissionId = consultant.AdmissionId,
                            ConsultantId = consultant.ConsultantId,
                            HospitalFee = consultant.HospitalFee,
                            DoctorFee = consultant.DocFee,
                            Amount = consultant.HospitalFee + consultant.DocFee,
                            paymentStatus = PaymentStatus.NOT_PAID,
                            itemInvoiceStatus = ItemInvoiceStatus.Add,
                            CreateUser = Convert.ToInt32(userIdCookie),
                            ModifiedUser = Convert.ToInt32(userIdCookie),
                            CreateDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        //consultant.AdmissionId = consultant.AdmissionId;
                        //consultant.Amount = consultant.HospitalFee + consultant.DocFee;
                        //Save each consultant to the database (e.g., via an API call)
                        new AdmissionService().CreateAdmissionConsultant(adminconsultant);
                    }
                    
                   
                }

                return Ok("Consultants added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        private List<AdmissionConsultant> GetAdmissionConsultant(int admissionId)
        {
            List<AdmissionConsultant> admissionConsultant = new List<AdmissionConsultant>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.GetFromJsonAsync<List<AdmissionConsultant>>($"GetAdmissionConsultant?AdmissionId={admissionId}");
                    postObj.Wait();
                    admissionConsultant = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return admissionConsultant;
        }

        public IActionResult DeleteAdmissionConsultant()
        {  var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            myAdmission.ModifiedDate = DateTime.Now;
            myAdmission.ModifiedUser = Convert.ToInt32(userIdCookie);
            using (var httpClient = new HttpClient())
            {

                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.PostAsJsonAsync<Admission>("DeleteAdmissionConsultant?", myAdmission);
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
        #endregion

        #region Item Management
        public List<HospitalMgrSystem.Model.Item> SearchItems()
        {
            List<HospitalMgrSystem.Model.Item> items = new List<HospitalMgrSystem.Model.Item>();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Items/");
                    var postObj = httpClient.GetFromJsonAsync<List<HospitalMgrSystem.Model.Item>>("GetAllInvestigation");
                    postObj.Wait();
                    var result = postObj.Result;

                    items = result;

                }
                catch (Exception ex) { }
            }
            return items;
        }
        [HttpPost]
        public IActionResult CreateAdmissionItems([FromBody] List<AdmissionItems> items)
        {
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            var adminservice = new AdmissionService();

            try
            {
               foreach (var item in items)
               {
                  AdmissionItems admissionItems;
                  var existing = adminservice.GetAdmissionItemsById(item.ItemId, item.AdmissionId);
                    if (existing.AdmissionId != 0)
                    {
                        admissionItems = new AdmissionItems
                        {
                            Id = existing.Id,
                            AdmissionId = existing.AdmissionId,
                            ItemId = existing.ItemId,
                            Type = existing.Type,
                            Qty = existing.Qty+item.Qty,
                            Price = item.Price,
                            Amount = item.Qty * item.Price+ existing.Amount,
                            paymentStatus = PaymentStatus.NOT_PAID,
                            itemInvoiceStatus = ItemInvoiceStatus.Add,
                            CreateUser = Convert.ToInt32(userIdCookie),
                            ModifiedUser = Convert.ToInt32(userIdCookie),
                            CreateDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        new AdmissionService().CreateAdmissionItems(admissionItems);
                    }
                    else
                    {
                        admissionItems = new AdmissionItems
                        {
                            AdmissionId = item.AdmissionId,
                            ItemId = item.ItemId,
                            Type = item.Type,
                            Qty = item.Qty,
                            Price = item.Price,
                            Amount = item.Qty * item.Price,
                            paymentStatus = PaymentStatus.NOT_PAID,
                            itemInvoiceStatus = ItemInvoiceStatus.Add,
                            CreateUser = Convert.ToInt32(userIdCookie),
                            ModifiedUser = Convert.ToInt32(userIdCookie),
                            CreateDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        new AdmissionService().CreateAdmissionItems(admissionItems);
                    }
                   
                   
               }
                
               return RedirectToAction("Index");
            }
                catch (Exception ex)
            {
                   return RedirectToAction("Index");
             }
            
        }

        private List<AdmissionItems> GetAdmissionItems(int admissionId)
        {
            List<AdmissionItems> admissionItems = new List<AdmissionItems>();
            AdmissionService admissionService = new AdmissionService();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    admissionItems = admissionService.GetAdmissionItemsbyAdmissionID(admissionId);

                }
                catch (Exception ex) { }
            }
            return admissionItems;
        }

        public IActionResult DeleteAdmissionItems()
        {
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            myAdmission.ModifiedDate = DateTime.Now;
            myAdmission.ModifiedUser = Convert.ToInt32(userIdCookie);
            using (var httpClient = new HttpClient())
            {

                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.PostAsJsonAsync<Admission>("DeleteAdmissionItems?", myAdmission);
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
        #endregion
        #region hospitalfee
        
        #endregion
    }
}
