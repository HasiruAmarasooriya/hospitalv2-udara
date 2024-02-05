using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.OPD;
using HospitalMgrSystem.Service.Report;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class ReportController : Controller
    {

        [BindProperty]
        public Report _OPDDto { get; set; }
        public IActionResult Index()
        {
            Report report = new Report();
            _OPDDto = report;
            _OPDDto.StartTime = DateTime.Now;
            _OPDDto.EndTime = DateTime.Now;
            _OPDDto.listopd = null;
            return View(_OPDDto);
        }

        public IActionResult filterForm()
        {
                try
                {
                    Report oPDDto = new Report();
                    oPDDto.listopd = new ReportsService().GetAllOPDByAndDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime, PaymentStatus.PAID);
                    return View("Index", oPDDto);

                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
           
        }
    }
}
