using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.Channeling;
using HospitalMgrSystem.Service.ChannelingSchedule;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Default;
using HospitalMgrSystem.Service.Drugs;
using HospitalMgrSystem.Service.OPD;
using HospitalMgrSystem.Service.Patients;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HospitalMgrSystemUI.Controllers
{

    public class ChannelingController : Controller
    {

        [BindProperty]
        public ChannelingDto channelingDto { get; set; }

        [BindProperty]
        public Channeling myChannelling { get; set; }
        public IActionResult Index()
        {
            OPDDto oPDDto = new OPDDto();
            oPDDto.listOPDTbDto = GetAllChannelingByStatus();
            return View(oPDDto);
        }

        //public ActionResult onChangePatient(string id)
        //{
        //    ChannelingDto channelingDto = new ChannelingDto();
        //    channelingDto.listConsultants = LoadConsultant();

        //    return PartialView("_PartialAddChanneling", channelingDto);
        //}
        public ActionResult CreateChanneling(int Id)
        {
            OPDDto oPDChannelingDto = new OPDDto();
            oPDChannelingDto.listConsultants = LoadConsultant();
            oPDChannelingDto.PatientList = LoadPatient();
            oPDChannelingDto.listChannelingSchedule = LoadChannelingShedule();
            oPDChannelingDto.Drugs = DrugsSearch();

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

                        oPDChannelingDto.opd = LoadChannelingByID(Id);
                        oPDChannelingDto.OPDDrugusList = GetChannelingDrugus(Id);
                        oPDChannelingDto.opdId = Id;
                        return PartialView("_PartialAddChanneling", oPDChannelingDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            oPDChannelingDto.opd = opdObject;
            return PartialView("_PartialAddChanneling", oPDChannelingDto);
        }

        private List<OPDDrugus> GetChannelingDrugus(int id)
        {
            List<OPDDrugus> oPDDrugus = new List<OPDDrugus>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    oPDDrugus = new OPDService().GetOPDDrugus(id);

                }
                catch (Exception ex) { }
            }
            return oPDDrugus;
        }

        public List<Drug> DrugsSearch()
        {
            List<Drug> drugs = new List<Drug>();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    drugs = new DrugsService().GetAllDrugsByStatus();

                }
                catch (Exception ex) { }
            }
            return drugs;
        }

        private List<Consultant> LoadConsultant()
        {
            try
            {
                List<Consultant> ConsultantLists = new ConsultantService().GetAllConsultantByStatus();
                return ConsultantLists;
            }
            catch (System.Exception ex)
            { }
            return null;
        }

        private List<Patient> LoadPatient()
        {
            try
            {
                List<Patient> PatientLists = new PatientService().GetAllPatientByStatus();
                return PatientLists;
            }
            catch (System.Exception ex)
            { }
            return null;
        }

        private List<ChannelingSchedule> LoadChannelingShedule()
        {
            List<ChannelingSchedule> channelingSchedule = new List<ChannelingSchedule>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    channelingSchedule = new ChannelingScheduleService().SheduleGetByStatus();

                }
                catch (Exception ex) { }
            }
            return channelingSchedule;
        }

        private List<OPDTbDto> GetAllChannelingByStatus()
        {
            List<OPD> opd = new List<OPD>();
            List<OPDTbDto> oPDTbDto = new List<OPDTbDto>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    opd = new ChannelingService().GetAllChannelingByStatus();
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
                            AppoimentNo = item.AppoimentNo,
                            paymentStatus = item.paymentStatus,
                            schedularId = item.schedularId
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

        [HttpGet("GetSheduleGetById")]
        private ActionResult<ChannelingSchedule> GetSheduleGetById(int Id)
        {
            ChannelingSchedule channelingSchedule = new ChannelingSchedule();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    channelingSchedule = new ChannelingScheduleService().SheduleGetById(Id);

                }
                catch (Exception ex) { }
            }
            return channelingSchedule;
        }

        public IActionResult AddNewChannel([FromBody] OPDDto oPDDto)
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
                    decimal consultantFee = new DefaultService().GetDefailtConsaltantPrice();
                    OPD OPDobj = new OPD();
                    oPDDto.opd.PatientID = patient.Id;
                    oPDDto.opd.DateTime = DateTime.Now;
                    oPDDto.opd.RoomID = 1;
                    oPDDto.opd.ModifiedUser = Convert.ToInt32(userIdCookie);
                    oPDDto.opd.CreatedUser = Convert.ToInt32(userIdCookie);
                    oPDDto.opd.AppoimentNo = oPDDto.opd.AppoimentNo;
                    oPDDto.opd.CreateDate = DateTime.Now;
                    oPDDto.opd.ModifiedDate = DateTime.Now;
                    oPDDto.opd.HospitalFee = oPDDto.OpdType == 1 ? hospitalFee : 0;
                    oPDDto.opd.paymentStatus = PaymentStatus.NOT_PAID;
                    oPDDto.opd.invoiceType = InvoiceType.CHE;
                    oPDDto.opd.ConsultantFee = consultantFee;

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

        public IActionResult DeleteChanneling()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                     new ChannelingService().DeleteChanneling(myChannelling.Id);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        private OPD LoadChannelingByID(int id)
        {
            OPD channeling = new OPD();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    channeling = new ChannelingService().GetChannelByIDNew(id);

                }
                catch (Exception ex) { }
            }
            return channeling;
        }

        public ActionResult OpenQR(int Id)
        {
            ChannelingDto channelingDto = new ChannelingDto();
            channelingDto.chID = Id;

            return PartialView("_PartialQR", channelingDto);
        }
    }
}
