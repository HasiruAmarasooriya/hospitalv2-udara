using HospitalMgrSystem.Model;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

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
            admissionDto.listAdmission = LoadAdmission();
            return View(admissionDto);
        }

        public ActionResult CreateAdmission(int id)
        {
            AdmissionDto admissionDto = new AdmissionDto();
            admissionDto.Rooms = LoadRoomList();
            admissionDto.Consultants = LoadConsultants();
            admissionDto.Patients = LoadPatient();

            if (id > 0)
            {
                using (var httpClient = new HttpClient())
                {

                    try
                    {
                        string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                        httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                        var postObj = httpClient.GetFromJsonAsync<Admission>("GetAdmissionByID?id=" + id);
                        postObj.Wait();
                        var res = postObj.Result;
                        admissionDto.Admissions = res;
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

        public ActionResult AddNewAdmission()
        {
            AddAdmission();
            return RedirectToAction("Index");
        }

        public ActionResult AddDrugusToAdmission(int Id)
        {
            AdmissionDto admissionDto = new AdmissionDto();
            admissionDto.Drugs = DrugsSearch();
            admissionDto.AdmissionsId = Id;
            admissionDto.AdmissionDrugusList = GetAdmissionDrugus();

            return PartialView("_PartialAddDrugus", admissionDto);
        }

        public ActionResult AddInvestigationToAdmission(int Id)
        {
            AdmissionDto admissionDto = new AdmissionDto();
            admissionDto.Investigations = GetInvestigations();
            admissionDto.AdmissionsId = Id;
            admissionDto.AdmissionInvestigationList = GetAdmissionInvestigation();

            return PartialView("_PartialInvestigation", admissionDto);
        }

        public ActionResult AddConsultantToAdmission(int Id)
        {
            AdmissionDto admissionDto = new AdmissionDto();
            admissionDto.consultants = SearchConsultant();
            admissionDto.AdmissionsId = Id;
            admissionDto.AdmissionConsultantList = GetAdmissionConsultant();

            return PartialView("_PartialConsultant", admissionDto);
        }

        public ActionResult AddItemsToAdmission(int Id)
        {
            AdmissionDto admissionDto = new AdmissionDto();
            admissionDto.Items = SearchItems();
            admissionDto.AdmissionsId = Id;
            admissionDto.AdmissionItemsList = GetAdmissionItems();

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

        private List<Admission> LoadAdmission()
        {
            List<Admission> consultants = new List<Admission>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.GetFromJsonAsync<List<Admission>>("GetAllAdmission");
                    postObj.Wait();
                    consultants = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return consultants;
        }

        private bool AddAdmission() 
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.PostAsJsonAsync<Admission>("CreateAdmission", _AdmissionDto.Admissions);
                    postObj.Wait();
                    var res = postObj.Result;
                    var result = res.Content.ReadFromJsonAsync<Admission>().Result;
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

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

        public IActionResult AddNewDrugs()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    _AdmissionDto.AdmissionDrugus.AdmissionId = _AdmissionDto.AdmissionsId;
                    _AdmissionDto.AdmissionDrugus.Amount = _AdmissionDto.AdmissionDrugus.Qty * _AdmissionDto.AdmissionDrugus.Price;
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.PostAsJsonAsync<AdmissionDrugus>("CreateAdmissionDrugus", _AdmissionDto.AdmissionDrugus);
                    postObj.Wait();
                    var res = postObj.Result;
                    var result = res.Content.ReadFromJsonAsync<AdmissionDrugus>().Result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        private List<AdmissionDrugus> GetAdmissionDrugus()
        {
            List<AdmissionDrugus> admissionDrugus = new List<AdmissionDrugus>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.GetFromJsonAsync<List<AdmissionDrugus>>("GetAdmissionDrugus");
                    postObj.Wait();
                    admissionDrugus = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return admissionDrugus;
        }

        public IActionResult DeleteAdmissionDrugus()
        {
            using (var httpClient = new HttpClient())
            {

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

        public IActionResult AddNewInvestigation()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    _AdmissionDto.AdmissionInvestigations.AdmissionId = _AdmissionDto.AdmissionsId;
                    _AdmissionDto.AdmissionInvestigations.Amount = _AdmissionDto.AdmissionInvestigations.Qty * _AdmissionDto.AdmissionInvestigations.Price;
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.PostAsJsonAsync<AdmissionInvestigation>("CreateAdmissionInvestigation", _AdmissionDto.AdmissionInvestigations);
                    postObj.Wait();
                    var res = postObj.Result;
                    var result = res.Content.ReadFromJsonAsync<AdmissionInvestigation>().Result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        private List<AdmissionInvestigation> GetAdmissionInvestigation()
        {
            List<AdmissionInvestigation> admissionInvestigations = new List<AdmissionInvestigation>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.GetFromJsonAsync<List<AdmissionInvestigation>>("GetAdmissionInvestigation");
                    postObj.Wait();
                    admissionInvestigations = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return admissionInvestigations;
        }

        public IActionResult DeleteAdmissionInvestigation()
        {
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

        public IActionResult AddNewConsultant()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    _AdmissionDto.AdmissionConsultants.AdmissionId = _AdmissionDto.AdmissionsId;
                    _AdmissionDto.AdmissionConsultants.Amount = _AdmissionDto.AdmissionConsultants.Qty * _AdmissionDto.AdmissionConsultants.Price;
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.PostAsJsonAsync<AdmissionConsultant>("CreateAdmissionConsultant", _AdmissionDto.AdmissionConsultants);
                    postObj.Wait();
                    var res = postObj.Result;
                    var result = res.Content.ReadFromJsonAsync<AdmissionConsultant>().Result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        private List<AdmissionConsultant> GetAdmissionConsultant()
        {
            List<AdmissionConsultant> admissionConsultant = new List<AdmissionConsultant>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.GetFromJsonAsync<List<AdmissionConsultant>>("GetAdmissionConsultant");
                    postObj.Wait();
                    admissionConsultant = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return admissionConsultant;
        }

        public IActionResult DeleteAdmissionConsultant()
        {
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
        public List<Item> SearchItems()
        {
            List<Item> items = new List<Item>();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Items/");
                    var postObj = httpClient.GetFromJsonAsync<List<Item>>("GetAllInvestigation");
                    postObj.Wait();
                    var result = postObj.Result;

                    items = result;

                }
                catch (Exception ex) { }
            }
            return items;
        }

        public IActionResult CreateAdmissionItems()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    _AdmissionDto.AdmissionItem.AdmissionId = _AdmissionDto.AdmissionsId;
                    _AdmissionDto.AdmissionItem.Amount = _AdmissionDto.AdmissionItem.Qty * _AdmissionDto.AdmissionItem.Price;
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.PostAsJsonAsync<AdmissionItems>("CreateAdmissionItems", _AdmissionDto.AdmissionItem);
                    postObj.Wait();
                    var res = postObj.Result;
                    var result = res.Content.ReadFromJsonAsync<AdmissionItems>().Result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        private List<AdmissionItems> GetAdmissionItems()
        {
            List<AdmissionItems> admissionItems = new List<AdmissionItems>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Admission/");
                    var postObj = httpClient.GetFromJsonAsync<List<AdmissionItems>>("GetAdmissionItems");
                    postObj.Wait();
                    admissionItems = postObj.Result;

                }
                catch (Exception ex) { }
            }
            return admissionItems;
        }

        public IActionResult DeleteAdmissionItems()
        {
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
    }
}
