using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Default;
using HospitalMgrSystem.Service.OPD;
using HospitalMgrSystem.Service.OPDSchedule;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class OPDSchedulerController : Controller
    {


        [BindProperty]
        public OPDSchedulerDto? _oPDSchedulerDto { get; set; }


        public IActionResult Index()
        {
            OPDSchedulerDto oPDSchedulerDto = new OPDSchedulerDto();
            oPDSchedulerDto.OPDSchedulerList = LoadAllOPDShedule();
            return View(oPDSchedulerDto);
        }

        public IActionResult AddTodayOPDShedular([FromBody] List<OPDScheduler> oPDSchedulers)
        {
            try
            {
                foreach (OPDScheduler oPDScheduler in oPDSchedulers)
                {
                    OPDSchedulerService oPDSchedulerService = new OPDSchedulerService();
                    oPDScheduler.CreateUser = 0;
                    oPDScheduler.CreateDate = DateTime.Now;
                    oPDScheduler.ModifiedUser = 0;
                    oPDScheduler.ModifiedDate = DateTime.Now;
                    oPDSchedulerService.CreateOPDSchedule(oPDScheduler);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return RedirectToAction("Index");
            }
        }

        public ActionResult OPDSchedulaerPOP([FromBody] OPDSchedulerDto _oPDSchedulerDto)
        {

            OPDSchedulerDto oPDSchedulerDto = new OPDSchedulerDto();
            oPDSchedulerDto.Consultants = LoadConsultants();
            DateTime dateToday = DateTime.Now.Date;
            if (_oPDSchedulerDto != null && _oPDSchedulerDto.cDate != null)
            {
                dateToday = _oPDSchedulerDto.cDate;
                oPDSchedulerDto.cDate = dateToday;
            }
            else
            {
                oPDSchedulerDto.cDate= dateToday;
            }
            oPDSchedulerDto.OPDSchedulerList = LoadOPDSheduleByDate(dateToday);
            if (oPDSchedulerDto.OPDSchedulerList != null)
            {
                foreach (OPDScheduler oPDScheduler in oPDSchedulerDto.OPDSchedulerList)
                {
                    
                    //oPDSchedulerDto.cDate = oPDScheduler.cDate;
                    if (oPDScheduler.OPDSession == OPDSession.Morning)
                    {
                        oPDSchedulerDto.activeMo = oPDScheduler.isActiveSession;
                        oPDSchedulerDto.startTimeMo = oPDScheduler.startTime;
                        oPDSchedulerDto.endTimeMo = oPDScheduler.endTime;
                        oPDSchedulerDto.DrMoID = oPDScheduler.ConsultantId;
                        oPDSchedulerDto.OPDSessionMo = oPDScheduler.OPDSession;
                        oPDSchedulerDto.OPDSheduleMoID = oPDScheduler.Id;
                        oPDSchedulerDto.OPDSchedulerStatusMo = oPDScheduler.OPDSchedulerStatus;
                    }
                    else if (oPDScheduler.OPDSession == OPDSession.Day)
                    {
                        oPDSchedulerDto.activeDa = oPDScheduler.isActiveSession;
                        oPDSchedulerDto.startTimeDay = oPDScheduler.startTime;
                        oPDSchedulerDto.endTimeDay = oPDScheduler.endTime;
                        oPDSchedulerDto.DrDaID = oPDScheduler.ConsultantId;
                        oPDSchedulerDto.OPDSessionDa = oPDScheduler.OPDSession;
                        oPDSchedulerDto.OPDSheduleDaID = oPDScheduler.Id;
                        oPDSchedulerDto.OPDSchedulerStatusDa = oPDScheduler.OPDSchedulerStatus;
                    }
                    else if (oPDScheduler.OPDSession == OPDSession.Night)
                    {
                        oPDSchedulerDto.activeNi = oPDScheduler.isActiveSession;
                        oPDSchedulerDto.startTimeNi = oPDScheduler.startTime;
                        oPDSchedulerDto.endTimeNi = oPDScheduler.endTime;
                        oPDSchedulerDto.DrNiID = oPDScheduler.ConsultantId;
                        oPDSchedulerDto.OPDSessionNi = oPDScheduler.OPDSession;
                        oPDSchedulerDto.OPDSheduleNiID = oPDScheduler.Id;
                        oPDSchedulerDto.OPDSchedulerStatusNi = oPDScheduler.OPDSchedulerStatus;
                    }
                   
                        
                }
            }
            return PartialView("_PartialAddOPDScheduler", oPDSchedulerDto);


        }


        private List<Consultant> LoadConsultants()
        {
            List<Consultant> consultants = new List<Consultant>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    consultants = new ConsultantService().GetAllOPDConsultantByStatus();

                }
                catch (Exception ex) { }
            }
            return consultants;
        }

        private List<OPDScheduler> LoadOPDSheduleByDate(DateTime cDate)
        {
            List<OPDScheduler> oPDSchedulers = new List<OPDScheduler>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    oPDSchedulers = new OPDSchedulerService().GetOPDSchedulersByCurrentDate(cDate);

                }
                catch (Exception ex) { }
            }
            return oPDSchedulers;
        }

        private List<OPDScheduler> LoadAllOPDShedule()
        {
            List<OPDScheduler> oPDSchedulers = new List<OPDScheduler>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    oPDSchedulers = new OPDSchedulerService().GetAllOPDSchedulerDByStatus();

                }
                catch (Exception ex) { }
            }
            return oPDSchedulers;
        }

    }

}
