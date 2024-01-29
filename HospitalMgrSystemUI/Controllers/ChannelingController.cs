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

        public IActionResult AddNewChannel()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    channelingDto.channeling.CreateDate = DateTime.Now;
                    channelingDto.channeling.ModifiedDate = DateTime.Now;
                    channelingDto.channeling.ChannelingSchedule = null;
                    Channeling ConsultantLists = new ChannelingService().CreateChanneling(channelingDto.channeling);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
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
