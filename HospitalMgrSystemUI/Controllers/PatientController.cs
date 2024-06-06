using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.Patients;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class PatientController : Controller
    {
        [BindProperty]
        public Patient myPatient { get; set; }
        [BindProperty]
        public string SearchValue { get; set; }
        private IConfiguration _configuration;

        public PatientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            List<PatientDto> patients = new List<PatientDto>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                   var result = new PatientService().GetAllPatientByStatus();
                    //string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    //httpClient.BaseAddress = new Uri(APIUrl + "Patient/");
                    //var postObj = httpClient.GetFromJsonAsync<List<Patient>>("GetAllPatients");
                    //postObj.Wait();
                    //var result = postObj.Result;

                    foreach (var item in result)
                    {
                        patients.Add(new PatientDto()
                        {
                            Id = item.Id,
                            Address = item.Address ?? "",
                            FullName = item.FullName ?? "",
                            Age = item.Age,
                            TelephoneNumber = item.TelephoneNumber ?? "",
                            MobileNumber = item.MobileNumber ?? "",
                            NIC = item.NIC ?? "",
                            Sex = (HospitalMgrSystem.Model.Enums.SexStatus)item.Sex
                        });
                    }
                }
                catch (Exception ex) { }
            }
            return View(patients);
        }

        public ActionResult CreatePatient(int id)
        {
            if (id > 0)
            {
                using (var httpClient = new HttpClient())
                {

                    try
                    {
                        string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                        myPatient.Id = id;
                        myPatient.CreateDate = DateTime.Now;
                        myPatient.ModifiedDate = DateTime.Now;
                        httpClient.BaseAddress = new Uri(APIUrl + "Patient/");
                        var postObj = httpClient.GetFromJsonAsync<Patient>("GetAllPatientsByID?id=" + id);
                        postObj.Wait();
                        var res = postObj.Result;

                        //var result = res.Content.ReadFromJsonAsync<User>().Result;
                        return PartialView("_PartialAddPatient", res);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }else
                return PartialView("_PartialAddPatient");



        }

        public IActionResult CreateNewPatient() 
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    //string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    myPatient.CreateDate = DateTime.Now;
                    myPatient.ModifiedDate = DateTime.Now;
                    new PatientService().CreatePatient(myPatient);
                    //httpClient.BaseAddress = new Uri(APIUrl + "Patient/");
                    //var postObj = httpClient.PostAsJsonAsync<Patient>("CreatePatient", myPatient);
                    //postObj.Wait();
                    //var res = postObj.Result;
                    //var result = res.Content.ReadFromJsonAsync<User>().Result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public IActionResult DeletePatient()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    myPatient.ModifiedDate = DateTime.Now;
                    new PatientService().DeletePatient(myPatient);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public IActionResult Search(string txtSearch)
        {
            List<PatientDto> patients = new List<PatientDto>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "Patient/");
                    var postObj = httpClient.GetFromJsonAsync<List<Patient>>("SearchPatient?text=" + txtSearch);
                    postObj.Wait();
                    var result = postObj.Result;

                    foreach (var item in result)
                    {
                        patients.Add(new PatientDto()
                        {
                            Id = item.Id,
                            Address = item.Address,
                            FullName = item.FullName,
                            Age = item.Age,
                            TelephoneNumber = item.TelephoneNumber,
                            MobileNumber = item.MobileNumber,
                            NIC = item.NIC,
                            Sex = (HospitalMgrSystem.Model.Enums.SexStatus)item.Sex
                        });
                    }
                }
                catch (Exception ex) { }
            }
            return View("Index",patients);
        }
    }
}
