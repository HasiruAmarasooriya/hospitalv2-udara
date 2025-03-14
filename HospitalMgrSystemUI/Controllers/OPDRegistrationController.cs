﻿using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using HospitalMgrSystem.Service.Consultant;
using Microsoft.AspNetCore.Mvc;
using HospitalMgrSystem.Service.Patients;
using HospitalMgrSystemUI.Models;
using HospitalMgrSystem.Service.OPD;
using HospitalMgrSystem.Service.Drugs;
using HospitalMgrSystem.Service.Default;
using HospitalMgrSystem.Service.OPDSchedule;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.CashierSession;
using HospitalMgrSystem.Service.NightShiftSession;
using HospitalMgrSystem.Service.User;
using HospitalMgrSystem.Service.Stock;
using Org.BouncyCastle.Asn1.Ocsp;

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
            var oPDDto = new OPDDto
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                sessionType = -1,
                paidStatus = -1,
                isPoP = isPop,
                // patientsList = LoadPatients(),
                consultantList = LoadConsultants(),
                listOPDTbDtoSp = LoadOPD(),
                isNightShift = new DefaultService().GetDefailtShiftStatus()
            };

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
            var defaultService = new DefaultService();

            var oPDDto = new OPDDto
            {
                consultantList = LoadActiveConsultants(),
                // patientsList = LoadPatients(),
                Drugs = DrugsSearch()
            };

            var consaltantFee = defaultService.GetDefailtConsaltantPrice();
            var hospitalFee = defaultService.GetDefailtHospitalPrice();

            var opdObject = new OPD
            {
                ConsultantFee = consaltantFee,
                HospitalFee = hospitalFee
            };

            if (Id > 0)
            {
                using var httpClient = new HttpClient();

                try
                {


                    //int tranType = (int)StoreTranMethod.OPD_IN;
                    //oPDDto.OPDDrugusList = new OPDService().GetOPDDrugus(tranType);
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
                    var hospitalFee = new DefaultService().GetDefailtHospitalPrice();
                    var OPDobj = new OPD();
                    var activeOpdSession = new OPDSchedulerService().GetActiveOPDSchedulers();

                    oPDDto.opd.PatientID = patient.Id;
                    oPDDto.opd.DateTime = DateTime.Now;
                    oPDDto.opd.RoomID = 1;
                    oPDDto.opd.ModifiedUser = Convert.ToInt32(userIdCookie);
                    oPDDto.opd.CreatedUser = Convert.ToInt32(userIdCookie);
                    oPDDto.opd.AppoimentNo = 0;
                    oPDDto.opd.CreateDate = DateTime.Now;
                    oPDDto.opd.ModifiedDate = DateTime.Now;
                    oPDDto.opd.HospitalFee = oPDDto.opd.OpdType == 1 ? hospitalFee : 0;
                    oPDDto.opd.Description = oPDDto.opd.OpdType == 1 ? "OPD" : "Other";
                    oPDDto.opd.paymentStatus = PaymentStatus.NOT_PAID;
                    oPDDto.opd.invoiceType = InvoiceType.OPD;
                    oPDDto.opd.ConsultantFee = 0;
                    oPDDto.opd.schedularId = activeOpdSession[0].Id;
                    
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
                            int DrugID = drugusItem.DrugId;
                            int refnumber = 0;
                            int batchnumber = 0;
                            //int TranType = (int)StoreTranMethod.OPD_IN;
                            var TranLog = new DrugsService().GetDrugDetailsById(DrugID);
                            decimal Qty = drugusItem.Qty;
                            
                            if (TranLog == null)
                            {
                                var stockTran = new stockTransaction
                                {
                                    BillId = OPDobj.Id,
                                    DrugIdRef = drugusItem.DrugId,
                                    Qty = -Qty,
                                    TranType = StoreTranMethod.OPD_Out,
                                    RefNumber = "OPD_" + drugusItem.opdId,
                                    Remark = "OPD_Drug_Issue",
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
                                    BillId = OPDobj.Id,
                                    DrugIdRef = drugusItem.DrugId,
                                    Qty = -Qty,
                                    TranType = StoreTranMethod.OPD_Out,
                                    RefNumber = "OPD_" + drugusItem.opdId,
                                    Remark = "OPD_Drug_Issue",
                                    BatchNumber = TranLog.BatchNumber,
                                    CreateUser = Convert.ToInt32(userIdCookie),
                                    ModifiedUser = Convert.ToInt32(userIdCookie),
                                    CreateDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now
                                };
                                new StockService().LogTransaction(stockTran);


                            }

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
                    var activeOpdSession = new OPDSchedulerService().GetActiveOPDSchedulers();

                    oPDDto.opd.PatientID = patient.Id;
                    oPDDto.opd.DateTime = DateTime.Now;
                    oPDDto.opd.RoomID = 1;
                    oPDDto.opd.ModifiedUser = Convert.ToInt32(userIdCookie);
                    oPDDto.opd.CreatedUser = Convert.ToInt32(userIdCookie);
                    oPDDto.opd.AppoimentNo = 0;
                    oPDDto.opd.CreateDate = DateTime.Now;
                    oPDDto.opd.ModifiedDate = DateTime.Now;
                    oPDDto.opd.HospitalFee = oPDDto.opd.OpdType == 1 ? hospitalFee : 0;
                    oPDDto.opd.Description = oPDDto.opd.OpdType == 1 ? "OPD" : "Other";
                    oPDDto.opd.paymentStatus = PaymentStatus.NOT_PAID;
                    oPDDto.opd.invoiceType = InvoiceType.OPD;
                    oPDDto.opd.ConsultantFee = 0;
                    oPDDto.opd.schedularId = activeOpdSession[0].Id;


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
                            int DrugID = drugusItem.DrugId;
                            int refnumber = 0;
                            int batchnumber = 0;
                            var TranLog = new DrugsService().GetDrugDetailsById(DrugID);
                            decimal Qty = drugusItem.Qty;

                            if (TranLog == null)
                            {
                                var stockTran = new stockTransaction
                                {
                                    BillId = OPDobj.Id,
                                    DrugIdRef = drugusItem.DrugId,
                                    Qty = -Qty,
                                    TranType = StoreTranMethod.OPD_Out,
                                    RefNumber = "OPD_" + drugusItem.opdId,
                                    Remark = "OPD_Drug_Issue",
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
                                    BillId = OPDobj.Id,
                                    DrugIdRef = drugusItem.DrugId,
                                    Qty = -Qty,
                                    TranType = StoreTranMethod.OPD_Out,
                                    RefNumber = "OPD_" + drugusItem.opdId,
                                    Remark = "OPD_Drug_Issue",
                                    BatchNumber = TranLog.BatchNumber,
                                    CreateUser = Convert.ToInt32(userIdCookie),
                                    ModifiedUser = Convert.ToInt32(userIdCookie),
                                    CreateDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now
                                };
                                new StockService().LogTransaction(stockTran);


                            }
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
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            var userId = Convert.ToInt32(userIdCookie);

            try
            {
                new OPDService().DeleteOPD(Id, userId);
                var drugDetailsList = new OPDService().RefundDrugDetailsById(Id);
                StockService stockService = new StockService();
                foreach (var drugDetails in drugDetailsList)
                {
                    var stockTransaction = new stockTransaction
                    {
                        GrpvId = drugDetails.GrpvId,
                        DrugIdRef = drugDetails.DrugIdRef,
                        BillId = Id,
                        Qty = -drugDetails.Qty,
                        TranType = StoreTranMethod.OPD_Refund,
                        RefNumber = $"OPD_Bill_Deleted_{Id}",
                        Remark = "OPD_Drug_Refund",
                        BatchNumber = drugDetails.BatchNumber,
                        CreateUser = Convert.ToInt32(userIdCookie),
                        CreateDate = DateTime.Now,
                        ModifiedUser = Convert.ToInt32(userIdCookie),
                        ModifiedDate = DateTime.Now
                    };
                  
                    stockService.LogTransaction(stockTransaction);
                }
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

        private List<OpdOtherXrayDataTableDto> LoadOPD()
        {

            using (var httpClient = new HttpClient())
            {
                try
                {
                    return new OPDService().GetAllOPDByStatusWithoutXraySP(1, DateTime.Today);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

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
                    //int tranType = (int)StoreTranMethod.OPD_IN;
                    //drugs = new DrugsService().GetOPDDrugus(tranType);
                    drugs = new DrugsService().GetAllDrugsByStatus();
                }
                catch (Exception ex)
                {
                }
            }

            return drugs;
        }

        public JsonResult CheckDrugQuantity(int drugId, decimal enteredQty)
       {
            
            try
            {
                var drug = new DrugsService().GetDrugById(drugId );

                if (drug != null)
                {

                    if (drug.Qty < enteredQty)
                    {
                        return Json(new { success = false, message = $"Not enough stock. Available quantity is {drug.Qty}" });
                    }

                    return Json(new { success = true, message = "Stock is available." });
                }
                else
                {
                    return Json(new { success = false, message = "Drug not found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error occurred while checking drug quantity." });
            }
        }
        [HttpGet]
        public JsonResult GetDrugPriceByID(int drugId)
        {
           
            try
            {
                var drug = new DrugsService().GetDrugById(drugId);

                if (drug != null)
                {
                    return Json(new { success = true, price = drug.Price });
                }
                else
                {
                    return Json(new { success = false, price = 0 });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error occurred while checking drug quantity." });
            }
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
        [HttpPost]
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