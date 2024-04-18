using HospitalMgrSystem.Model;
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

        public ActionResult DownloadReport([FromBody] Report reportData)
        {
            Report report = new Report();
            ReportsService reportsService = new ReportsService();

            report.StartTime = reportData.StartTime;
            report.EndTime = reportData.EndTime;
            try
            {
                report.listopd = reportsService.GetAllOPDByAndDateRangePaidStatus(reportData.StartTime, reportData.EndTime, PaymentStatus.PAID, "OPD");
                report.listNeedToPayOPD = reportsService.GetAllOPDByDateRangeAndNeedToPayStatus(reportData.StartTime, reportData.EndTime, "OPD");
                report.listNightShiftOPD = reportsService.GetAllOPDByDateRangeAndNightShiftStatus(reportData.StartTime, reportData.EndTime, "OPD");
                report.listNotPaidOPD = reportsService.GetAllOPDAndChannellingByAndDateRangeNotPaid(reportData.StartTime, reportData.EndTime, InvoiceType.OPD, "OPD");
                report.listopdGrugs = reportsService.GetAllOPDGrugsByDateRangePaidStatus(reportData.StartTime, reportData.EndTime, "OPD");
                report.OPDPaymentData = reportsService.GetAllOPDPaymentsData(reportData.StartTime, reportData.EndTime, "OPD");

                report.listXRAY = reportsService.GetAllOPDByAndDateRangePaidStatus(report.StartTime, report.EndTime, PaymentStatus.PAID, "X-RAY");
                report.listNeedToPayXRAY = reportsService.GetAllOPDByDateRangeAndNeedToPayStatus(reportData.StartTime, reportData.EndTime, "X-RAY");
                report.listNightShiftXRAY = reportsService.GetAllOPDByDateRangeAndNightShiftStatus(reportData.StartTime, reportData.EndTime, "X-RAY");
                report.listNotPaidXRAY = reportsService.GetAllOPDAndChannellingByAndDateRangeNotPaid(reportData.StartTime, reportData.EndTime, InvoiceType.OPD, "X-RAY");
                report.listXRAYGrugs = reportsService.GetAllOPDGrugsByDateRangePaidStatus(reportData.StartTime, reportData.EndTime, "X-RAY");
                report.XRAYPaymentData = reportsService.GetAllOPDPaymentsData(reportData.StartTime, reportData.EndTime, "X-RAY");

                report.listOTHER = reportsService.GetAllOPDByAndDateRangePaidStatus(report.StartTime, report.EndTime, PaymentStatus.PAID, "Other");
                report.listNeedToPayOTHER = reportsService.GetAllOPDByDateRangeAndNeedToPayStatus(reportData.StartTime, reportData.EndTime, "Other");
                report.listNightShiftOTHER = reportsService.GetAllOPDByDateRangeAndNightShiftStatus(reportData.StartTime, reportData.EndTime, "Other");
                report.listNotPaidOTHER = reportsService.GetAllOPDAndChannellingByAndDateRangeNotPaid(reportData.StartTime, reportData.EndTime, InvoiceType.OPD, "Other");
                report.listOTHERGrugs = reportsService.GetAllOPDGrugsByDateRangePaidStatus(reportData.StartTime, reportData.EndTime, "Other");
                report.OTHERPaymentData = reportsService.GetAllOPDPaymentsData(reportData.StartTime, reportData.EndTime, "Other");

                report.listChanneling = reportsService.GetAllChannelingByDateRangePaidStatus(reportData.StartTime, reportData.EndTime);
                report.listNeedToPayChanneling = reportsService.GetAllChannelingByDateRangeAndNeedToPayStatus(reportData.StartTime, reportData.EndTime);
                report.listNotPaidChanneling = reportsService.GetAllChannelingByDateRangeAndNotPaidStatus(reportData.StartTime, reportData.EndTime);
                report.listChannelingGrugs = reportsService.GetAllOPDGrugsByDateRangePaidStatus(reportData.StartTime, reportData.EndTime, null);
                report.channelingPaymentData = reportsService.GetAllOPDPaymentsData(reportData.StartTime, reportData.EndTime, null);

                return PartialView("_PartialReportSummary", report);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }


        }

        public IActionResult filterForm()
        {
            try
            {
                Report oPDDto = new Report();

                if (_OPDDto.InvoicedType == 0)
                {
                    oPDDto.listopd = new ReportsService().GetAllOPDByAndDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime, PaymentStatus.PAID, "OPD");
                    oPDDto.listNeedToPayOPD = new ReportsService().GetAllOPDByDateRangeAndNeedToPayStatus(_OPDDto.StartTime, _OPDDto.EndTime, "OPD");
                    oPDDto.listNightShiftOPD = new ReportsService().GetAllOPDByDateRangeAndNightShiftStatus(_OPDDto.StartTime, _OPDDto.EndTime, "OPD");
                    oPDDto.listNotPaidOPD = new ReportsService().GetAllOPDAndChannellingByAndDateRangeNotPaid(_OPDDto.StartTime, _OPDDto.EndTime, InvoiceType.OPD, "OPD");
                    oPDDto.listopdGrugs = new ReportsService().GetAllOPDGrugsByDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime, "OPD");
                    oPDDto.OPDPaymentData = new ReportsService().GetAllOPDPaymentsData(_OPDDto.StartTime, _OPDDto.EndTime, "OPD");
                }

                if (_OPDDto.InvoicedType == 1)
                {
                    oPDDto.listChanneling = new ReportsService().GetAllChannelingByDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNeedToPayChanneling = new ReportsService().GetAllChannelingByDateRangeAndNeedToPayStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNotPaidChanneling = new ReportsService().GetAllChannelingByDateRangeAndNotPaidStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.channelingPaymentData = new ReportsService().GetAllOPDPaymentsData(_OPDDto.StartTime, _OPDDto.EndTime, null);
                }

                oPDDto.StartTime = _OPDDto.StartTime;
                oPDDto.EndTime = _OPDDto.EndTime;
                oPDDto.InvoicedType = _OPDDto.InvoicedType;

                return View("Index", oPDDto);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

        }

        public IActionResult customSearch(int value)
        {
            try
            {
                DateTime startDate;
                DateTime endDate;

                switch (value)
                {
                    case 1: // Today
                        startDate = DateTime.Today;
                        endDate = startDate.AddDays(1).AddTicks(-1);
                        break;
                    case 2: // Yesterday
                        startDate = DateTime.Today.AddDays(-1);
                        endDate = startDate.AddDays(1).AddTicks(-1);
                        break;
                    case 3: // Last Week
                        startDate = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek + 7) % 7);
                        endDate = startDate.AddDays(7).AddTicks(-1);
                        break;
                    case 4: // Last Month
                        startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-1);
                        endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddTicks(-1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), "Invalid value for custom search.");
                }


                Report oPDDto = new Report();
                _OPDDto.StartTime = startDate;
                _OPDDto.EndTime = endDate;
                if (_OPDDto.InvoicedType == 0)
                {
                    oPDDto.listopd = new ReportsService().GetAllOPDByAndDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime, PaymentStatus.PAID, "OPD");
                    oPDDto.listNeedToPayOPD = new ReportsService().GetAllOPDByDateRangeAndNeedToPayStatus(_OPDDto.StartTime, _OPDDto.EndTime, "OPD");
                    oPDDto.listNightShiftOPD = new ReportsService().GetAllOPDByDateRangeAndNightShiftStatus(_OPDDto.StartTime, _OPDDto.EndTime, "OPD");
                    oPDDto.listNotPaidOPD = new ReportsService().GetAllOPDAndChannellingByAndDateRangeNotPaid(_OPDDto.StartTime, _OPDDto.EndTime, InvoiceType.OPD, "OPD");
                    oPDDto.listopdGrugs = new ReportsService().GetAllOPDGrugsByDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime, "OPD");
                    oPDDto.OPDPaymentData = new ReportsService().GetAllOPDPaymentsData(_OPDDto.StartTime, _OPDDto.EndTime, "OPD");
                }

                if (_OPDDto.InvoicedType == 1)
                {
                    oPDDto.listChanneling = new ReportsService().GetAllChannelingByDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNeedToPayChanneling = new ReportsService().GetAllChannelingByDateRangeAndNeedToPayStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNotPaidChanneling = new ReportsService().GetAllOPDAndChannellingByAndDateRangeNotPaid(_OPDDto.StartTime, _OPDDto.EndTime, InvoiceType.CHE, "Channelling");
                    oPDDto.channelingPaymentData = new ReportsService().GetAllOPDPaymentsData(_OPDDto.StartTime, _OPDDto.EndTime, null);
                }

                oPDDto.StartTime = _OPDDto.StartTime;
                oPDDto.EndTime = _OPDDto.EndTime;
                oPDDto.InvoicedType = _OPDDto.InvoicedType;

                return View("Index", oPDDto);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }

        }
    }
}
