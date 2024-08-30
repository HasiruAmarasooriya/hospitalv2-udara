using HospitalMgrSystem.DataAccess;
using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.ChannelingSchedule;
using HospitalMgrSystem.Service.User;
using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Service.Report
{
	public class DrugDataTemp
	{
		public int DrugId { get; set; }

		public decimal Qty { get; set; }

		// Add other properties as needed
		public Drug drugData { get; set; }
	}

	public class ReportsService
	{
		#region PAYMENT REPORTS

		// Only for the OPD, XRAY and Investigation
		public PaymentSummaryOpdXrayOtherDTO GetAllOPDPaymentsDataSP(DateTime startDate, string description)
		{
			using var context = new HospitalDBContext();
			var reportDataOpdPaidDtos = new List<PaymentSummaryOpdXrayOtherDTO>();

			reportDataOpdPaidDtos = context.Set<PaymentSummaryOpdXrayOtherDTO>()
				.FromSqlRaw("EXEC GetPaymentSummaryForOpdXrayOther @SelectedDate = {0}, @Description = {1}", startDate, description)
				.ToList();

			if (reportDataOpdPaidDtos.Count <= 0) return new PaymentSummaryOpdXrayOtherDTO();

			var paymentSummaryOpdXrayOtherDto = new PaymentSummaryOpdXrayOtherDTO
			{
				TotalAmount = reportDataOpdPaidDtos[0].TotalAmount,
				TotalRefundAmount = reportDataOpdPaidDtos[0].TotalRefundAmount,
				TotalPaidAmount = reportDataOpdPaidDtos[0].TotalPaidAmount
			};

			return paymentSummaryOpdXrayOtherDto;
		}

		public List<PaymentSummaryOfDoctorsOPDDTO> GetAllOPDPaymentsDataOfOPDDoctorsSP(DateTime startDate, string description)
		{
			using var context = new HospitalDBContext();
			var reportDataOpdPaidDtos = new List<PaymentSummaryOfDoctorsOPDDTO>();

			reportDataOpdPaidDtos = context.Set<PaymentSummaryOfDoctorsOPDDTO>()
				.FromSqlRaw("EXEC GetPaymentSummaryGroupByDoctorNameANDDateAndOPDType @SelectedDate = {0}, @Description = {1}", startDate, description)
				.ToList();

			return reportDataOpdPaidDtos;
		}

		public Payment GetAllOPDPaymentsData(DateTime startDate, DateTime endDate, string description)
		{
			var paymentData = new Payment();
			using var dbContext = new HospitalDBContext();
			List<int> opdIds, invoiceList, opdIdsNeedToPay, invoiceListNeedToPay, opdIdsNotPaid, invoiceListNotPaid;

			if (description == null)
			{
				opdIds = dbContext.OPD
					.Where(o => o.Status == CommonStatus.Active && o.invoiceType == InvoiceType.CHE &&
								o.DateTime >= startDate && o.DateTime <= endDate &&
								o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
				invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).Select(r => r.Id)
					.ToList();

				opdIdsNeedToPay = dbContext.OPD
					.Where(o => o.Status == CommonStatus.Active && o.invoiceType == InvoiceType.CHE &&
								o.DateTime >= startDate && o.DateTime <= endDate &&
								o.paymentStatus == PaymentStatus.NEED_TO_PAY).Select(r => r.Id).ToList();
				invoiceListNeedToPay = dbContext.Invoices.Where(o => opdIdsNeedToPay.Contains(o.ServiceID))
					.Select(r => r.Id).ToList();

				opdIdsNotPaid = dbContext.OPD
					.Where(o => o.Status == CommonStatus.Active && o.invoiceType == InvoiceType.CHE &&
								o.DateTime >= startDate && o.DateTime <= endDate &&
								o.paymentStatus == PaymentStatus.NOT_PAID).Select(r => r.Id).ToList();
				invoiceListNotPaid = dbContext.Invoices.Where(o => opdIdsNotPaid.Contains(o.ServiceID))
					.Select(r => r.Id).ToList();
			}
			else
			{
				opdIds = dbContext.OPD
					.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
								o.DateTime >= startDate && o.DateTime <= endDate &&
								o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
				invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).Select(r => r.Id)
					.ToList();

				opdIdsNeedToPay = dbContext.OPD
					.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
								o.DateTime >= startDate && o.DateTime <= endDate &&
								o.paymentStatus == PaymentStatus.NEED_TO_PAY).Select(r => r.Id).ToList();
				invoiceListNeedToPay = dbContext.Invoices.Where(o => opdIdsNeedToPay.Contains(o.ServiceID))
					.Select(r => r.Id).ToList();

				opdIdsNotPaid = dbContext.OPD
					.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
								o.DateTime >= startDate && o.DateTime <= endDate &&
								o.paymentStatus == PaymentStatus.NOT_PAID).Select(r => r.Id).ToList();
				invoiceListNotPaid = dbContext.Invoices.Where(o => opdIdsNotPaid.Contains(o.ServiceID))
					.Select(r => r.Id).ToList();
			}


			var totalCashierAmount = dbContext.Payments
				.Where(o => o.BillingType == BillingType.CASHIER && invoiceList.Contains(o.InvoiceID))
				.Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

			var totalBalanceAmount = dbContext.Payments
				.Where(o => o.BillingType == BillingType.BALENCE && invoiceList.Contains(o.InvoiceID))
				.Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

			var totalRefundAmount = dbContext.Payments
				.Where(o => o.BillingType == BillingType.REFUND && invoiceList.Contains(o.InvoiceID))
				.Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

			var totalRefundAmountInNeedToPay = dbContext.Payments
				.Where(o => o.BillingType == BillingType.REFUND && invoiceListNeedToPay.Contains(o.InvoiceID))
				.Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

			var removedItemSumNeedToPay = dbContext.InvoiceItems.Where(o =>
					o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNeedToPay.Contains(o.InvoiceId))
				.Sum(o => o.price * o.qty);
			var removedItemSumRefund = dbContext.InvoiceItems.Where(o =>
					o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNotPaid.Contains(o.InvoiceId))
				.Sum(o => o.price * o.qty);

			var totalRefund = removedItemSumNeedToPay + removedItemSumRefund;
			var totalAmount = totalCashierAmount + totalBalanceAmount + removedItemSumNeedToPay +
							  removedItemSumRefund;
			var totalPaidAmount = totalAmount + totalRefundAmount;

			var oldPaidAmount = totalPaidAmount - (totalRefundAmount + totalRefundAmountInNeedToPay);
			var actRefundAmount = oldPaidAmount - totalPaidAmount;
			var needToPayTotal = totalRefund - actRefundAmount;

			paymentData.TotalAmountOld = oldPaidAmount;
			paymentData.TotalAmountNeedToPay = needToPayTotal;
			paymentData.TotalAmountRefunded = actRefundAmount;
			paymentData.TotalAmount = totalAmount;
			paymentData.TotalPaidAmount = totalPaidAmount;
			paymentData.TotalRefund = totalRefund;

			return paymentData;
		}


		public Payment GetAllChannelingPaymentsData(DateTime startDate, DateTime endDate)
		{
			var paymentData = new Payment();
			using var dbContext = new HospitalDBContext();

			var opds = new List<Model.OPD>();
			var channelingScheduleService = new ChannelingScheduleService();
			List<Model.ChannelingSchedule> scanDoctorSessionDetails = new List<Model.ChannelingSchedule>();
			List<Model.InvoiceItem> mtInvoiceItemList = new List<Model.InvoiceItem>();

			var schedularList = dbContext.ChannelingSchedule
				.Include(o => o.Consultant)
				.Include(o => o.Consultant!.Specialist)
				.Where(o => o.Status == 0 && o.DateTime >= startDate && o.DateTime <= endDate)
				.ToList();

			List<int> scheduleIds = schedularList.Select(o => o.Id).ToList();

			var opdList = dbContext.OPD
								.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && scheduleIds.Contains(o.schedularId))
								.ToList();
			List<int> opdIdList = opdList.Select(o => o.Id).ToList();

			List<Invoice> invoiceList = dbContext.Invoices.Where(o => opdIdList.Contains(o.ServiceID) && o.InvoiceType == InvoiceType.CHE && o.Status == 0).ToList();

			List<int> invoiceIdList = invoiceList.Select(o => o.Id).ToList();

			mtInvoiceItemList = dbContext.InvoiceItems
					 .Where(o => o.Status == 0 && invoiceIdList.Contains(o.InvoiceId))
					 .ToList();

			decimal invoicePaidTotal = mtInvoiceItemList.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.BILLED)
												.Sum(o => o.price * o.qty);
			decimal invoiceRefundTotal = mtInvoiceItemList.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove)
									.Sum(o => o.price * o.qty);


			paymentData.TotalAmountOld = invoicePaidTotal + invoiceRefundTotal;
			paymentData.TotalAmountRefunded = invoiceRefundTotal;
			paymentData.TotalAmount = invoicePaidTotal;
			paymentData.TotalPaidAmount = invoicePaidTotal;
			return paymentData;
		}


		#endregion

		#region OTHER REPORTS

		public List<Model.OPD> GetAllOtherByDateRangePaidStatus(DateTime startDate, DateTime endDate)
		{
			var mtList = new List<Model.OPD>();
			var newmtList = new List<Model.OPD>();


			using var dbContext = new HospitalDBContext();
			mtList = dbContext.OPD
				.Include(c => c.patient)
				.Include(c => c.consultant)
				.Include(c => c.room)
				.Include(c => c.nightShiftSession.User)
				.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.PAID && o.Description == "Other")
				.OrderByDescending(o => o.Id)
				.ToList();

			var opdIds = dbContext.OPD
				.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.PAID && o.Description == "Other").Select(r => r.Id)
				.ToList();
			var invoiceList = dbContext.Invoices
				.Where(o => o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID)).ToList();

			foreach (var item in invoiceList)
			{
				var opdObj = new Model.OPD();
				opdObj = mtList.Where(o => o.Id == item.ServiceID).SingleOrDefault();

				var invoiceItemList = dbContext.InvoiceItems.Where(o =>
					o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED).ToList();
				var user = new Model.User();
				user = GetUserById(opdObj.ModifiedUser);

				opdObj.issuedUser = user;
				opdObj.invoiceID = item.Id;
				opdObj.TotalAmount = 0;
				opdObj.cashier = opdObj.nightShiftSession.User;
				opdObj.TotalPaidAmount = 0;
				opdObj.deviation = 0;
				foreach (var invoiceItem in invoiceItemList)
				{
					opdObj.TotalAmount += invoiceItem.Total;
				}

				var paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
				foreach (var payment in paymentList)
				{
					var paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount +
									 payment.ChequeAmount + payment.GiftCardAmount;
					opdObj.TotalPaidAmount += paidAmount;
				}

				opdObj.deviation = opdObj.TotalPaidAmount - opdObj.TotalAmount;
				newmtList.Add(opdObj);
			}

			return newmtList;
		}

		#endregion

		#region CHANNELING REPORTS

		public List<ReportOpdXrayOtherDrugs> GetAllOpdXrayOtherDrugsSP(DateTime dateTime, string description)
		{
			using var context = new HospitalDBContext();
			var reportDataOpdPaidDtos = new List<ReportOpdXrayOtherDrugs>();

			reportDataOpdPaidDtos = context.Set<ReportOpdXrayOtherDrugs>()
				.FromSqlRaw("EXEC GellAllOpdDrugsByDateReport @SelectedDate = {0}, @Description = {1}", dateTime, description)
				.ToList();

			return reportDataOpdPaidDtos;
		}

		public List<ReportOpdXrayOtherPaidDto> GetAllOpdXrayOtherPaidDetailsSp(DateTime dateTime, string description)
		{
			using var context = new HospitalDBContext();
			var reportDataOpdPaidDtos = new List<ReportOpdXrayOtherPaidDto>();

			reportDataOpdPaidDtos = context.Set<ReportOpdXrayOtherPaidDto>()
				.FromSqlRaw("EXEC GetAllOpdXrayOtherReportByDateAndDesc @SelectedDate = {0}, @Description = {1}", dateTime, description)
				.ToList();

			return reportDataOpdPaidDtos;
		}

		public List<ReportOpdXrayOtherRefundDTO> GetAllOpdXrayOtherRefundDetailsSp(DateTime dateTime, string description)
		{
			using var context = new HospitalDBContext();
			var reportDataOpdPaidDtos = new List<ReportOpdXrayOtherRefundDTO>();

			reportDataOpdPaidDtos = context.Set<ReportOpdXrayOtherRefundDTO>()
				.FromSqlRaw("EXEC GetAllOpdXrayOtherRefundsReportByDateAndDesc @SelectedDate = {0}, @Description = {1}", dateTime, description)
				.ToList();

			return reportDataOpdPaidDtos;
		}

		public List<ChannelingPaidReport> GetAllChannelingPaidReports(DateTime dateTime)
		{
			using var context = new HospitalDBContext();
			var reportData = new List<ChannelingPaidReport>();

			reportData = context.Set<ChannelingPaidReport>()
				.FromSqlRaw("EXEC GetAllChannelingPaidReportByDate @Date = {0}", dateTime)
				.ToList();

			return reportData;
		}

		public List<PreviousForwardBookingDataDto> GetPreviousForwardBookingData(DateTime dateTime)
		{
			using var context = new HospitalDBContext();
			var reportData = new List<PreviousForwardBookingDataDto>();

			reportData = context.Set<PreviousForwardBookingDataDto>()
				.FromSqlRaw("EXEC GetAllForwardBookingPaidPreviousDays @Date = {0}", dateTime)
				.ToList();

			return reportData;
		}

		public List<Model.OPD> GetAllChannelingItemsData(DateTime startDate, DateTime endDate)
		{
			using var dbContext = new HospitalDBContext();

			var opds = new List<Model.OPD>();

			var scheduleIds = dbContext.ChannelingSchedule
				.Include(o => o.Consultant)
				.Include(o => o.Consultant!.Specialist)
				.Where(o => o.Status == 0 && o.DateTime >= startDate && o.DateTime <= endDate)
				.Select(o => o.Id)
				.ToList();

			var opdIdList = dbContext.OPD
								.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && scheduleIds.Contains(o.schedularId))
								.Select(o => o.Id)
								.ToList();

			var invoiceList = dbContext.Invoices.Where(o => opdIdList.Contains(o.ServiceID) && o.InvoiceType == InvoiceType.CHE && o.Status == 0)
								.ToList();

			var invoiceIdList = invoiceList.Select(o => o.Id).ToList();

			var invoiceItemListConsaltant = dbContext.InvoiceItems.Where(o => invoiceIdList.Contains(o.InvoiceId) && o.Status == 0 && o.billingItemsType == BillingItemsType.Consultant && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED)
												.Select(o => o.InvoiceId)
												.ToList();

			var billedConsultantInvoiceserviceIDList = invoiceList.Where(o => invoiceItemListConsaltant.Contains(o.Id)).Select(o => o.ServiceID).ToList();


			var opdChannelingData = dbContext.OPD
				.Include(o => o.consultant)
				.Include(o => o.consultant!.Specialist)
				.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && billedConsultantInvoiceserviceIDList.Contains(o.Id))
				.GroupBy(o => new { o.schedularId })
				.Select(g => new { SchedularId = g.Key, BillCount = g.Count(), Consultant = g.First().consultant })
				.ToList();

			var channelingService = new ChannelingScheduleService();

			int[] scanSpList = { 44, 13, 12 };

			foreach (var channel in opdChannelingData)
			{
				var schedularId = channel.SchedularId.schedularId;

				var result = dbContext.ChannelingSchedule
					.Include(c => c.Consultant)
					.Include(c => c.Consultant!.Specialist)
					.Include(c => c.Room)
					.SingleOrDefault(o => o.Id == schedularId);

				if (scanSpList.Contains(result!.Consultant!.Specialist!.Id))
				{
					result.scanList = channelingService.GetScanListByScheduleID(schedularId);
				}
				else
				{
					result.scanList = null;
				}

				var opd = new Model.OPD
				{
					consultant = channel.Consultant,
					scanDoctorSessionDetails = result.scanList
				};

				opds.Add(opd);
			}

			return opds;
		}



		public List<Model.OPD> GetAllChannelingConsultantsGroups2(DateTime startDate, DateTime endDate)
		{
			using var dbContext = new HospitalDBContext();
			var opds = new List<Model.OPD>();
			var channelingScheduleService = new ChannelingScheduleService();
			var scanDoctorSessionDetails = new List<Model.ChannelingSchedule>();

			var schedularList = dbContext.ChannelingSchedule
				.Include(o => o.Consultant)
				.Include(o => o.Consultant!.Specialist)
				.Where(o => o.Status == 0 && o.DateTime >= startDate && o.DateTime <= endDate)
				.ToList();

			var scheduleIds = schedularList.Select(o => o.Id).ToList();

			var opdList = dbContext.OPD
								.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && scheduleIds.Contains(o.schedularId))
								.ToList();
			var opdIdList = opdList.Select(o => o.Id).ToList();

			var invoiceList = dbContext.Invoices.Where(o => opdIdList.Contains(o.ServiceID) && o.InvoiceType == InvoiceType.CHE && o.Status == 0).ToList();

			var invoiceIdList = invoiceList.Select(o => o.Id).ToList();

			var invoiceItemListConsaltant = dbContext.InvoiceItems.Where(o => invoiceIdList.Contains(o.InvoiceId) && o.Status == 0 && o.billingItemsType == BillingItemsType.Consultant && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED)
												.Select(o => o.InvoiceId)
												.ToList();
			var invoiceItemListHospital = dbContext.InvoiceItems.Where(o => invoiceIdList.Contains(o.InvoiceId) && o.Status == 0 && o.billingItemsType == BillingItemsType.Hospital && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED)
												 .Select(o => o.InvoiceId)
												 .ToList();

			var billedConsultantInvoiceserviceIDList = invoiceList.Where(o => invoiceItemListConsaltant.Contains(o.Id)).Select(o => o.ServiceID).ToList();
			var billedHospitalInvoiceserviceIDList = invoiceList.Where(o => invoiceItemListHospital.Contains(o.Id)).Select(o => o.ServiceID).ToList();

			var opdChannelingData = dbContext.OPD
				.Include(o => o.consultant)
				.Include(o => o.consultant!.Specialist)
				.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && billedConsultantInvoiceserviceIDList.Contains(o.Id))
				.GroupBy(o => new { o.schedularId })
				.Select(g => new { SchedularId = g.Key, BillCount = g.Count(), Consultant = g.First().consultant })
				.ToList();


			foreach (var channel in opdChannelingData)
			{
				var schedularId = channel.SchedularId.schedularId;

				var channelingSchedule = channelingScheduleService.SheduleGetById(schedularId);

				var opd = new Model.OPD
				{
					schedularId = schedularId,
					BillCount = (channelingSchedule.actualPatientCount - channelingSchedule.totalRefundDoctorFeeCount - channelingSchedule.fullRefundCount),
					consultant = channel.Consultant,
					DoctorAmount = channelingSchedule.totalDoctorFeeAmount,
					HospitalAmount = channelingSchedule.totalHospitalFeeAmount,
					DateTime = channelingSchedule.DateTime,
					channelingScheduleData = channelingSchedule,
					DoctorRefundCount = channelingSchedule.totalRefundDoctorFeeCount,
					HospitalRefundCount = channelingSchedule.totalRefundHospitalFeeCount,
					HospitalDiscountAmount = 0M,
					DoctorDiscountAmount = 0M,
					scanDoctorSessionDetails = channelingSchedule.scanList,
					FullRefundCount = channelingSchedule.fullRefundCount
				};

				opds.Add(opd);
			}

			return opds;
		}

		public decimal GetTotalPaidDoctorFeeAmount(int id)
		{
			using var dbContext = new HospitalDBContext();
			var mtOPDList = new List<Model.OPD>();
			List<Invoice> mtInvoiceList = new List<Invoice>();

			mtOPDList = dbContext.OPD
				.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.schedularId == id)
				.OrderByDescending(o => o.Id)
				.ToList();
			var opdIds = mtOPDList.Where(o => o.paymentStatus == PaymentStatus.PAID).Select(o => o.Id).ToList();

			mtInvoiceList = dbContext.Invoices
				.Where(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID))
				.OrderByDescending(o => o.Id)
				.ToList();

			var invoiceIds = mtInvoiceList.Select(o => o.Id).ToList();
			var mtInvoiceItemList = dbContext.InvoiceItems
				.Where(o => o.Status == 0 && o.itemInvoiceStatus != ItemInvoiceStatus.Remove &&
							o.billingItemsType == BillingItemsType.Consultant && invoiceIds.Contains(o.InvoiceId))
				.GroupBy(o => o.InvoiceId)
				.Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
				.ToList();


			decimal totalPaidDoctorFeeAmount = 0;
			if (mtInvoiceItemList != null && mtInvoiceItemList.Count > 0)
			{
				foreach (var invoice in mtInvoiceItemList)
				{
					var invT = invoice.Total != null ? invoice.Total : 0;
					totalPaidDoctorFeeAmount += invT;
				}
			}

			return totalPaidDoctorFeeAmount;
		}

		public decimal GetTotalPaidHospitalFeeAmount(int id)
		{
			using var dbContext = new HospitalDBContext();
			var mtOPDList = new List<Model.OPD>();
			List<Invoice> mtInvoiceList = new List<Invoice>();

			mtOPDList = dbContext.OPD
				.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.schedularId == id)
				.OrderByDescending(o => o.Id)
				.ToList();
			var opdIds = mtOPDList.Where(o => o.paymentStatus == PaymentStatus.PAID).Select(o => o.Id).ToList();

			mtInvoiceList = dbContext.Invoices
				.Where(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID))
				.OrderByDescending(o => o.Id)
				.ToList();

			var invoiceIds = mtInvoiceList.Select(o => o.Id).ToList();
			var mtInvoiceItemList = dbContext.InvoiceItems
				.Where(o => o.Status == 0 && o.itemInvoiceStatus != ItemInvoiceStatus.Remove &&
							o.billingItemsType == BillingItemsType.Hospital && invoiceIds.Contains(o.InvoiceId))
				.GroupBy(o => o.InvoiceId)
				.Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
				.ToList();


			decimal totalPaidHospitalFeeAmount = 0;
			if (mtInvoiceItemList != null && mtInvoiceItemList.Count > 0)
			{
				foreach (var invoice in mtInvoiceItemList)
				{
					var invT = invoice.Total != null ? invoice.Total : 0;
					totalPaidHospitalFeeAmount += invT;
				}
			}

			return totalPaidHospitalFeeAmount;
		}

		public List<(int SchedularId, decimal TotalPaidDoctorFeeAmount)> GetTotalPaidDoctorFeeAmounts(
			DateTime startDate, DateTime endDate)
		{
			using var dbContext = new HospitalDBContext();
			var results = new List<(int SchedularId, decimal TotalPaidDoctorFeeAmount)>();

			var schedularIds = dbContext.OPD
				.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.DateTime >= startDate &&
							o.DateTime <= endDate && o.paymentStatus == PaymentStatus.PAID)
				.Select(o => o.schedularId)
				.Distinct()
				.ToList();

			foreach (var id in schedularIds)
			{
				var opdIds = dbContext.OPD
					.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.schedularId == id &&
								o.DateTime >= startDate && o.DateTime <= endDate)
					.Select(o => o.Id)
					.ToList();

				var invoiceIds = dbContext.Invoices
					.Where(i => i.Status == 0 && i.InvoiceType == InvoiceType.CHE && opdIds.Contains(i.ServiceID))
					.Select(i => i.Id)
					.ToList();

				var totalPaidDoctorFeeAmount = dbContext.InvoiceItems
					.Where(ii =>
						ii.Status == 0 && ii.itemInvoiceStatus != ItemInvoiceStatus.Remove &&
						ii.billingItemsType == BillingItemsType.Consultant && invoiceIds.Contains(ii.InvoiceId))
					.GroupBy(ii => ii.InvoiceId)
					.Select(g => g.Sum(ii => ii.price * ii.qty))
					.DefaultIfEmpty(0)
					.Sum();

				results.Add((id, totalPaidDoctorFeeAmount));
			}

			return results;
		}


		public List<(int SchedularId, decimal TotalPaidHospitalFeeAmount)> GetTotalPaidHospitalFeeAmount(
			DateTime startDate, DateTime endDate)
		{
			using var dbContext = new HospitalDBContext();
			var results = new List<(int SchedularId, decimal TotalPaidHospitalFeeAmount)>();

			var schedularIds = dbContext.OPD
				.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.DateTime >= startDate &&
							o.DateTime <= endDate && o.paymentStatus == PaymentStatus.PAID)
				.Select(o => o.schedularId)
				.Distinct()
				.ToList();

			foreach (var id in schedularIds)
			{
				var opdIds = dbContext.OPD
					.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.schedularId == id &&
								o.DateTime >= startDate && o.DateTime <= endDate)
					.Select(o => o.Id)
					.ToList();

				var invoiceIds = dbContext.Invoices
					.Where(i => i.Status == 0 && i.InvoiceType == InvoiceType.CHE && opdIds.Contains(i.ServiceID))
					.Select(i => i.Id)
					.ToList();

				var totalPaidHospitalFeeAmount = dbContext.InvoiceItems
					.Where(ii =>
						ii.Status == 0 && ii.itemInvoiceStatus != ItemInvoiceStatus.Remove &&
						ii.billingItemsType == BillingItemsType.Hospital && invoiceIds.Contains(ii.InvoiceId))
					.GroupBy(ii => ii.InvoiceId)
					.Select(g => g.Sum(ii => ii.price * ii.qty))
					.DefaultIfEmpty(0)
					.Sum();

				results.Add((id, totalPaidHospitalFeeAmount));
			}

			return results;
		}

		public List<Model.OPD> GetAllChannelingByDateRangePaidStatus(DateTime startDate, DateTime endDate)
		{
			var mtList = new List<Model.OPD>();
			var newmtList = new List<Model.OPD>();

			using var dbContext = new HospitalDBContext();
			mtList = dbContext.OPD
				.Include(c => c.patient)
				.Include(c => c.consultant)
				.Include(c => c.room)
				.Include(c => c.nightShiftSession.User)
				.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.PAID)
				.OrderByDescending(o => o.Id)
				.ToList();

			var opdIds = dbContext.OPD
				.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
			var invoiceList = dbContext.Invoices
				.Where(o => o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID)).ToList();

			foreach (var item in invoiceList)
			{
				var opdObj = new Model.OPD();
				opdObj = mtList.Where(o => o.Id == item.ServiceID).SingleOrDefault();

				var invoiceItemList = dbContext.InvoiceItems.Where(o =>
					o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED).ToList();
				var user = new Model.User();
				user = GetUserById(opdObj.ModifiedUser);

				opdObj.issuedUser = user;
				opdObj.invoiceID = item.Id;
				opdObj.TotalAmount = 0;
				opdObj.cashier = opdObj.nightShiftSession.User;
				opdObj.TotalPaidAmount = 0;
				opdObj.deviation = 0;
				foreach (var invoiceItem in invoiceItemList)
				{
					opdObj.TotalAmount += invoiceItem.Total;
				}

				var paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
				foreach (var payment in paymentList)
				{
					var paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount +
									 payment.ChequeAmount + payment.GiftCardAmount;
					opdObj.TotalPaidAmount += paidAmount;
				}

				opdObj.deviation = opdObj.TotalPaidAmount - opdObj.TotalAmount;
				newmtList.Add(opdObj);
			}

			return newmtList;
		}

		public List<Model.OPD> GetAllChannelingByDateRangeAndNeedToPayStatus(DateTime startDate, DateTime endDate)
		{
			using var dbContext = new HospitalDBContext();

			var mtList = new List<Model.OPD>();
			var newmtList = new List<Model.OPD>();

			mtList = dbContext.OPD
				.Include(c => c.patient)
				.Include(c => c.consultant)
				.Include(c => c.room)
				.Include(c => c.nightShiftSession.User)
				.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.NEED_TO_PAY)
				.OrderByDescending(o => o.Id)
				.ToList();

			var opdIds = dbContext.OPD
				.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.NEED_TO_PAY).Select(r => r.Id).ToList();
			var invoiceList = dbContext.Invoices
				.Where(o => o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID)).ToList();

			foreach (var item in invoiceList)
			{
				var opdObj = new Model.OPD();
				opdObj = mtList.Where(o => o.Id == item.ServiceID).SingleOrDefault();

				var invoiceItemList = dbContext.InvoiceItems.Where(o =>
					o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.Remove).ToList();
				var user = new Model.User();
				user = GetUserById(opdObj.ModifiedUser);

				opdObj.issuedUser = user;
				opdObj.invoiceID = item.Id;
				opdObj.TotalAmount = 0;
				opdObj.TotalRefund = 0;
				opdObj.cashier = opdObj.nightShiftSession.User;
				opdObj.TotalPaidAmount = 0;
				opdObj.deviation = 0;
				foreach (var invoiceItem in invoiceItemList)
				{
					opdObj.TotalRefund += invoiceItem.Total;
				}

				var paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
				decimal tRefund = 0;
				foreach (var payment in paymentList)
				{
					var paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount +
									 payment.ChequeAmount + payment.GiftCardAmount;
					if (payment.BillingType == BillingType.REFUND)
					{
						tRefund += paidAmount;
					}

					opdObj.TotalPaidAmount += paidAmount;
				}

				opdObj.TotalNeedToRefund = opdObj.TotalRefund + tRefund;
				opdObj.TotalOldAmount = opdObj.TotalPaidAmount - tRefund;
				newmtList.Add(opdObj);
			}

			return newmtList;
		}

		public List<ForwardBookingDataTableDTO> GetAllForwardBookingDataForReportByDate(DateTime dateTime)
		{
			using var dbContext = new HospitalDBContext();

			return dbContext.Set<ForwardBookingDataTableDTO>()
				.FromSqlRaw("EXEC GetAllForwardBookingOfCurrentDay @Date = {0}", dateTime)
				.ToList();
		}

		public List<ChannelingRefundReportDto> GetAllChannelingRefundReportByDate(DateTime dateTime)
		{
			using var dbContext = new HospitalDBContext();

			return dbContext.Set<ChannelingRefundReportDto>()
				.FromSqlRaw("EXEC GetAllChannelingRefundReportByDate @Date = {0}", dateTime)
				.ToList();
		}

		public List<ChannelingPaymentSummaryReportDto> GetAllChannelingPaymentsSummaryReportByDate(DateTime dateTime)
		{
			using var dbContext = new HospitalDBContext();

			return dbContext.Set<ChannelingPaymentSummaryReportDto>()
				.FromSqlRaw("EXEC GetAllChannelingPaymentsSummaryReportByDate @Date = {0}", dateTime)
				.ToList();
		}

		public List<Model.OPD> GetAllChannelingByDateRangeAndNeedToPayStatus2(DateTime startDate, DateTime endDate)
		{
			using var dbContext = new HospitalDBContext();

			var mtList = new List<Model.OPD>();
			var newmtList = new List<Model.OPD>();



			var schedularList = dbContext.ChannelingSchedule
								.Include(o => o.Consultant)
								.Include(o => o.Consultant!.Specialist)
								.Where(o => o.Status == 0 && o.DateTime >= startDate && o.DateTime <= endDate)
								.ToList();

			var scheduleIds = schedularList.Select(o => o.Id).ToList();

			mtList = dbContext.OPD
					.Include(c => c.patient)
					.Include(c => c.consultant)
					.Include(c => c.room)
					.Include(c => c.nightShiftSession.User)
					.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && scheduleIds.Contains(o.schedularId))
					.ToList();



			var opdIds = mtList.Select(r => r.Id).ToList();

			var invoiceList = dbContext.Invoices
				.Where(o => o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID)).ToList();

			var invoiceIds = invoiceList.Select(r => r.Id).ToList();

			//var invoiceItrmList = dbContext.InvoiceItems.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceIds.Contains(o.InvoiceId)).ToList();

			var totalRefundAmountByInvoicw = dbContext.InvoiceItems
								.Where(o => o.Status == 0 && o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceIds.Contains(o.InvoiceId))
								.GroupBy(o => o.InvoiceId)
								.Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
								.ToList();


			var filtered_invoiceListIds = totalRefundAmountByInvoicw.Select(r => r.InvoiceId).ToList();
			var filtered_invoiceList = invoiceList.Where(o => o.Status == 0 && filtered_invoiceListIds.Contains(o.Id)).ToList();

			foreach (var item in filtered_invoiceList)
			{
				var opdObj = new Model.OPD();
				opdObj = mtList.Where(o => o.Id == item.ServiceID).SingleOrDefault();

				var invoiceItem = totalRefundAmountByInvoicw.Where(o => o.InvoiceId == item.Id).FirstOrDefault();
				var user = new Model.User();
				user = GetUserById(opdObj.ModifiedUser);

				opdObj.issuedUser = user;
				opdObj.invoiceID = item.Id;
				opdObj.TotalAmount = 0;
				opdObj.TotalRefund = invoiceItem.Total;
				opdObj.cashier = opdObj.nightShiftSession.User;
				opdObj.TotalPaidAmount = 0;
				opdObj.deviation = 0;

				var paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
				decimal tRefund = 0;
				foreach (var payment in paymentList)
				{
					var paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount +
									 payment.ChequeAmount + payment.GiftCardAmount;
					if (payment.BillingType == BillingType.REFUND)
					{
						tRefund += paidAmount;
					}

					opdObj.TotalPaidAmount += paidAmount;
				}

				opdObj.TotalNeedToRefund = opdObj.TotalRefund + tRefund;
				opdObj.TotalOldAmount = opdObj.TotalPaidAmount - tRefund;
				opdObj.channelingScheduleData = schedularList.FirstOrDefault(o => o.Id == opdObj.schedularId) ?? null;

				newmtList.Add(opdObj);
			}

			return newmtList;
		}

		public List<Model.OPD> GetAllChannelingByDateRangeAndNotPaidStatus(DateTime startDate, DateTime endDate)
		{
			var dbContext = new HospitalDBContext();
			var mtList = dbContext.OPD
				.Include(c => c.patient)
				.Include(c => c.consultant)
				.Include(c => c.room)
				.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.NOT_PAID && o.invoiceType == InvoiceType.CHE)
				.OrderByDescending(o => o.Id)
				.ToList();

			var userIds = mtList.Select(r => r.ModifiedUser).ToList();

			var userList = dbContext.Users.Where(o => userIds.Contains(o.Id)).ToList();

			foreach (var item in mtList)
			{
				item.TotalAmount = 0;
				item.TotalAmount += item.HospitalFee;
				item.TotalAmount += item.ConsultantFee;

				foreach (var user in userList)
				{
					if (item.ModifiedUser == user.Id)
					{
						item.issuedUser = user;
					}
				}
			}

			return mtList;
		}

		#endregion

		#region OPD REPORTS

		public List<Model.OPD> GetAllOPDByDateRange(DateTime startDate, DateTime endDate)
		{
			var mtList = new List<Model.OPD>();
			using var dbContext = new HospitalDBContext();
			mtList = dbContext.OPD
				.Include(c => c.patient)
				.Include(c => c.consultant)
				.Include(c => c.room)
				.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate)
				.OrderByDescending(o => o.Id)
				.ToList();

			return mtList;
		}

		public List<(string drugName, decimal drugCount)> GetAllDrugsGroupListWithDate(DateTime startDate,
			DateTime endDate)
		{
			var dbContext = new HospitalDBContext();
			var drugList = dbContext.OPDDrugus
				.Include(c => c.Drug)
				.Include(c => c.opd)
				.Where(o => o.Status == CommonStatus.Active && o.ModifiedDate >= startDate &&
							o.ModifiedDate <= endDate && o.IsRefund == 0 &&
							o.itemInvoiceStatus == ItemInvoiceStatus.BILLED && o.opd!.Description == "Other")
				.GroupBy(o => o.DrugId)
				.Select(r => new { DrugId = r.Key, Count = r.Sum(x => x.Qty) })
				.ToList();

			var drugGroupList = new List<(string drugName, decimal drugCount)>();

			foreach (var item in drugList)
			{
				var drug = dbContext.Drugs.Where(o => o.Id == item.DrugId).FirstOrDefault();
				drugGroupList.Add((drug.DrugName, item.Count));
			}

			return drugGroupList;
		}


		public List<OPDDrugus> GetAllOpdGrugsByDateRangePaidStatus(DateTime startDate, DateTime endDate,
			string? description)
		{
			var mtList = new List<OPDDrugus>();

			using var dbContext = new HospitalDBContext();


			if (description == null)
			{
				var temp = dbContext.OPDDrugus
					.Include(c => c.opd)
					.Include(c => c.Drug)
					.Where(o => o.Status == CommonStatus.Active && o.ModifiedDate >= startDate &&
								o.ModifiedDate <= endDate && o.IsRefund == 0 &&
								o.itemInvoiceStatus == ItemInvoiceStatus.BILLED &&
								o.opd.invoiceType == InvoiceType.CHE)
					.GroupBy(o => o.DrugId)
					.Select(r => new
					{
						DrugId = r.Key,
						Qty = r.Sum(x => x.Qty),
						DrugData = r.Select(x => x.Drug).FirstOrDefault(),
						Total = r.Sum(x => x.Amount)
					}).ToList();

				foreach (var item in temp)
				{
					var opdDrug = new OPDDrugus
					{
						DrugId = item.DrugId,
						Qty = item.Qty,
						Drug = item.DrugData,
						Amount = item.Total
					};

					mtList.Add(opdDrug);
				}
			}
			else
			{
				var temp = dbContext.OPDDrugus
					.Include(c => c.opd)
					.Include(c => c.Drug)
					.Where(o => o.Status == CommonStatus.Active && o.ModifiedDate >= startDate &&
								o.ModifiedDate <= endDate && o.IsRefund == 0 &&
								o.itemInvoiceStatus == ItemInvoiceStatus.BILLED && o.opd.Description == description)
					.GroupBy(o => o.DrugId)
					.Select(r => new
					{
						DrugId = r.Key,
						Qty = r.Sum(x => x.Qty),
						DrugData = r.Select(x => x.Drug).FirstOrDefault(),
						Total = r.Sum(x => x.Amount)
					}).ToList();


				foreach (var item in temp)
				{
					var opdDrug = new OPDDrugus
					{
						DrugId = item.DrugId,
						Qty = item.Qty,
						Drug = item.DrugData,
						Amount = item.Total
					};

					mtList.Add(opdDrug);
				}
			}


			return mtList;
		}

		public List<Model.OPD> GetAllOPDByDateRangeAndNotPaidStatus(DateTime startDate, DateTime endDate)
		{
			var mtList = new List<Model.OPD>();
			using var dbContext = new HospitalDBContext();
			mtList = dbContext.OPD
				.Include(c => c.patient)
				.Include(c => c.consultant)
				.Include(c => c.room)
				.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.NOT_PAID && o.invoiceType == InvoiceType.OPD)
				.OrderByDescending(o => o.Id)
				.ToList();

			return mtList;
		}


		// Only for the OPD, XRAY and Investigation
		public List<Model.OPD> GetAllOPDByDateRangeAndNightShiftStatus(DateTime startDate, DateTime endDate,
			string description)
		{
			var mtList = new List<Model.OPD>();
			var newmtList = new List<Model.OPD>();
			using var dbContext = new HospitalDBContext();
			mtList = dbContext.OPD
				.Include(c => c.patient)
				.Include(c => c.consultant)
				.Include(c => c.room)
				.Include(c => c.nightShiftSession.User)
				.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
							o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.OPD)
				.OrderByDescending(o => o.Id)
				.ToList();

			var opdIds = dbContext.OPD
				.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
							o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.OPD).Select(r => r.Id).ToList();
			List<Invoice> invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).ToList();

			foreach (var item in invoiceList)
			{
				var opdObj = new Model.OPD();
				opdObj = mtList.Where(o => o.Id == item.ServiceID).SingleOrDefault();

				List<InvoiceItem> invoiceItemList = dbContext.InvoiceItems.Where(o =>
					o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED).ToList();
				var user = new Model.User();
				user = GetUserById(opdObj.ModifiedUser);

				opdObj.issuedUser = user;
				opdObj.invoiceID = item.Id;
				opdObj.TotalAmount = 0;
				opdObj.cashier = opdObj.nightShiftSession.User;
				opdObj.TotalPaidAmount = 0;
				opdObj.deviation = 0;
				foreach (var invoiceItem in invoiceItemList)
				{
					opdObj.TotalAmount += invoiceItem.Total;
				}

				List<Payment> paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
				foreach (var payment in paymentList)
				{
					var paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount +
									 payment.ChequeAmount + payment.GiftCardAmount;
					opdObj.TotalPaidAmount += paidAmount;
				}

				opdObj.deviation = opdObj.TotalPaidAmount - opdObj.TotalAmount;
				newmtList.Add(opdObj);
			}

			return newmtList;
		}


		// Only for the OPD, XRAY and Investigation
		public List<Model.OPD> GetAllOPDByDateRangeAndNeedToPayStatus(DateTime startDate, DateTime endDate,
			string description)
		{
			using var dbContext = new HospitalDBContext();
			var mtList = new List<Model.OPD>();
			var newmtList = new List<Model.OPD>();

			mtList = dbContext.OPD
				.Include(c => c.patient)
				.Include(c => c.consultant)
				.Include(c => c.room)
				.Include(c => c.nightShiftSession.User)
				.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
							o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.NEED_TO_PAY)
				.OrderByDescending(o => o.Id)
				.ToList();

			var opdIds = dbContext.OPD
				.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
							o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.NEED_TO_PAY).Select(r => r.Id).ToList();
			List<Invoice> invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).ToList();

			foreach (var item in invoiceList)
			{
				var opdObj = new Model.OPD();
				opdObj = mtList.Where(o => o.Id == item.ServiceID).SingleOrDefault();

				List<InvoiceItem> invoiceItemList = dbContext.InvoiceItems.Where(o =>
					o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.Remove).ToList();
				var user = new Model.User();
				user = GetUserById(opdObj.ModifiedUser);

				opdObj.issuedUser = user;
				opdObj.invoiceID = item.Id;
				opdObj.TotalAmount = 0;
				opdObj.TotalRefund = 0;
				opdObj.cashier = opdObj.nightShiftSession.User;
				opdObj.TotalPaidAmount = 0;
				opdObj.deviation = 0;

				foreach (var invoiceItem in invoiceItemList)
				{
					opdObj.TotalRefund += invoiceItem.Total;
				}

				List<Payment> paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
				decimal tRefund = 0;

				foreach (var payment in paymentList)
				{
					var paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount +
									 payment.ChequeAmount + payment.GiftCardAmount;
					if (payment.BillingType == BillingType.REFUND)
					{
						tRefund += paidAmount;
					}

					opdObj.TotalPaidAmount += paidAmount;
				}

				opdObj.TotalNeedToRefund = opdObj.TotalRefund + tRefund;
				opdObj.TotalOldAmount = opdObj.TotalPaidAmount - tRefund;
				newmtList.Add(opdObj);
			}

			return newmtList;
		}


		// Only for the OPD, XRAY and Investigation
		public (List<Model.OPD>, int, decimal?) GetAllOpdByAndDateRangePaidStatus(DateTime startDate, DateTime endDate,
			PaymentStatus paymentStatus, string description)
		{
			using var dbContext = new HospitalDBContext();
			var newMtList = new List<Model.OPD>();
			decimal? totalAmount = 0;

			var mtList = dbContext.OPD
				.Include(c => c.patient)
				.Include(c => c.consultant)
				.Include(c => c.room)
				.Include(c => c.nightShiftSession.User)
				.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
							o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == paymentStatus && o.isOnOPD == 0)
				.OrderByDescending(o => o.Id)
				.ToList();

			var paidOpdIds = mtList.Select(r => r.Id).ToList();

			var opdIds = dbContext.OPD
				.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
							o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();

			var opdDrugs = dbContext.OPDDrugus
				.Include(c => c.Drug)
				.Include(c => c.Drug!.DrugsCategory)
				.Include(c => c.Drug!.DrugsSubCategory)
				.Where(o => opdIds.Contains(o.opdId)).ToList();

			var invoiceList = dbContext.Invoices.Where(o => paidOpdIds.Contains(o.ServiceID) && o.InvoiceType == InvoiceType.OPD).ToList();

			foreach (var item in invoiceList)
			{
				var opdObj = mtList.SingleOrDefault(o => o.Id == item.ServiceID);

				var invoiceItemList = dbContext.InvoiceItems.Where(o =>
					o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED).ToList();

				var user = GetUserById(opdObj!.ModifiedUser);

				opdObj.issuedUser = user;
				opdObj.invoiceID = item.Id;
				opdObj.TotalAmount = 0;
				opdObj.cashier = opdObj.nightShiftSession?.User;
				opdObj.TotalPaidAmount = 0;
				opdObj.deviation = 0;

				// opdObj.OpdDrugus = opdDrugs.First(o => o.opdId == opdObj.Id);

				foreach (var invoiceItem in invoiceItemList)
				{
					opdObj.TotalAmount += invoiceItem.Total;
				}

				var paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
				foreach (var payment in paymentList)
				{
					var paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount +
									 payment.ChequeAmount + payment.GiftCardAmount;
					opdObj.TotalPaidAmount += paidAmount;
				}

				opdObj.deviation = opdObj.TotalPaidAmount - opdObj.TotalAmount;
				totalAmount += opdObj.TotalAmount;

				newMtList.Add(opdObj);
			}


			//night shift

			DateTime startDateNightShit = startDate;
			DateTime endDateNightShit = endDate;

			startDateNightShit = startDate.AddDays(-1).AddHours(18);
			endDateNightShit = endDate.AddHours(10);

			var mtListnightShift = dbContext.OPD
									.Include(c => c.patient)
									.Include(c => c.consultant)
									.Include(c => c.room)
									.Include(c => c.nightShiftSession.User)
									.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
												o.DateTime >= startDateNightShit && o.DateTime <= endDateNightShit && o.paymentStatus == paymentStatus && o.isOnOPD == 1)
									.OrderByDescending(o => o.Id)
									.ToList();


			var paidNOpdIds = mtListnightShift.Select(r => r.Id).ToList();

			var opdNIds = dbContext.OPD
				.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
							o.DateTime >= startDate && o.DateTime <= endDate &&
							o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();

			var opdNDrugs = dbContext.OPDDrugus
				.Include(c => c.Drug)
				.Include(c => c.Drug!.DrugsCategory)
				.Include(c => c.Drug!.DrugsSubCategory)
				.Where(o => opdNIds.Contains(o.opdId)).ToList();

			var invoiceListN = dbContext.Invoices.Where(o => paidNOpdIds.Contains(o.ServiceID) && o.InvoiceType == InvoiceType.OPD).ToList();

			foreach (var item in invoiceListN)
			{
				var opdObj = mtListnightShift.SingleOrDefault(o => o.Id == item.ServiceID);

				var invoiceItemList = dbContext.InvoiceItems.Where(o =>
					o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED).ToList();

				var user = GetUserById(opdObj!.ModifiedUser);

				opdObj.issuedUser = user;
				opdObj.invoiceID = item.Id;
				opdObj.TotalAmount = 0;
				opdObj.cashier = opdObj.nightShiftSession?.User;
				opdObj.TotalPaidAmount = 0;
				opdObj.deviation = 0;

				// opdObj.OpdDrugus = opdDrugs.First(o => o.opdId == opdObj.Id);

				foreach (var invoiceItem in invoiceItemList)
				{
					opdObj.TotalAmount += invoiceItem.Total;
				}

				var paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
				foreach (var payment in paymentList)
				{
					var paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount +
									 payment.ChequeAmount + payment.GiftCardAmount;
					opdObj.TotalPaidAmount += paidAmount;
				}

				opdObj.deviation = opdObj.TotalPaidAmount - opdObj.TotalAmount;
				totalAmount += opdObj.TotalAmount;

				newMtList.Add(opdObj);
			}


			var count = newMtList.Count;

			return (newMtList, count, totalAmount);
		}


		// Only for the OPD, XRAY and Investigation
		public List<Model.OPD> GetAllOPDAndChannellingByAndDateRangeNotPaid(DateTime startDate, DateTime endDate,
			InvoiceType invoiceType, string description)
		{
			var mtList = new List<Model.OPD>();
			var newmtList = new List<Model.OPD>();
			using var dbContext = new HospitalDBContext();
			if (description != "OPD" && description != "X-RAY" && description != "Other")
			{
				mtList = dbContext.OPD
					.Include(c => c.patient)
					.Include(c => c.consultant)
					.Include(c => c.room)
					.Include(c => c.nightShiftSession.User)
					.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate &&
								o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NOT_PAID &&
								o.invoiceType == invoiceType)
					.OrderByDescending(o => o.Id)
					.ToList();
			}
			else
			{
				mtList = dbContext.OPD
					.Include(c => c.patient)
					.Include(c => c.consultant)
					.Include(c => c.room)
					.Include(c => c.nightShiftSession.User)
					.Where(o => o.Status == CommonStatus.Active && o.Description == description &&
								o.DateTime >= startDate && o.DateTime <= endDate &&
								o.paymentStatus == PaymentStatus.NOT_PAID && o.invoiceType == invoiceType)
					.OrderByDescending(o => o.Id)
					.ToList();
			}

			List<int> opdIds = new();

			if (description != "OPD" && description != "X-RAY" && description != "Other")
			{
				opdIds = dbContext.OPD
					.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate &&
								o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NOT_PAID &&
								o.invoiceType == invoiceType).Select(r => r.Id).ToList();
			}
			else
			{
				opdIds = dbContext.OPD.Where(o =>
					o.Status == CommonStatus.Active && o.Description == description && o.DateTime >= startDate &&
					o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NOT_PAID &&
					o.invoiceType == invoiceType).Select(r => r.Id).ToList();
			}

			List<OPDDrugus> drugsList = dbContext.OPDDrugus.Where(o => opdIds.Contains(o.opdId)).ToList();

			foreach (var item in mtList)
			{
				var opdObj = new Model.OPD();
				opdObj = item;
				var user = new Model.User();
				user = GetUserById(opdObj.ModifiedUser);

				opdObj.issuedUser = user;

				opdObj.TotalAmount = 0;
				opdObj.TotalAmount += opdObj.HospitalFee;
				opdObj.TotalAmount += opdObj.ConsultantFee;
				foreach (var drug in drugsList)
				{
					var price = drug.Price * drug.Qty;
					opdObj.TotalAmount += price;
				}

				newmtList.Add(opdObj);
			}

			return newmtList;
		}

		#endregion


		private Model.User GetUserById(int id)
		{
			var user = new Model.User();

			using var httpClient = new HttpClient();
			try
			{
				user = new UserService().GetUserByID(id);
			}
			catch (Exception ex)
			{
			}

			return user;
		}
	}
}