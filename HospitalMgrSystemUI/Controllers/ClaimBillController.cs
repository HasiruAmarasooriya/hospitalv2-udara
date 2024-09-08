using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.CashierSession;
using HospitalMgrSystem.Service.ClaimBill;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Default;
using HospitalMgrSystem.Service.Drugs;
using HospitalMgrSystem.Service.Patients;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class ClaimBillController : Controller
    {
	    private IConfiguration _configuration;

	    public ClaimBillController(IConfiguration configuration)
	    {
		    _configuration = configuration;
	    }

		public IActionResult Index()
        {
            var claimBillDto = new ClaimBillDto
            {
                patientsList = LoadPatients(),
                consultantsList = LoadConsultants(),
                claimBillDtos = new ClaimBillService().GetAllClaimBillsSP(),
                Scans = new DefaultService().getAllScanItems(),
                Drugs = new DrugsService().GetAllDrugsByStatus()
			};

            return View(claimBillDto);
        }

        public ActionResult AddNewClaimBill([FromBody] ClaimBillDto claimBillDto)
        {
	        var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
	        var userID = Convert.ToInt32(userIdCookie);

			using (var httpClient = new HttpClient())
            {
                try
                {
	                var sessionId = GetActiveCashierSession(userID)[0].Id;

					var patient = new Patient()
	                {
                        FullName = claimBillDto.PatientName,
                        MobileNumber = claimBillDto.ContactNumber,
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        NIC = claimBillDto.NIC,
                        Status = PatientStatus.New
	                };

                    patient = new PatientService().CreatePatient(patient);
	                
	                claimBillDto.dateTime = DateTime.Now;

                    claimBillDto.claimBill = new ClaimBill
                    {
                        PatientID = patient.Id,
                        ConsultantId = claimBillDto.ConsultantId,
                        InvoiceId = null,
                        RefNo = claimBillDto.RefNo,
                        SubTotal = claimBillDto.SubTotal,
                        Discount = claimBillDto.Discount,
                        TotalAmount = claimBillDto.TotalAmount,
                        CashAmount = claimBillDto.Cash,
                        Balance = claimBillDto.Balance,
                        Status = 0,
                        CsId = sessionId,
                        CreateDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };

                    var claimBillService = new ClaimBillService();
                    claimBillDto.claimBill = claimBillService.CreateClaimBill(claimBillDto.claimBill);

                    if (claimBillDto.ClaimBillItemsList == null) return PartialView("_PartialReceipt", claimBillDto);

					foreach (var item in claimBillDto.ClaimBillItemsList!)
                    {
	                    item.ClaimBillId = claimBillDto.claimBill.Id;  // Set ClaimBillId
	                    item.RefId = claimBillDto.claimBill.RefNo; // Set RefId manually or dynamically as needed

	                    // You can also set CreateDate, CreateUser, ModifiedDate, ModifiedUser here
	                    item.CreateDate = DateTime.Now;
	                    item.CreateUser = userID; // Assuming userID is the ID of the user creating the claim
	                    item.ModifiedDate = DateTime.Now;
	                    item.ModifiedUser = userID; // Assuming userID is the ID of the user modifying the claim
                    }

                    var scanItems = claimBillService.CreateClaimBillItemsList(claimBillDto.ClaimBillItemsList);

                    
                    claimBillDto.ClaimBillItemsList = claimBillService.GetChannelingItemsNames(scanItems);

					return PartialView("_PartialReceipt", claimBillDto);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpGet]
        [Route("/ClaimBill/LoadPatientsAjax/{value}")]
        public IActionResult LoadPatientsAjax(int value)
        {
            Patient patient = LoadPatientByID(value);
            return Json(patient);
        }

        private List<Patient> LoadPatients()
        {
            List<Patient> patients = new List<Patient>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    patients = new PatientService().GetAllPatientByStatus();

                }
                catch (Exception ex) { }
            }
            return patients;
        }

        private Patient LoadPatientByID(int id)
        {
            Patient patient = new Patient();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    patient = new PatientService().GetAllPatientByID(id);

                }
                catch (Exception ex) { }
            }
            return patient;
        }

        private List<Consultant> LoadConsultants()
        {
            List<Consultant> consultants = new List<Consultant>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    consultants = new ConsultantService().GetAllConsultantByStatus();

                }
                catch (Exception ex) { }
            }
            return consultants;
        }

        private List<CashierSession> GetActiveCashierSession(int id)
        {
	        List<CashierSession> CashierSessionList = new List<CashierSession>();

	        using (var httpClient = new HttpClient())
	        {
		        try
		        {
			        CashierSessionList = new CashierSessionService().GetACtiveCashierSessions(id);

		        }
		        catch (Exception ex) { }
	        }
	        return CashierSessionList;
        }
	}
}
