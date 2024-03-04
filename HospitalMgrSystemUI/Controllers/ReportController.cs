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

                if (_OPDDto.InvoicedType == 0)
                {
                    oPDDto.listopd = new ReportsService().GetAllOPDByAndDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime, PaymentStatus.PAID);
                    oPDDto.listNeedToPayOPD = new ReportsService().GetAllOPDByDateRangeAndNeedToPayStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNightShiftOPD = new ReportsService().GetAllOPDByDateRangeAndNightShiftStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNotPaidOPD = new ReportsService().GetAllOPDAndChannellingByAndDateRangeNotPaid(_OPDDto.StartTime, _OPDDto.EndTime,InvoiceType.OPD);
                    oPDDto.listopdGrugs = new ReportsService().GetAllOPDGrugsByDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.paymentData = new ReportsService().GetAllPaymentsData(_OPDDto.StartTime, _OPDDto.EndTime);
                } 

                if (_OPDDto.InvoicedType == 1)
                {
                    oPDDto.listChanneling = new ReportsService().GetAllChannelingByDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNeedToPayChanneling = new ReportsService().GetAllChannelingByDateRangeAndNeedToPayStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNotPaidChanneling = new ReportsService().GetAllChannelingByDateRangeAndNotPaidStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.paymentData = new ReportsService().GetAllPaymentsData(_OPDDto.StartTime, _OPDDto.EndTime);
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
                    oPDDto.listopd = new ReportsService().GetAllOPDByAndDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime, PaymentStatus.PAID);
                    oPDDto.listNeedToPayOPD = new ReportsService().GetAllOPDByDateRangeAndNeedToPayStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNightShiftOPD = new ReportsService().GetAllOPDByDateRangeAndNightShiftStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNotPaidOPD = new ReportsService().GetAllOPDAndChannellingByAndDateRangeNotPaid(_OPDDto.StartTime, _OPDDto.EndTime, InvoiceType.OPD);
                    oPDDto.listopdGrugs = new ReportsService().GetAllOPDGrugsByDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.paymentData = new ReportsService().GetAllPaymentsData(_OPDDto.StartTime, _OPDDto.EndTime);
                }

                if (_OPDDto.InvoicedType == 1)
                {
                    oPDDto.listChanneling = new ReportsService().GetAllChannelingByDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNeedToPayChanneling = new ReportsService().GetAllChannelingByDateRangeAndNeedToPayStatus(_OPDDto.StartTime, _OPDDto.EndTime);
                    oPDDto.listNotPaidChanneling = new ReportsService().GetAllOPDAndChannellingByAndDateRangeNotPaid(_OPDDto.StartTime, _OPDDto.EndTime, InvoiceType.CHE);
                    oPDDto.paymentData = new ReportsService().GetAllPaymentsData(_OPDDto.StartTime, _OPDDto.EndTime);
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
