using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.Channeling;
using HospitalMgrSystem.Service.ChannelingSchedule;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Default;
using HospitalMgrSystem.Service.Drugs;
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
            ChannelingDto _channelingDto = new ChannelingDto();
            _channelingDto.ChannelingList = GetAllChannelingByStatus();
            return View(_channelingDto);
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

                        oPDChannelingDto.channeling = LoadChannelingByID(Id);
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

        private List<Channeling> GetAllChannelingByStatus()
        {
            List<Channeling> channelingSchedule = new List<Channeling>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    channelingSchedule = new ChannelingService().GetAllChannelingByStatus();

                }
                catch (Exception ex) { }
            }
            return channelingSchedule;
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

        private Channeling LoadChannelingByID(int id)
        {
            Channeling channeling = new Channeling();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    channeling = new ChannelingService().GetChannelByID(id);

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
