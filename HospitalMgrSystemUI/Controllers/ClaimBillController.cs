using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.ClaimBill;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Patients;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class ClaimBillController : Controller
    {
        public IActionResult Index()
        {
            var claimBillDto = new ClaimBillDto
            {
                patientsList = LoadPatients(),
                consultantsList = LoadConsultants(),
                claimBillDtos = new ClaimBillService().GetAllClaimBillsSP()
			};

            return View(claimBillDto);
        }

        public ActionResult AddNewClaimBill([FromBody] ClaimBillDto claimBillDto)
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    claimBillDto.dateTime = DateTime.Now;

                    claimBillDto.claimBill = new ClaimBill
                    {
                        PatientID = claimBillDto.claimBill.PatientID,
                        ConsultantId = claimBillDto.claimBill.ConsultantId,
                        RefNo = claimBillDto.claimBill.RefNo,
                        SubTotal = claimBillDto.claimBill.SubTotal,
                        Discount = claimBillDto.claimBill.Discount,
                        TotalAmount = claimBillDto.claimBill.TotalAmount,
                        CashAmount = claimBillDto.claimBill.CashAmount,
                        Balance = claimBillDto.claimBill.Balance,
                        CreateDate = claimBillDto.dateTime,
                        ModifiedDate = claimBillDto.dateTime
                    };

                    ClaimBillService claimBillService = new ClaimBillService();
                    claimBillDto.claimBill = claimBillService.CreateClaimBill(claimBillDto.claimBill);

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
    }
}
