using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.CashierSession;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Default;
using HospitalMgrSystem.Service.Drugs;
using HospitalMgrSystem.Service.NightShiftSession;
using HospitalMgrSystem.Service.OPD;
using HospitalMgrSystem.Service.OPDSchedule;
using HospitalMgrSystem.Service.Patients;
using HospitalMgrSystem.Service.User;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class XRAYController : Controller
    {
        [BindProperty] public Patient myPatient { get; set; }

        [BindProperty] public OPDDto _OPDDto { get; set; }

        [BindProperty] public OPD myOPD { get; set; }
        [BindProperty] public string SearchValue { get; set; }

        [BindProperty] public int opdId { get; set; }



        #region OPD Management

        public IActionResult Index(int isPop)
        {
            OPDDto oPDDto = new OPDDto();
            oPDDto.StartTime = DateTime.Now;
            oPDDto.EndTime = DateTime.Now;
            oPDDto.sessionType = -1;
            oPDDto.paidStatus = -1;
            oPDDto.isPoP = isPop;
            oPDDto.patientsList = LoadPatients();
            oPDDto.consultantList = LoadConsultants();
            oPDDto.listOPDTbDto = LoadOPD();
            oPDDto.isNightShift = new DefaultService().GetDefailtShiftStatus();
            return View(oPDDto);
        }

        public IActionResult filterForm()
        {
            // If session type is not -1
            if (_OPDDto.paidStatus != -1)
            {
                try
                {
                    OPDDto oPDDto = new OPDDto();
                    List<OPDTbDto> oPDTbDto = new List<OPDTbDto>();
                    var result = new OPDService().GetAllOPDByAndDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime,
                        (PaymentStatus)_OPDDto.paidStatus);

                    foreach (var item in result)
                    {
                        oPDTbDto.Add(new OPDTbDto()
                        {
                            Id = item.Id,
                            roomName = item.room.Name,
                            consaltantName = item.consultant.Name,
                            FullName = item.patient.FullName,
                            MobileNumber = item.patient.MobileNumber,
                            DateTime = item.DateTime,
                            Sex = (HospitalMgrSystem.Model.Enums.SexStatus)item.patient.Sex,
                            Status = item.Status,
                            TotalAmount = item.TotalAmount,
                            paymentStatus = item.paymentStatus
                        });
                    }

                    oPDDto.listOPDTbDto = oPDTbDto;
                    return View("Index", oPDDto);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }

            // Get all OPD by date range
            try
            {
                OPDDto oPDDto = new OPDDto();
                List<OPDTbDto> oPDTbDto = new List<OPDTbDto>();
                var result = new OPDService().GetAllOPDByDateRange(_OPDDto.StartTime, _OPDDto.EndTime);
                foreach (var item in result)
                {
                    oPDTbDto.Add(new OPDTbDto()
                    {
                        Id = item.Id,
                        roomName = item.room.Name,
                        consaltantName = item.consultant.Name,
                        FullName = item.patient.FullName,
                        MobileNumber = item.patient.MobileNumber,
                        DateTime = item.DateTime,
                        Sex = (HospitalMgrSystem.Model.Enums.SexStatus)item.patient.Sex,
                        Status = item.Status
                    });
                }

                oPDDto.listOPDTbDto = oPDTbDto;
                return View("Index", oPDDto);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult CreateOPDReg(int Id)
        {
            OPDDto oPDDto = new OPDDto();
            oPDDto.consultantList = LoadActiveConsultants();
            oPDDto.patientsList = LoadPatients();
            oPDDto.Drugs = DrugsSearch();
            decimal consaltantFee =0;
            decimal hospitalFee = 0;
            OPD opdObject = new OPD();
            opdObject.ConsultantFee = consaltantFee;
            opdObject.HospitalFee = hospitalFee;

            if (Id > 0)
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        oPDDto.opd = new OPDService().GetAllOPDByID(Id);
                        oPDDto.opd.HospitalFee = 0;
                        oPDDto.opd.HospitalFee = 0;
                        oPDDto.OPDDrugusList = GetOPDDrugus(Id);
                        oPDDto.opdId = Id;
                        return PartialView("_PartialAddOPDRegistration", oPDDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            else
            {
                oPDDto.opd = opdObject;
                return PartialView("_PartialAddOPDRegistration", oPDDto);
            }
        }

        //Create user modify user details should be include
        public IActionResult AddNewOPD([FromBody] OPDDto oPDDto)
        {
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            var NightShiftSessionList = new List<NightShiftSession>();
            NightShiftSessionList = GetActiveShiftSession();

            try
            {
                Patient patient = new Patient();

                oPDDto.patient.CreateUser = Convert.ToInt32(userIdCookie);
                oPDDto.patient.ModifiedUser = Convert.ToInt32(userIdCookie);
                patient = CreatePatient(oPDDto.patient);
                if (patient != null)
                {
                    decimal hospitalFee = new DefaultService().GetDefailtHospitalPrice();
                    OPD OPDobj = new OPD();
                    oPDDto.opd.PatientID = patient.Id;
                    oPDDto.opd.DateTime = DateTime.Now;
                    oPDDto.opd.RoomID = 1;
                    oPDDto.opd.ModifiedUser = Convert.ToInt32(userIdCookie);
                    oPDDto.opd.CreatedUser = Convert.ToInt32(userIdCookie);
                    oPDDto.opd.AppoimentNo = 0;
                    oPDDto.opd.CreateDate = DateTime.Now;
                    oPDDto.opd.ModifiedDate = DateTime.Now;
                    oPDDto.opd.HospitalFee =  0;
                    oPDDto.opd.Description = "X-RAY";
                    oPDDto.opd.paymentStatus = PaymentStatus.NOT_PAID;
                    oPDDto.opd.invoiceType = InvoiceType.OPD;
                    oPDDto.opd.ConsultantFee = 0;
                    oPDDto.opd.shiftID = NightShiftSessionList[0].Id;

                    if (oPDDto.opd.Id > 0)
                    {
                        OPDobj = new OPDService().UpdateOPDStatus(oPDDto.opd, oPDDto.OPDDrugusList);
                    }
                    else
                    {

                        OPDobj = new OPDService().CreateOPD(oPDDto.opd);
                    }

                    if (OPDobj != null)
                    {
                        foreach (var drugusItem in oPDDto.OPDDrugusList)
                        {
                            drugusItem.opdId = OPDobj.Id;
                            drugusItem.itemInvoiceStatus = ItemInvoiceStatus.Add;
                            drugusItem.Amount = drugusItem.Qty * drugusItem.Price;
                            drugusItem.IsRefund = 0;
                            new OPDService().CreateOPDDrugus(drugusItem);
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult AddNewOPDWithQR([FromBody] OPDDto oPDDto)
        {
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];

            try
            {
                Patient patient = new Patient();

                oPDDto.patient.CreateUser = Convert.ToInt32(userIdCookie);
                oPDDto.patient.ModifiedUser = Convert.ToInt32(userIdCookie);
                patient = CreatePatient(oPDDto.patient);
                OPD OPDobj = new OPD();

                string name = patient.FullName;
                var age = patient.Age;
                var phone = patient.MobileNumber;
                var sex = patient.Sex;

                OPDDto oPDDtoId = new OPDDto();

                if (patient != null)
                {
                    decimal hospitalFee = new DefaultService().GetDefailtHospitalPrice();
                    oPDDto.opd.PatientID = patient.Id;
                    oPDDto.opd.DateTime = DateTime.Now;
                    oPDDto.opd.RoomID = 1;
                    oPDDto.opd.ModifiedUser = Convert.ToInt32(userIdCookie);
                    oPDDto.opd.CreatedUser = Convert.ToInt32(userIdCookie);
                    oPDDto.opd.AppoimentNo = 0;
                    oPDDto.opd.CreateDate = DateTime.Now;
                    oPDDto.opd.ModifiedDate = DateTime.Now;
                    oPDDto.opd.HospitalFee = 0;
                    oPDDto.opd.Description = "X-RAY";
                    oPDDto.opd.paymentStatus = PaymentStatus.NOT_PAID;
                    oPDDto.opd.invoiceType = InvoiceType.OPD;
                    oPDDto.opd.ConsultantFee = 0;


                    if (oPDDto.opd.Id > 0)
                    {
                        OPDobj = new OPDService().UpdateOPDStatus(oPDDto.opd, oPDDto.OPDDrugusList);
                    }
                    else
                    {

                        OPDobj = new OPDService().CreateOPD(oPDDto.opd);
                    }

                    if (OPDobj != null)
                    {
                        foreach (var drugusItem in oPDDto.OPDDrugusList)
                        {
                            drugusItem.opdId = OPDobj.Id;
                            drugusItem.itemInvoiceStatus = ItemInvoiceStatus.Add;
                            drugusItem.Amount = drugusItem.Qty * drugusItem.Price;
                            drugusItem.IsRefund = 0;
                            new OPDService().CreateOPDDrugus(drugusItem);
                        }

                        OPD opdDataForQr = new OPDService().GetAllOPDByID(OPDobj.Id);

                        _OPDDto.opdId = OPDobj.Id;
                        _OPDDto.name = opdDataForQr.patient?.FullName;
                        _OPDDto.age = opdDataForQr.patient.Age;
                        _OPDDto.months = opdDataForQr.patient.Months;
                        _OPDDto.days = opdDataForQr.patient.Days;
                        _OPDDto.sex = opdDataForQr.patient.Sex;
                        _OPDDto.phone = opdDataForQr.patient.MobileNumber;
                        _OPDDto.TotalAmount = opdDataForQr.TotalAmount;
                        _OPDDto.CreatedUserName = new UserService().GetUserByID(opdDataForQr.CreatedUser).FullName;
                    }
                }

                return PartialView("_PartialQR", _OPDDto);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        //Create user modify user details should be include
        public IActionResult DeleteOPD(int Id)
        {
            try
            {
                new OPDService().DeleteOPD(Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult searchPateint(string SearchValue)
        {
            OPDDto oPDDto = new OPDDto();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    oPDDto.patientsList = new PatientService().SearchPatient(SearchValue);
                }
                catch (Exception ex)
                {
                }
            }

            return View("Index", oPDDto);
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
                catch (Exception ex)
                {
                }
            }

            return consultants;
        }

        private List<Consultant> LoadActiveConsultants()
        {
            List<OPDScheduler> opdSchedulers = new List<OPDScheduler>();
            List<Consultant> consultants = new List<Consultant>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    opdSchedulers = new OPDSchedulerService().GetActiveOPDSchedulers();
                    if (opdSchedulers != null)
                    {
                        foreach (var opdScheduler in opdSchedulers)
                        {
                            // Check if the consultant is not already in the list
                            if (!consultants.Any(c => c.Id == opdScheduler.ConsultantId))
                            {
                                if (opdScheduler.Consultant != null)
                                {
                                    consultants.Add(opdScheduler.Consultant);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            return consultants;
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
                catch (Exception ex)
                {
                }
            }

            return patients;
        }

        private List<OPDTbDto> LoadOPD()
        {
            List<OPD> opd = new List<OPD>();
            List<OPDTbDto> oPDTbDto = new List<OPDTbDto>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    opd = new OPDService().GetAllOPDByDescription("X-RAY");
                    var result = opd;

                    foreach (var item in result)
                    {
                        oPDTbDto.Add(new OPDTbDto()
                        {
                            Id = item.Id,
                            roomName = item.room.Name,
                            consaltantName = item.consultant.Name,
                            FullName = item.patient.FullName,
                            MobileNumber = item.patient.MobileNumber,
                            DateTime = item.DateTime,
                            Sex = (SexStatus)item.patient.Sex,
                            Status = item.Status,
                            paymentStatus = item.paymentStatus
                        });
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return oPDTbDto;
        }


        public Patient CreatePatient(Patient patientObj)
        {
            try
            {
                patientObj.CreateDate = DateTime.Now;
                patientObj.ModifiedDate = DateTime.Now;
                if (patientObj != null)
                {
                    PatientService patientService = new PatientService();
                    Patient resPatient = patientService.CreatePatient(patientObj);
                    return resPatient;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
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
                catch (Exception ex)
                {
                }
            }

            return patient;
        }

        #endregion

        #region Drugs  Management

        public List<Drug> DrugsSearch()
        {
            List<Drug> drugs = new List<Drug>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    drugs = new DrugsService().GetAllDrugsByStatus();
                }
                catch (Exception ex)
                {
                }
            }

            return drugs;
        }

        private List<OPDDrugus> GetOPDDrugus(int id)
        {
            List<OPDDrugus> oPDDrugus = new List<OPDDrugus>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    oPDDrugus = new OPDService().GetOPDDrugus(id);
                }
                catch (Exception ex)
                {
                }
            }

            return oPDDrugus;
        }

        public IActionResult DeleteOPDDrug(int Id)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    new OPDService().DeleteOPDDrugus(Id);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        #endregion


        [HttpGet]
        [Route("/XRAY/LoadPatientsAjax/{value}")]
        public IActionResult LoadPatientsAjax(int value)
        {
            Patient patient = LoadPatientByID(value);
            return Json(patient);
        }

        private IActionResult print()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    new OPDService().printme("1");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult OpenQR(int Id)
        {
            OPDDto opdDto = new OPDDto();
            opdDto.opd = new OPDService().GetAllOPDByID(Id);
            opdDto.opdId = Id;

            string name = opdDto.opd.patient.FullName;
            var age = opdDto.opd.patient.Age;
            var months = opdDto.opd.patient.Months;
            var days = opdDto.opd.patient.Days;
            var phone = opdDto.opd.patient.MobileNumber;
            var sex = opdDto.opd.patient.Sex;
            var totalAmount = opdDto.opd.TotalAmount;
            var createdUser = new UserService().GetUserByID(opdDto.opd.ModifiedUser).FullName;

            _OPDDto.opdId = Id;
            _OPDDto.name = name;
            _OPDDto.age = age;
            _OPDDto.months = months;
            _OPDDto.days = days;
            _OPDDto.sex = sex;
            _OPDDto.phone = phone;
            _OPDDto.TotalAmount = totalAmount;
            _OPDDto.CreatedUserName = createdUser;

            return PartialView("_PartialQR", _OPDDto);
        }


        private List<NightShiftSession> GetActiveShiftSession()
        {
            List<NightShiftSession> NightShiftSessionList = new List<NightShiftSession>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    NightShiftSessionList = new NightShiftSessionService().GetACtiveNtShiftSessions();
                }
                catch (Exception ex)
                {
                }
            }

            return NightShiftSessionList;
        }

        private List<NightShiftSession> GetACtiveNtShiftSessionsByUserID(int id, Shift shift)
        {
            List<NightShiftSession> NightShiftSessionList = new List<NightShiftSession>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    NightShiftSessionList = new NightShiftSessionService().GetACtiveNtShiftSessionsByUserID(id, shift);
                }
                catch (Exception ex)
                {
                }
            }

            return NightShiftSessionList;
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
                catch (Exception ex)
                {
                }
            }

            return CashierSessionList;
        }


        private List<CashierSession> GetAllNightsiftActiveCashierSession()
        {
            List<CashierSession> CashierSessionList = new List<CashierSession>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    CashierSessionList = new CashierSessionService().GetAllNightsiftActiveCashierSession();
                }
                catch (Exception ex)
                {
                }
            }

            return CashierSessionList;
        }
    }
}
