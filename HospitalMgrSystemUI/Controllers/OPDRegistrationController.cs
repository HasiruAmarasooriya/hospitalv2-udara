using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.Consultant;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System;
using HospitalMgrSystem.Service.Patients;
using HospitalMgrSystemUI.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using HospitalMgrSystem.Service.OPD;
using HospitalMgrSystem.Service.Drugs;
using HospitalMgrSystem.Service.Investigation;
using HospitalMgrSystem.Service.Item;
using HospitalMgrSystem.Service.Default;
using HospitalMgrSystem.Service.OPDSchedule;
using System.Security.Cryptography;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.CashierSession;
using HospitalMgrSystem.Service.NightShiftSession;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalMgrSystem.Service.User;

namespace HospitalMgrSystemUI.Controllers
{
    public class OPDRegistrationController : Controller
    {
        [BindProperty] public Patient myPatient { get; set; }

        [BindProperty] public OPDDto _OPDDto { get; set; }

        [BindProperty] public OPD myOPD { get; set; }
        [BindProperty] public string SearchValue { get; set; }

        [BindProperty] public int opdId { get; set; }

        #region Shift Management

        public IActionResult ChangeShift()
        {
            List<NightShiftSession> NightShiftSessionList = new List<NightShiftSession>();
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            int userId = Convert.ToInt32(userIdCookie);
            NightShiftSessionList = GetActiveShiftSession();


            if (NightShiftSessionList.Count > 0)
            {
                //if opd user already has OPD session (Day or night)
                foreach (NightShiftSession item in NightShiftSessionList)
                {
                    item.shiftSessionStatus = ShiftSessionStatus.END;
                    item.EndTime = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.ModifiedUser = userId;
                    //mark as current shit over
                    new NightShiftSessionService().CreateNightShiftSession(item);

                    if (item.userID == userId)
                    {
                        // then create new session
                        if (item.shift == Shift.NIGHT_SHIFT)
                        {
                            // if previous shift equal to night shift, new shift, mark as Day shift
                            NightShiftSession nightShiftSession = new NightShiftSession();

                            nightShiftSession.userID = userId;
                            nightShiftSession.StartingTime = DateTime.Now;
                            nightShiftSession.EndTime = DateTime.Now;
                            nightShiftSession.shiftSessionStatus = ShiftSessionStatus.START;
                            nightShiftSession.shift = Shift.DAY_SHIFT;
                            nightShiftSession.Status = CommonStatus.Active;
                            nightShiftSession.CreateDate = DateTime.Now;
                            nightShiftSession.CreateUser = userId;
                            nightShiftSession.ModifiedDate = DateTime.Now;
                            nightShiftSession.ModifiedUser = userId;

                            //create day shift
                            new NightShiftSessionService().CreateNightShiftSession(nightShiftSession);

                            CashierSession newCashierSession = new CashierSession();
                            List<CashierSession> mtList = new List<CashierSession>();

                            //previous all night shift  cashier sessions update as end
                            mtList = GetAllNightsiftActiveCashierSession();
                            if (mtList.Count > 0)
                            {
                                foreach (CashierSession cashierSessionItem in mtList)
                                {
                                    newCashierSession.Id = cashierSessionItem.Id;
                                    newCashierSession.EndBalence = 0;
                                    newCashierSession.EndTime = DateTime.Now;
                                    newCashierSession.cashierSessionStatus = CashierSessionStatus.END;
                                    newCashierSession.ModifiedUser = Convert.ToInt32(userIdCookie);
                                    newCashierSession.ModifiedDate = DateTime.Now;
                                    newCashierSession =
                                        new CashierSessionService().CreateCashierSession(newCashierSession);
                                }
                            }
                        }
                        else
                        {
                            NightShiftSession nightShiftSession = new NightShiftSession();

                            nightShiftSession.userID = userId;
                            nightShiftSession.StartingTime = DateTime.Now;
                            nightShiftSession.EndTime = DateTime.Now;
                            nightShiftSession.shiftSessionStatus = ShiftSessionStatus.START;
                            nightShiftSession.shift = Shift.NIGHT_SHIFT;
                            nightShiftSession.Status = CommonStatus.Active;
                            nightShiftSession.CreateDate = DateTime.Now;
                            nightShiftSession.CreateUser = userId;
                            nightShiftSession.ModifiedDate = DateTime.Now;
                            nightShiftSession.ModifiedUser = userId;

                            new NightShiftSessionService().CreateNightShiftSession(nightShiftSession);

                            CashierSession newCashierSession = new CashierSession();
                            List<CashierSession> mtList = new List<CashierSession>();
                            mtList = GetActiveCashierSession(Convert.ToInt32(userIdCookie));
                            if (mtList.Count == 0)
                            {
                                newCashierSession.userID = Convert.ToInt32(userIdCookie);
                                newCashierSession.StartingTime = DateTime.Now;
                                newCashierSession.StartBalence = 0;
                                newCashierSession.EndBalence = 0;
                                newCashierSession.EndTime = DateTime.Now;
                                newCashierSession.CreateUser = Convert.ToInt32(userIdCookie);
                                newCashierSession.CreateDate = DateTime.Now;
                                newCashierSession.cashierSessionStatus = CashierSessionStatus.START;
                                newCashierSession.ModifiedUser = Convert.ToInt32(userIdCookie);
                                newCashierSession.ModifiedDate = DateTime.Now;
                                newCashierSession.UserRole = UserRole.OPDNURSE;
                                newCashierSession = new CashierSessionService().CreateCashierSession(newCashierSession);
                            }
                        }
                    }
                }
            }
            //else
            //{
            //    //if opd user 0  OPD session (Day or night)
            //    bool session = new NightShiftSessionService().checkIsNightShift();
            //    if (session)
            //    {

            //        NightShiftSession nightShiftSession = new NightShiftSession();

            //        nightShiftSession.userID = userId;
            //        nightShiftSession.StartingTime = DateTime.Now;
            //        nightShiftSession.EndTime = DateTime.Now;
            //        nightShiftSession.shiftSessionStatus = ShiftSessionStatus.START;
            //        nightShiftSession.shift = Shift.DAY_SHIFT;
            //        nightShiftSession.Status = CommonStatus.Active;
            //        nightShiftSession.CreateDate = DateTime.Now;
            //        nightShiftSession.CreateUser = userId;
            //        nightShiftSession.ModifiedDate = DateTime.Now;
            //        nightShiftSession.ModifiedUser = userId;

            //        new NightShiftSessionService().CreateNightShiftSession(nightShiftSession);

            //        CashierSession newCashierSession = new CashierSession();
            //        List<CashierSession> mtList = new List<CashierSession>();
            //        mtList = GetActiveCashierSession(Convert.ToInt32(userIdCookie));
            //        if (mtList.Count > 0)
            //        {
            //            newCashierSession.Id = mtList[0].Id;
            //            newCashierSession.userID = Convert.ToInt32(userIdCookie);
            //            newCashierSession.StartingTime = DateTime.Now;
            //            newCashierSession.StartBalence = 0;
            //            newCashierSession.EndBalence = 0;
            //            newCashierSession.EndTime = DateTime.Now;
            //            newCashierSession.cashierSessionStatus = CashierSessionStatus.END;
            //            newCashierSession.ModifiedUser = Convert.ToInt32(userIdCookie);
            //            newCashierSession.ModifiedDate = DateTime.Now;

            //            newCashierSession = new CashierSessionService().CreateCashierSession(newCashierSession);

            //        }
            //    }
            //    else
            //    {

            //        NightShiftSession nightShiftSession = new NightShiftSession();

            //        nightShiftSession.userID = userId;
            //        nightShiftSession.StartingTime = DateTime.Now;
            //        nightShiftSession.EndTime = DateTime.Now;
            //        nightShiftSession.shiftSessionStatus = ShiftSessionStatus.START;
            //        nightShiftSession.shift = Shift.NIGHT_SHIFT;
            //        nightShiftSession.Status = CommonStatus.Active;
            //        nightShiftSession.CreateDate = DateTime.Now;
            //        nightShiftSession.CreateUser = userId;
            //        nightShiftSession.ModifiedDate = DateTime.Now;
            //        nightShiftSession.ModifiedUser = userId;

            //        new NightShiftSessionService().CreateNightShiftSession(nightShiftSession);

            //        CashierSession newCashierSession = new CashierSession();
            //        List<CashierSession> mtList = new List<CashierSession>();
            //        mtList = GetActiveCashierSession(Convert.ToInt32(userIdCookie));
            //        if (mtList.Count == 0)
            //        {

            //            newCashierSession.userID = Convert.ToInt32(userIdCookie);
            //            newCashierSession.StartingTime = DateTime.Now;
            //            newCashierSession.StartBalence = 0;
            //            newCashierSession.EndBalence = 0;
            //            newCashierSession.EndTime = DateTime.Now;
            //            newCashierSession.CreateUser = Convert.ToInt32(userIdCookie);
            //            newCashierSession.CreateDate = DateTime.Now;
            //            newCashierSession.cashierSessionStatus = CashierSessionStatus.START;
            //            newCashierSession.ModifiedUser = Convert.ToInt32(userIdCookie);
            //            newCashierSession.ModifiedDate = DateTime.Now;
            //            newCashierSession.UserRole = UserRole.OPDNURSE;
            //            newCashierSession = new CashierSessionService().CreateCashierSession(newCashierSession);

            //        }
            //    }

            //}

            //change shift
            OPDService oPDService = new OPDService();
            oPDService.UpdateShift();


            return RedirectToAction("Index");
        }

        public void CreateFirstShift()
        {
            List<NightShiftSession> NightShiftSessionList = new List<NightShiftSession>();
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            int userId = Convert.ToInt32(userIdCookie);
            bool session = new NightShiftSessionService().checkIsNightShift();
            if (session)
            {
                NightShiftSessionList = GetACtiveNtShiftSessionsByUserID(userId, Shift.NIGHT_SHIFT);
            }
            else
            {
                NightShiftSessionList = GetACtiveNtShiftSessionsByUserID(userId, Shift.DAY_SHIFT);
            }


            if (NightShiftSessionList.Count == 0)
            {
                if (!session)
                {
                    // if Day shift 
                    NightShiftSession nightShiftSession = new NightShiftSession();

                    nightShiftSession.userID = userId;
                    nightShiftSession.StartingTime = DateTime.Now;
                    nightShiftSession.EndTime = DateTime.Now;
                    nightShiftSession.shiftSessionStatus = ShiftSessionStatus.START;
                    nightShiftSession.shift = Shift.DAY_SHIFT;
                    nightShiftSession.Status = CommonStatus.Active;
                    nightShiftSession.CreateDate = DateTime.Now;
                    nightShiftSession.CreateUser = userId;
                    nightShiftSession.ModifiedDate = DateTime.Now;
                    nightShiftSession.ModifiedUser = userId;

                    //create day shift
                    new NightShiftSessionService().CreateNightShiftSession(nightShiftSession);
                }
                else
                {
                    NightShiftSession nightShiftSession = new NightShiftSession();

                    nightShiftSession.userID = userId;
                    nightShiftSession.StartingTime = DateTime.Now;
                    nightShiftSession.EndTime = DateTime.Now;
                    nightShiftSession.shiftSessionStatus = ShiftSessionStatus.START;
                    nightShiftSession.shift = Shift.NIGHT_SHIFT;
                    nightShiftSession.Status = CommonStatus.Active;
                    nightShiftSession.CreateDate = DateTime.Now;
                    nightShiftSession.CreateUser = userId;
                    nightShiftSession.ModifiedDate = DateTime.Now;
                    nightShiftSession.ModifiedUser = userId;

                    new NightShiftSessionService().CreateNightShiftSession(nightShiftSession);

                    CashierSession newCashierSession = new CashierSession();
                    List<CashierSession> mtList = new List<CashierSession>();
                    mtList = GetActiveCashierSession(Convert.ToInt32(userIdCookie));
                    if (mtList.Count == 0)
                    {
                        newCashierSession.userID = Convert.ToInt32(userIdCookie);
                        newCashierSession.StartingTime = DateTime.Now;
                        newCashierSession.StartBalence = 0;
                        newCashierSession.EndBalence = 0;
                        newCashierSession.EndTime = DateTime.Now;
                        newCashierSession.CreateUser = Convert.ToInt32(userIdCookie);
                        newCashierSession.CreateDate = DateTime.Now;
                        newCashierSession.cashierSessionStatus = CashierSessionStatus.START;
                        newCashierSession.ModifiedUser = Convert.ToInt32(userIdCookie);
                        newCashierSession.ModifiedDate = DateTime.Now;
                        newCashierSession.UserRole = UserRole.OPDNURSE;
                        newCashierSession = new CashierSessionService().CreateCashierSession(newCashierSession);
                    }
                }
            }
        }

        #endregion

        #region OPD Management

        public IActionResult Index(int isPop)
        {
            CreateFirstShift();
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
            decimal consaltantFee = new DefaultService().GetDefailtConsaltantPrice();
            decimal hospitalFee = new DefaultService().GetDefailtHospitalPrice();
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
                    oPDDto.opd.HospitalFee = oPDDto.OpdType == 1 ? hospitalFee : 0;
                    oPDDto.opd.Description = oPDDto.OpdType == 1 ? "OPD" : oPDDto.OpdType == 2 ? "X-RAY" : "Other";
                    oPDDto.opd.paymentStatus = PaymentStatus.NOT_PAID;
                    oPDDto.opd.invoiceType = InvoiceType.OPD;
                    oPDDto.opd.ConsultantFee = 0;

                    if (oPDDto.opd.Id > 0)
                    {
                        OPDobj = new OPDService().UpdateOPDStatus(oPDDto.opd, oPDDto.OPDDrugusList);
                    }
                    else
                    {
                        List<NightShiftSession> NightShiftSessionList = new List<NightShiftSession>();
                        NightShiftSessionList = GetActiveShiftSession();
                        if (NightShiftSessionList.Count > 0)
                        {
                            oPDDto.opd.shiftID = NightShiftSessionList[0].Id;
                            if (NightShiftSessionList[0].shift == Shift.NIGHT_SHIFT)
                            {
                                oPDDto.opd.isOnOPD = 1;
                            }
                        }

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
                    oPDDto.opd.HospitalFee = oPDDto.opd.OpdType == 1 ? hospitalFee : 0;
                    oPDDto.opd.Description = oPDDto.OpdType == 1 ? "OPD" : "Other";
                    oPDDto.opd.paymentStatus = PaymentStatus.NOT_PAID;
                    oPDDto.opd.invoiceType = InvoiceType.OPD;
                    oPDDto.opd.ConsultantFee = 0;


                    if (oPDDto.opd.Id > 0)
                    {
                        OPDobj = new OPDService().UpdateOPDStatus(oPDDto.opd, oPDDto.OPDDrugusList);
                    }
                    else
                    {
                        List<NightShiftSession> NightShiftSessionList = new List<NightShiftSession>();
                        NightShiftSessionList = GetActiveShiftSession();
                        if (NightShiftSessionList.Count > 0)
                        {
                            oPDDto.opd.shiftID = NightShiftSessionList[0].Id;
                            if (NightShiftSessionList[0].shift == Shift.NIGHT_SHIFT)
                            {
                                oPDDto.opd.isOnOPD = 1;
                            }
                        }

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
                // return View("Index", _OPDDto);
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
                    opd = new OPDService().GetAllOPDByStatus();
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
        [Route("/OPDRegistration/LoadPatientsAjax/{value}")]
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