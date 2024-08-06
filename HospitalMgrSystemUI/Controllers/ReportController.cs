using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.ChannelingSchedule;
using HospitalMgrSystem.Service.Report;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
	public class ReportController : Controller
	{
		[BindProperty] public Report _OPDDto { get; set; }

		public IActionResult Index()
		{
			Report report = new Report();
			_OPDDto = report;
			_OPDDto.StartTime = DateTime.Now;
			_OPDDto.EndTime = DateTime.Now;
			_OPDDto.listopd = (null, null, null);
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
				report.listopd = reportsService.GetAllOpdByAndDateRangePaidStatus(reportData.StartTime,
					reportData.EndTime, PaymentStatus.PAID, "OPD");
				report.listNeedToPayOPD =
					reportsService.GetAllOPDByDateRangeAndNeedToPayStatus(reportData.StartTime, reportData.EndTime,
						"OPD");
				report.listNightShiftOPD =
					reportsService.GetAllOPDByDateRangeAndNightShiftStatus(reportData.StartTime, reportData.EndTime,
						"OPD");
				report.listNotPaidOPD =
					reportsService.GetAllOPDAndChannellingByAndDateRangeNotPaid(reportData.StartTime,
						reportData.EndTime, InvoiceType.OPD, "OPD");
				report.listopdGrugs =
					reportsService.GetAllOpdGrugsByDateRangePaidStatus(reportData.StartTime, reportData.EndTime, "OPD");
				report.OPDPaymentData =
					reportsService.GetAllOPDPaymentsData(reportData.StartTime, reportData.EndTime, "OPD");

				report.listXRAY = reportsService.GetAllOpdByAndDateRangePaidStatus(report.StartTime, report.EndTime,
					PaymentStatus.PAID, "X-RAY");
				report.listNeedToPayXRAY =
					reportsService.GetAllOPDByDateRangeAndNeedToPayStatus(reportData.StartTime, reportData.EndTime,
						"X-RAY");
				report.listNightShiftXRAY =
					reportsService.GetAllOPDByDateRangeAndNightShiftStatus(reportData.StartTime, reportData.EndTime,
						"X-RAY");
				report.listNotPaidXRAY =
					reportsService.GetAllOPDAndChannellingByAndDateRangeNotPaid(reportData.StartTime,
						reportData.EndTime, InvoiceType.OPD, "X-RAY");
				report.listXRAYGrugs =
					reportsService.GetAllOpdGrugsByDateRangePaidStatus(reportData.StartTime, reportData.EndTime,
						"X-RAY");
				report.XRAYPaymentData =
					reportsService.GetAllOPDPaymentsData(reportData.StartTime, reportData.EndTime, "X-RAY");

				report.listOTHER = reportsService.GetAllOpdByAndDateRangePaidStatus(report.StartTime, report.EndTime,
					PaymentStatus.PAID, "Other");
				report.listNeedToPayOTHER =
					reportsService.GetAllOPDByDateRangeAndNeedToPayStatus(reportData.StartTime, reportData.EndTime,
						"Other");
				report.listNightShiftOTHER =
					reportsService.GetAllOPDByDateRangeAndNightShiftStatus(reportData.StartTime, reportData.EndTime,
						"Other");
				report.listNotPaidOTHER =
					reportsService.GetAllOPDAndChannellingByAndDateRangeNotPaid(reportData.StartTime,
						reportData.EndTime, InvoiceType.OPD, "Other");
				report.listOTHERGrugs =
					reportsService.GetAllOpdGrugsByDateRangePaidStatus(reportData.StartTime, reportData.EndTime,
						"Other");
				report.OTHERPaymentData =
					reportsService.GetAllOPDPaymentsData(reportData.StartTime, reportData.EndTime, "Other");

				report.listChanneling =
					reportsService.GetAllChannelingByDateRangePaidStatus(reportData.StartTime, reportData.EndTime);
				report.listNeedToPayChanneling =
					reportsService.GetAllChannelingByDateRangeAndNeedToPayStatus(reportData.StartTime,
						reportData.EndTime);
				report.listNotPaidChanneling =
					reportsService.GetAllChannelingByDateRangeAndNotPaidStatus(reportData.StartTime,
						reportData.EndTime);
				report.listChannelingGrugs =
					reportsService.GetAllOpdGrugsByDateRangePaidStatus(reportData.StartTime, reportData.EndTime, null);
				report.channelingPaymentData =
					reportsService.GetAllOPDPaymentsData(reportData.StartTime, reportData.EndTime, null);

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
				var oPdDto = new Report();
				var reportsService = new ReportsService();

				oPdDto.StartTime = _OPDDto.StartTime;
				oPdDto.EndTime = _OPDDto.EndTime;
				oPdDto.InvoicedType = _OPDDto.InvoicedType;

				switch (_OPDDto.InvoicedType)
				{
					// If the user selects the OPD option
					case 0:
						oPdDto.OpdPaidDtos = reportsService.GetAllOpdXrayOtherPaidDetailsSp(_OPDDto.StartTime, "OPD");
						oPdDto.OpdRefundDtos = reportsService.GetAllOpdXrayOtherRefundDetailsSp(_OPDDto.StartTime, "OPD");
						oPdDto.listopdGrugsDto = reportsService.GetAllOpdXrayOtherDrugsSP(_OPDDto.StartTime, "OPD");
						oPdDto.OpdPaymentDataDto = reportsService.GetAllOPDPaymentsDataSP(_OPDDto.StartTime, "OPD");
						oPdDto.OpdPaymentDataOfDoctorsDto = reportsService.GetAllOPDPaymentsDataOfOPDDoctorsSP(_OPDDto.StartTime, "OPD");

						return View("OPDIndex", oPdDto);

					// If the user selects the Channeling option
					case 1:
						oPdDto.listChanneling = reportsService.GetAllChannelingItemsData(_OPDDto.StartTime, _OPDDto.EndTime);
						oPdDto.ChannelingPaidReports = reportsService.GetAllChannelingPaidReports(_OPDDto.StartTime);
                        oPdDto.ChannelingRefundReportDtos = reportsService.GetAllChannelingRefundReportByDate(_OPDDto.StartTime);
						oPdDto.ChannelingPaymentSummaryReportDtos = reportsService.GetAllChannelingPaymentsSummaryReportByDate(_OPDDto.StartTime);
						oPdDto.ForwardBookingDataTableDtos = reportsService.GetAllForwardBookingDataForReportByDate(_OPDDto.StartTime);
						oPdDto.PreviousForwardBookingDataDtos = reportsService.GetPreviousForwardBookingData(_OPDDto.StartTime);

						return View("ChannelingIndex", oPdDto);

					// If the user selects the OTHER Section
					case 2:
						oPdDto.OtherPaidDtos = reportsService.GetAllOpdXrayOtherPaidDetailsSp(_OPDDto.StartTime, "Other");
						oPdDto.OtherRefundDtos = reportsService.GetAllOpdXrayOtherRefundDetailsSp(_OPDDto.StartTime, "Other");
						oPdDto.listOTHERGrugsDto = reportsService.GetAllOpdXrayOtherDrugsSP(_OPDDto.StartTime, "Other");
						oPdDto.OTHERPaymentDataDto = reportsService.GetAllOPDPaymentsDataSP(_OPDDto.StartTime, "Other");

						return View("OtherIndex", oPdDto);

					// If the user selects the X-RAY option
					case 3:
						oPdDto.XrayPaidDtos = reportsService.GetAllOpdXrayOtherPaidDetailsSp(_OPDDto.StartTime, "X-RAY");
						oPdDto.XrayRefundDtos = reportsService.GetAllOpdXrayOtherRefundDetailsSp(_OPDDto.StartTime, "X-RAY");
						oPdDto.XrayPaymentDataDto = reportsService.GetAllOPDPaymentsDataSP(_OPDDto.StartTime, "X-RAY");

						return View("XRAYIndex", oPdDto);

					default:
						return RedirectToAction("Index");
				}
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
					oPDDto.listopd = new ReportsService().GetAllOpdByAndDateRangePaidStatus(_OPDDto.StartTime,
						_OPDDto.EndTime, PaymentStatus.PAID, "OPD");
					oPDDto.listNeedToPayOPD =
						new ReportsService().GetAllOPDByDateRangeAndNeedToPayStatus(_OPDDto.StartTime, _OPDDto.EndTime,
							"OPD");
					oPDDto.listNightShiftOPD =
						new ReportsService().GetAllOPDByDateRangeAndNightShiftStatus(_OPDDto.StartTime, _OPDDto.EndTime,
							"OPD");
					oPDDto.listNotPaidOPD =
						new ReportsService().GetAllOPDAndChannellingByAndDateRangeNotPaid(_OPDDto.StartTime,
							_OPDDto.EndTime, InvoiceType.OPD, "OPD");
					oPDDto.listopdGrugs =
						new ReportsService().GetAllOpdGrugsByDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime,
							"OPD");
					oPDDto.OPDPaymentData =
						new ReportsService().GetAllOPDPaymentsData(_OPDDto.StartTime, _OPDDto.EndTime, "OPD");
				}

				if (_OPDDto.InvoicedType == 1)
				{
					oPDDto.listChanneling =
						new ReportsService().GetAllChannelingByDateRangePaidStatus(_OPDDto.StartTime, _OPDDto.EndTime);
					oPDDto.listNeedToPayChanneling =
						new ReportsService().GetAllChannelingByDateRangeAndNeedToPayStatus(_OPDDto.StartTime,
							_OPDDto.EndTime);
					oPDDto.listNotPaidChanneling =
						new ReportsService().GetAllOPDAndChannellingByAndDateRangeNotPaid(_OPDDto.StartTime,
							_OPDDto.EndTime, InvoiceType.CHE, "Channelling");
					oPDDto.channelingPaymentData =
						new ReportsService().GetAllOPDPaymentsData(_OPDDto.StartTime, _OPDDto.EndTime, null);
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

		private ChannelingSchedule LoadChannelingSheduleByID(int id)
		{
			ChannelingSchedule channelingSchedule = new ChannelingSchedule();

			using (var httpClient = new HttpClient())
			{
				try
				{
					channelingSchedule = new ChannelingScheduleService().SheduleGetById(id);
				}
				catch (Exception ex)
				{
				}
			}

			return channelingSchedule;
		}
	}
}