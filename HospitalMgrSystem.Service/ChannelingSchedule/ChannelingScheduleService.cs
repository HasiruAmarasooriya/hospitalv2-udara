using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.Default;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.ChannelingSchedule
{
	public class ChannelingScheduleService : IChannelingSchedule
	{
		public object channelingSchedule;


		private object dbContext;

		public List<Model.ChannelingSchedule> GetAllChannelingScheduleByDateTime(DateTime startDate, DateTime endDate)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule
					.Include(c => c.Consultant)
					.Include(c => c.Consultant.Specialist)
					.Include(c => c.Consultant)
					.Where(o => o.Status == 0 && o.DateTime >= startDate && o.DateTime <= endDate)
					.OrderByDescending(o => o.DateTime)
					.ToList();

				return schedularIdList;
			}
		}

		public List<Model.ChannelingSchedule> GetAllChannelingScheduleByDateTimeWithSpeciality(DateTime startDate,
			DateTime endDate, int specialistId)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule
					.Include(c => c.Consultant)
					.Include(c => c.Consultant.Specialist)
					.Include(c => c.Consultant)
					.Where(o => o.Status == 0 && o.DateTime >= startDate && o.DateTime <= endDate &&
								o.Consultant.SpecialistId == specialistId)
					.OrderByDescending(o => o.DateTime)
					.ToList();

				return schedularIdList;
			}
		}

		public List<Model.ChannelingSchedule> GetAllChannelingScheduleByDateTimeWithStatus(DateTime startDate,
			DateTime endDate, ChannellingScheduleStatus channellingScheduleStatus)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule
					.Include(c => c.Consultant)
					.Include(c => c.Consultant.Specialist)
					.Include(c => c.Consultant)
					.Where(o => o.Status == 0 && o.DateTime >= startDate && o.DateTime <= endDate &&
								o.scheduleStatus == channellingScheduleStatus)
					.OrderByDescending(o => o.DateTime)
					.ToList();

				return schedularIdList;
			}
		}

		public List<Model.ChannelingSchedule> GetAllChannelingScheduleByDateTimeWithSpecialityAndStatus(
			DateTime startDate, DateTime endDate, ChannellingScheduleStatus channellingScheduleStatus, int specialistId)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule
					.Include(c => c.Consultant)
					.Include(c => c.Consultant.Specialist)
					.Include(c => c.Consultant)
					.Where(o => o.Status == 0 && o.DateTime >= startDate && o.DateTime <= endDate &&
								o.scheduleStatus == channellingScheduleStatus &&
								o.Consultant.SpecialistId == specialistId)
					.OrderByDescending(o => o.DateTime)
					.ToList();

				return schedularIdList;
			}
		}


		public Model.ChannelingSchedule CreateChannelingSchedule(Model.ChannelingSchedule channelingSchedule)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				if (channelingSchedule.Id == 0)
				{
					dbContext.ChannelingSchedule.Add(channelingSchedule);
					dbContext.SaveChanges();
				}
				else
				{
					Model.ChannelingSchedule result =
						(from p in dbContext.ChannelingSchedule where p.Id == channelingSchedule.Id select p)
						.SingleOrDefault();

					if (channelingSchedule.NoOfAppointment >= result.NoOfAppointment)
					{
						result.DateTime = channelingSchedule.DateTime;
						result.NoOfAppointment = channelingSchedule.NoOfAppointment;
						result.Status = channelingSchedule.Status;
						result.scheduleStatus = channelingSchedule.scheduleStatus;
						result.RoomId = channelingSchedule.RoomId;

						dbContext.SaveChanges();
					}
				}

				return dbContext.ChannelingSchedule.Find(channelingSchedule.Id);
			}
		}


		public List<Model.ChannelingSchedule> SheduleGetByStatus()
		{
			List<Model.ChannelingSchedule> mtList = new List<Model.ChannelingSchedule>();
			using (DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
			{
				mtList = dbContext.ChannelingSchedule
					.Include(c => c.Room)
					.Include(c => c.Consultant)
					.Include(c => c.Consultant!.Specialist)
					.Include(c => c.Consultant)
					.Where(o => o.Status == 0)
					.OrderByDescending(o => o.DateTime)
					.ToList();

				// Get Total amount of Each consultant using OPD table
				var totalAmountOfEachCashier = dbContext.OPD
					.Where(o => o.invoiceType == InvoiceType.CHE)
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.ConsultantFee + o.HospitalFee + o.OtherFee) })
					.ToList();

				var bookedChannelCount = dbContext.OPD
					.Where(o => o.invoiceType == InvoiceType.CHE)
					.GroupBy(o => new { o.schedularId, o.AppoimentNo })
					.Select(g => new { ScheduleId = g.Key.schedularId, Count = g.Count() })
					.ToList();

				/*var bookedChannelCount = dbContext.OPD
                    .Where(o => o.invoiceType == InvoiceType.CHE)
                    .GroupBy(o => o.schedularId)
                    .Select(g => new { ScheduleId = g.Key, Count = g.Count() })
                    .ToList();*/

				var bookedAndPaidCount = dbContext.OPD
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID)
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Count() })
					.ToList();

				var refundCount = dbContext.OPD
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.NEED_TO_PAY)
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Count() })
					.ToList();

				foreach (var item in mtList)
				{
					foreach (var bookedItem in bookedChannelCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.booked = bookedItem.Count;
						}
					}

					foreach (var bookedItem in bookedAndPaidCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.paid = bookedItem.Count;
						}
					}

					foreach (var bookedItem in refundCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.refund = bookedItem.Count;
						}
					}

					foreach (var totalAmount in totalAmountOfEachCashier)
					{
						if (item.Id == totalAmount.ScheduleId)
						{
							item.totalAmount = totalAmount.Total;
						}
					}
				}
			}

			return mtList;
		}

		public List<Model.ChannelingSchedule> SheduleGetBySelected(DateTime dateTime)
		{
			List<Model.ChannelingSchedule> mtList = new List<Model.ChannelingSchedule>();
			List<Model.OPD> mtOPDList = new List<Model.OPD>();
			List<Model.Invoice> mtInvoiceList = new List<Model.Invoice>();
			//   List<Model.InvoiceItem> mtInvoiceItemList = new List<Model.InvoiceItem>();
			using (DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
			{
				mtList = dbContext.ChannelingSchedule
					.Include(c => c.Room)
					.Include(c => c.Consultant)
					.Include(c => c.Consultant!.Specialist)
					.Include(c => c.Consultant)
					.Where(o => o.Status == Model.Enums.CommonStatus.Active && o.DateTime > dateTime)
					.OrderByDescending(o => o.DateTime)
					.ToList();

				var scheduleIds = mtList.Select(o => o.Id).ToList();
				mtOPDList = dbContext.OPD
					.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && scheduleIds.Contains(o.schedularId))
					.OrderByDescending(o => o.Id)
					.ToList();

				var opdIds = mtOPDList.Where(o => o.paymentStatus == PaymentStatus.PAID).Select(o => o.Id).ToList();
				mtInvoiceList = dbContext.Invoices
					 .Where(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID))
					 .OrderByDescending(o => o.Id)
					 .ToList();

				var invoiceIds = mtInvoiceList.Select(o => o.Id).ToList();
				var mtInvoiceItemList = dbContext.InvoiceItems
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus != ItemInvoiceStatus.Remove && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();
				var mtInvoiceRefundItemList = dbContext.InvoiceItems
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();

				foreach (var iId in mtInvoiceList)
				{

					foreach (var iItem in mtInvoiceItemList)
					{
						if (iItem.InvoiceId == iId.Id)
						{
							var _invoice = mtInvoiceList.First(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && o.Id == iItem.InvoiceId);
							var rowToUpdate = mtOPDList.FirstOrDefault(o => o.Id == _invoice.ServiceID);

							// Check if the row exists
							if (rowToUpdate != null)
							{
								rowToUpdate.TotalPaidAmount = iItem.Total;
							}

						}
					}

				}

				foreach (var iId in mtInvoiceList)
				{

					foreach (var iItem in mtInvoiceRefundItemList)
					{
						if (iItem.InvoiceId == iId.Id)
						{
							var _invoice = mtInvoiceList.First(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && o.Id == iItem.InvoiceId);
							var rowToUpdate = mtOPDList.FirstOrDefault(o => o.Id == _invoice.ServiceID);

							// Check if the row exists
							if (rowToUpdate != null)
							{
								rowToUpdate.TotalRefund = iItem.Total;
							}

						}
					}

				}


				// Get Total amount of Each consultant using OPD table
				var totalAmountOfEachCashier = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.ConsultantFee + o.HospitalFee + o.OtherFee) })
					.ToList();

				var bookedChannelCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Select(o => o.AppoimentNo).Distinct().Count() })
					.ToList();

				var bookedAllAppinmentChannelCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Select(o => o.AppoimentNo).Count() })
					.ToList();

				/*var bookedChannelCount = dbContext.OPD
                    .Where(o => o.invoiceType == InvoiceType.CHE)
                    .GroupBy(o => o.schedularId)
                    .Select(g => new { ScheduleId = g.Key, Count = g.Count() })
                    .ToList();*/
				var bookedAndRefundAmount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.TotalRefund) })
					.ToList();

				var bookedAndPaidAmount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.TotalPaidAmount) })
					.ToList();

				var bookedAndPaidCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Count() })
					.ToList();

				var refundCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.NEED_TO_PAY && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Count() })
					.ToList();

				foreach (var item in mtList)
				{
					foreach (var bookedItem in bookedChannelCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.booked = bookedItem.Count;
						}
					}

					foreach (var refundItem in bookedAndRefundAmount)
					{
						if (item.Id == refundItem.ScheduleId)
						{
							item.totalRefund = refundItem.Total;
						}
					}

					foreach (var bookedItem in bookedAndPaidAmount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.totalPaidAmount = bookedItem.Total;
						}
					}

					foreach (var bookedItem in bookedAndPaidCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.paid = bookedItem.Count;
						}
					}

					foreach (var bookedAllAppinmentChanneltItem in bookedAllAppinmentChannelCount)
					{
						if (item.Id == bookedAllAppinmentChanneltItem.ScheduleId)
						{
							item.allBookedAppoinment = bookedAllAppinmentChanneltItem.Count;
						}
					}

					foreach (var bookedItem in refundCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.refund = bookedItem.Count;
						}
					}

					foreach (var totalAmount in totalAmountOfEachCashier)
					{
						if (item.Id == totalAmount.ScheduleId)
						{
							item.totalAmount = totalAmount.Total;
						}
					}
				}
			}

			return mtList;
		}

		public List<Model.ChannelingSchedule> SheduleGetBySelectedByConsultantID(DateTime dateTime, int cnsuID)
		{
			List<Model.ChannelingSchedule> mtList = new List<Model.ChannelingSchedule>();
			List<Model.OPD> mtOPDList = new List<Model.OPD>();
			List<Model.Invoice> mtInvoiceList = new List<Model.Invoice>();
			//   List<Model.InvoiceItem> mtInvoiceItemList = new List<Model.InvoiceItem>();
			using (DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
			{
				mtList = dbContext.ChannelingSchedule
					.Include(c => c.Room)
					.Include(c => c.Consultant)
					.Include(c => c.Consultant!.Specialist)
					.Include(c => c.Consultant)
					.Where(o => o.Status == Model.Enums.CommonStatus.Active && o.DateTime > dateTime && o.Consultant.SpecialistId == cnsuID)
					.OrderByDescending(o => o.DateTime)
					.ToList();

				var scheduleIds = mtList.Select(o => o.Id).ToList();
				mtOPDList = dbContext.OPD
					.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && scheduleIds.Contains(o.schedularId))
					.OrderByDescending(o => o.Id)
					.ToList();

				var opdIds = mtOPDList.Where(o => o.paymentStatus == PaymentStatus.PAID).Select(o => o.Id).ToList();
				mtInvoiceList = dbContext.Invoices
					 .Where(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID))
					 .OrderByDescending(o => o.Id)
					 .ToList();

				var invoiceIds = mtInvoiceList.Select(o => o.Id).ToList();
				var mtInvoiceItemList = dbContext.InvoiceItems
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus != ItemInvoiceStatus.Remove && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();
				var mtInvoiceRefundItemList = dbContext.InvoiceItems
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();

				foreach (var iId in mtInvoiceList)
				{

					foreach (var iItem in mtInvoiceItemList)
					{
						if (iItem.InvoiceId == iId.Id)
						{
							var _invoice = mtInvoiceList.First(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && o.Id == iItem.InvoiceId);
							var rowToUpdate = mtOPDList.FirstOrDefault(o => o.Id == _invoice.ServiceID);

							// Check if the row exists
							if (rowToUpdate != null)
							{
								rowToUpdate.TotalPaidAmount = iItem.Total;
							}

						}
					}

				}

				foreach (var iId in mtInvoiceList)
				{

					foreach (var iItem in mtInvoiceRefundItemList)
					{
						if (iItem.InvoiceId == iId.Id)
						{
							var _invoice = mtInvoiceList.First(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && o.Id == iItem.InvoiceId);
							var rowToUpdate = mtOPDList.FirstOrDefault(o => o.Id == _invoice.ServiceID);

							// Check if the row exists
							if (rowToUpdate != null)
							{
								rowToUpdate.TotalRefund = iItem.Total;
							}

						}
					}

				}


				// Get Total amount of Each consultant using OPD table
				var totalAmountOfEachCashier = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.ConsultantFee + o.HospitalFee + o.OtherFee) })
					.ToList();

				var bookedChannelCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Select(o => o.AppoimentNo).Distinct().Count() })
					.ToList();

				var bookedAllAppinmentChannelCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Select(o => o.AppoimentNo).Count() })
					.ToList();

				/*var bookedChannelCount = dbContext.OPD
                    .Where(o => o.invoiceType == InvoiceType.CHE)
                    .GroupBy(o => o.schedularId)
                    .Select(g => new { ScheduleId = g.Key, Count = g.Count() })
                    .ToList();*/
				var bookedAndRefundAmount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.TotalRefund) })
					.ToList();

				var bookedAndPaidAmount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.TotalPaidAmount) })
					.ToList();

				var bookedAndPaidCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Count() })
					.ToList();

				var refundCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.NEED_TO_PAY && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Count() })
					.ToList();

				foreach (var item in mtList)
				{
					foreach (var bookedItem in bookedChannelCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.booked = bookedItem.Count;
						}
					}

					foreach (var refundItem in bookedAndRefundAmount)
					{
						if (item.Id == refundItem.ScheduleId)
						{
							item.totalRefund = refundItem.Total;
						}
					}

					foreach (var bookedItem in bookedAndPaidAmount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.totalPaidAmount = bookedItem.Total;
						}
					}

					foreach (var bookedItem in bookedAndPaidCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.paid = bookedItem.Count;
						}
					}

					foreach (var bookedAllAppinmentChanneltItem in bookedAllAppinmentChannelCount)
					{
						if (item.Id == bookedAllAppinmentChanneltItem.ScheduleId)
						{
							item.allBookedAppoinment = bookedAllAppinmentChanneltItem.Count;
						}
					}

					foreach (var bookedItem in refundCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.refund = bookedItem.Count;
						}
					}

					foreach (var totalAmount in totalAmountOfEachCashier)
					{
						if (item.Id == totalAmount.ScheduleId)
						{
							item.totalAmount = totalAmount.Total;
						}
					}
				}
			}

			return mtList;
		}
		public List<Model.ChannelingSchedule> SheduleGetBySelectedByStatus(DateTime dateTime, ChannellingScheduleStatus channellingScheduleStatus)
		{
			List<Model.ChannelingSchedule> mtList = new List<Model.ChannelingSchedule>();
			List<Model.OPD> mtOPDList = new List<Model.OPD>();
			List<Model.Invoice> mtInvoiceList = new List<Model.Invoice>();
			//   List<Model.InvoiceItem> mtInvoiceItemList = new List<Model.InvoiceItem>();
			using (DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
			{
				mtList = dbContext.ChannelingSchedule
					.Include(c => c.Room)
					.Include(c => c.Consultant)
					.Include(c => c.Consultant!.Specialist)
					.Include(c => c.Consultant)
					.Where(o => o.Status == Model.Enums.CommonStatus.Active && o.DateTime > dateTime && o.scheduleStatus == channellingScheduleStatus)
					.OrderByDescending(o => o.DateTime)
					.ToList();

				var scheduleIds = mtList.Select(o => o.Id).ToList();
				mtOPDList = dbContext.OPD
					.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && scheduleIds.Contains(o.schedularId))
					.OrderByDescending(o => o.Id)
					.ToList();

				var opdIds = mtOPDList.Where(o => o.paymentStatus == PaymentStatus.PAID).Select(o => o.Id).ToList();
				mtInvoiceList = dbContext.Invoices
					 .Where(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID))
					 .OrderByDescending(o => o.Id)
					 .ToList();

				var invoiceIds = mtInvoiceList.Select(o => o.Id).ToList();
				var mtInvoiceItemList = dbContext.InvoiceItems
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus != ItemInvoiceStatus.Remove && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();
				var mtInvoiceRefundItemList = dbContext.InvoiceItems
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();

				foreach (var iId in mtInvoiceList)
				{

					foreach (var iItem in mtInvoiceItemList)
					{
						if (iItem.InvoiceId == iId.Id)
						{
							var _invoice = mtInvoiceList.First(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && o.Id == iItem.InvoiceId);
							var rowToUpdate = mtOPDList.FirstOrDefault(o => o.Id == _invoice.ServiceID);

							// Check if the row exists
							if (rowToUpdate != null)
							{
								rowToUpdate.TotalPaidAmount = iItem.Total;
							}

						}
					}

				}

				foreach (var iId in mtInvoiceList)
				{

					foreach (var iItem in mtInvoiceRefundItemList)
					{
						if (iItem.InvoiceId == iId.Id)
						{
							var _invoice = mtInvoiceList.First(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && o.Id == iItem.InvoiceId);
							var rowToUpdate = mtOPDList.FirstOrDefault(o => o.Id == _invoice.ServiceID);

							// Check if the row exists
							if (rowToUpdate != null)
							{
								rowToUpdate.TotalRefund = iItem.Total;
							}

						}
					}

				}


				// Get Total amount of Each consultant using OPD table
				var totalAmountOfEachCashier = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.ConsultantFee + o.HospitalFee + o.OtherFee) })
					.ToList();

				var bookedChannelCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Select(o => o.AppoimentNo).Distinct().Count() })
					.ToList();

				var bookedAllAppinmentChannelCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Select(o => o.AppoimentNo).Count() })
					.ToList();

				/*var bookedChannelCount = dbContext.OPD
                    .Where(o => o.invoiceType == InvoiceType.CHE)
                    .GroupBy(o => o.schedularId)
                    .Select(g => new { ScheduleId = g.Key, Count = g.Count() })
                    .ToList();*/
				var bookedAndRefundAmount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.TotalRefund) })
					.ToList();

				var bookedAndPaidAmount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.TotalPaidAmount) })
					.ToList();

				var bookedAndPaidCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Count() })
					.ToList();

				var refundCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.NEED_TO_PAY && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Count() })
					.ToList();

				foreach (var item in mtList)
				{
					foreach (var bookedItem in bookedChannelCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.booked = bookedItem.Count;
						}
					}

					foreach (var refundItem in bookedAndRefundAmount)
					{
						if (item.Id == refundItem.ScheduleId)
						{
							item.totalRefund = refundItem.Total;
						}
					}

					foreach (var bookedItem in bookedAndPaidAmount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.totalPaidAmount = bookedItem.Total;
						}
					}

					foreach (var bookedItem in bookedAndPaidCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.paid = bookedItem.Count;
						}
					}

					foreach (var bookedAllAppinmentChanneltItem in bookedAllAppinmentChannelCount)
					{
						if (item.Id == bookedAllAppinmentChanneltItem.ScheduleId)
						{
							item.allBookedAppoinment = bookedAllAppinmentChanneltItem.Count;
						}
					}

					foreach (var bookedItem in refundCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.refund = bookedItem.Count;
						}
					}

					foreach (var totalAmount in totalAmountOfEachCashier)
					{
						if (item.Id == totalAmount.ScheduleId)
						{
							item.totalAmount = totalAmount.Total;
						}
					}
				}
			}

			return mtList;
		}
		public List<Model.ChannelingSchedule> SheduleGetBySelectedByConsultantIDAndStatus(DateTime dateTime, int cnsuID, ChannellingScheduleStatus channellingScheduleStatus)
		{
			List<Model.ChannelingSchedule> mtList = new List<Model.ChannelingSchedule>();
			List<Model.OPD> mtOPDList = new List<Model.OPD>();
			List<Model.Invoice> mtInvoiceList = new List<Model.Invoice>();
			//   List<Model.InvoiceItem> mtInvoiceItemList = new List<Model.InvoiceItem>();
			using (DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
			{
				mtList = dbContext.ChannelingSchedule
					.Include(c => c.Room)
					.Include(c => c.Consultant)
					.Include(c => c.Consultant!.Specialist)
					.Include(c => c.Consultant)
					.Where(o => o.Status == Model.Enums.CommonStatus.Active && o.DateTime > dateTime && o.Consultant.SpecialistId == cnsuID && o.scheduleStatus == channellingScheduleStatus)
					.OrderByDescending(o => o.DateTime)
					.ToList();

				var scheduleIds = mtList.Select(o => o.Id).ToList();
				mtOPDList = dbContext.OPD
					.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && scheduleIds.Contains(o.schedularId))
					.OrderByDescending(o => o.Id)
					.ToList();

				var opdIds = mtOPDList.Where(o => o.paymentStatus == PaymentStatus.PAID).Select(o => o.Id).ToList();
				mtInvoiceList = dbContext.Invoices
					 .Where(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID))
					 .OrderByDescending(o => o.Id)
					 .ToList();

				var invoiceIds = mtInvoiceList.Select(o => o.Id).ToList();
				var mtInvoiceItemList = dbContext.InvoiceItems
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus != ItemInvoiceStatus.Remove && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();
				var mtInvoiceRefundItemList = dbContext.InvoiceItems
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();

				foreach (var iId in mtInvoiceList)
				{

					foreach (var iItem in mtInvoiceItemList)
					{
						if (iItem.InvoiceId == iId.Id)
						{
							var _invoice = mtInvoiceList.First(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && o.Id == iItem.InvoiceId);
							var rowToUpdate = mtOPDList.FirstOrDefault(o => o.Id == _invoice.ServiceID);

							// Check if the row exists
							if (rowToUpdate != null)
							{
								rowToUpdate.TotalPaidAmount = iItem.Total;
							}

						}
					}

				}

				foreach (var iId in mtInvoiceList)
				{

					foreach (var iItem in mtInvoiceRefundItemList)
					{
						if (iItem.InvoiceId == iId.Id)
						{
							var _invoice = mtInvoiceList.First(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && o.Id == iItem.InvoiceId);
							var rowToUpdate = mtOPDList.FirstOrDefault(o => o.Id == _invoice.ServiceID);

							// Check if the row exists
							if (rowToUpdate != null)
							{
								rowToUpdate.TotalRefund = iItem.Total;
							}

						}
					}

				}


				// Get Total amount of Each consultant using OPD table
				var totalAmountOfEachCashier = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.ConsultantFee + o.HospitalFee + o.OtherFee) })
					.ToList();

				var bookedChannelCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Select(o => o.AppoimentNo).Distinct().Count() })
					.ToList();

				var bookedAllAppinmentChannelCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.Status == CommonStatus.Active && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Select(o => o.AppoimentNo).Count() })
					.ToList();

				/*var bookedChannelCount = dbContext.OPD
                    .Where(o => o.invoiceType == InvoiceType.CHE)
                    .GroupBy(o => o.schedularId)
                    .Select(g => new { ScheduleId = g.Key, Count = g.Count() })
                    .ToList();*/
				var bookedAndRefundAmount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.TotalRefund) })
					.ToList();

				var bookedAndPaidAmount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new
					{ ScheduleId = g.Key, Total = g.Sum(o => o.TotalPaidAmount) })
					.ToList();

				var bookedAndPaidCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Count() })
					.ToList();

				var refundCount = mtOPDList
					.Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.NEED_TO_PAY && scheduleIds.Contains(o.schedularId))
					.GroupBy(o => o.schedularId)
					.Select(g => new { ScheduleId = g.Key, Count = g.Count() })
					.ToList();

				foreach (var item in mtList)
				{
					foreach (var bookedItem in bookedChannelCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.booked = bookedItem.Count;
						}
					}

					foreach (var refundItem in bookedAndRefundAmount)
					{
						if (item.Id == refundItem.ScheduleId)
						{
							item.totalRefund = refundItem.Total;
						}
					}

					foreach (var bookedItem in bookedAndPaidAmount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.totalPaidAmount = bookedItem.Total;
						}
					}

					foreach (var bookedItem in bookedAndPaidCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.paid = bookedItem.Count;
						}
					}

					foreach (var bookedAllAppinmentChanneltItem in bookedAllAppinmentChannelCount)
					{
						if (item.Id == bookedAllAppinmentChanneltItem.ScheduleId)
						{
							item.allBookedAppoinment = bookedAllAppinmentChanneltItem.Count;
						}
					}

					foreach (var bookedItem in refundCount)
					{
						if (item.Id == bookedItem.ScheduleId)
						{
							item.refund = bookedItem.Count;
						}
					}

					foreach (var totalAmount in totalAmountOfEachCashier)
					{
						if (item.Id == totalAmount.ScheduleId)
						{
							item.totalAmount = totalAmount.Total;
						}
					}
				}
			}

			return mtList;
		}

		public Model.ChannelingSchedule SheduleGetByConsultantIdandDate(int id, string date)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				DateTime DateTime = DateTime.Parse(date); // Convert string date to DateTime

				Model.ChannelingSchedule result = (from p in dbContext.ChannelingSchedule
												   where p.ConsultantId == id && p.DateTime == DateTime
												   select p).SingleOrDefault();
				result.Status = 0;
				dbContext.SaveChanges();
				return result;
			}
		}

		public List<Model.ChannelingSchedule> SheduleGetByConsultantId(int id)
		{
			List<Model.ChannelingSchedule> mtList = new List<Model.ChannelingSchedule>();
			DateTime currentTime = DateTime.Now; // Get the current time

			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				mtList = dbContext.ChannelingSchedule
					.Include(c => c.Consultant)
					.Include(c => c.Consultant.Specialist)
					.Where(o => o.ConsultantId == id &&
								o.scheduleStatus != ChannellingScheduleStatus.NOT_ACTIVE &&
								o.scheduleStatus != ChannellingScheduleStatus.SESSION_CANCEL &&
								o.scheduleStatus != ChannellingScheduleStatus.SESSION_END &&
								o.Status == CommonStatus.Active)
					.ToList();
			}

			return mtList;
		}

		public List<Model.ChannelingSchedule> GetAllSheduleGetByConsultantIdAndSessionStatus(int id,
			ChannellingScheduleStatus channellingScheduleStatus)
		{
			List<Model.ChannelingSchedule> mtList = new List<Model.ChannelingSchedule>();
			DateTime currentTime = DateTime.Now; // Get the current time

			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				if (channellingScheduleStatus == ChannellingScheduleStatus.ALL && id == -2)
				{
					mtList = dbContext.ChannelingSchedule
						.Include(c => c.Consultant)
						.Include(c => c.Consultant.Specialist)
						.Where(o => o.Status == CommonStatus.Active)
						.ToList();
				}
				else if (channellingScheduleStatus == ChannellingScheduleStatus.ALL && id != -2)
				{
					mtList = dbContext.ChannelingSchedule
						.Include(c => c.Consultant)
						.Include(c => c.Consultant.Specialist)
						.Where(o => o.ConsultantId == id && o.Status == CommonStatus.Active)
						.ToList();
				}
				else if (channellingScheduleStatus != ChannellingScheduleStatus.ALL && id == -2)
				{
					mtList = dbContext.ChannelingSchedule
						.Include(c => c.Consultant)
						.Include(c => c.Consultant.Specialist)
						.Where(o => o.scheduleStatus == channellingScheduleStatus && o.Status == CommonStatus.Active)
						.ToList();
				}
				else
				{
					mtList = dbContext.ChannelingSchedule
						.Include(c => c.Consultant)
						.Include(c => c.Consultant.Specialist)
						.Where(o => o.ConsultantId == id &&
									o.scheduleStatus == channellingScheduleStatus &&
									o.Status == CommonStatus.Active)
						.ToList();
				}
			}

			return mtList;
		}

		public List<Model.OPD> GetRefundedOpdList(DateTime startDate, DateTime endDate)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				// Get the list of OPD entries with specified conditions
				List<Model.OPD> mtOPDList = dbContext.OPD
					.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.DateTime >= startDate && o.DateTime <= endDate)
					.OrderByDescending(o => o.Id)
					.ToList();

				// Get the IDs of OPD entries that have a PAID payment status
				var opdIds = mtOPDList.Where(o => o.paymentStatus == PaymentStatus.PAID).Select(o => o.Id).ToList();

				// Get the list of invoices associated with the filtered OPD entries
				List<Model.Invoice> mtInvoiceList = dbContext.Invoices
					.Where(o => o.Status == 0 && o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID))
					.OrderByDescending(o => o.Id)
					.ToList();

				// Get the IDs of the filtered invoices
				var invoiceIds = mtInvoiceList.Select(o => o.Id).ToList();

				// Get the list of refunded invoice items related to hospital fees
				var refundedInvoiceItems = dbContext.InvoiceItems
					.Where(o => o.Status == 0 && o.itemInvoiceStatus == ItemInvoiceStatus.Remove && o.billingItemsType == BillingItemsType.Hospital && invoiceIds.Contains(o.InvoiceId))
					.ToList();

				// Get the OPD IDs of the refunded invoice items
				var refundedOPDIds = mtInvoiceList.Where(i => refundedInvoiceItems.Any(ii => ii.InvoiceId == i.Id)).Select(i => i.ServiceID).ToList();

				// Return the list of OPDs that have been refunded
				return mtOPDList.Where(opd => refundedOPDIds.Contains(opd.Id)).ToList();
			}
		}


		public int GetTotalRefundHospitalFeeCount(int id)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				List<Model.OPD> mtOPDList = new List<Model.OPD>();
				List<Model.Invoice> mtInvoiceList = new List<Model.Invoice>();

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


				int count = dbContext.InvoiceItems
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus == ItemInvoiceStatus.Remove && o.billingItemsType == BillingItemsType.Hospital && invoiceIds.Contains(o.InvoiceId))
					 .Count();

				return count;
			}
		}

		public int GetTotalRefundDoctorFeeCount(int id)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				List<Model.OPD> mtOPDList = new List<Model.OPD>();
				List<Model.Invoice> mtInvoiceList = new List<Model.Invoice>();

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


				int count = dbContext.InvoiceItems
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus == ItemInvoiceStatus.Remove && o.billingItemsType == BillingItemsType.Consultant && invoiceIds.Contains(o.InvoiceId))
					 .Count();

				return count;
			}
		}

		public decimal GetTotalRefundDoctorFeeAmount(int id, decimal hospitalFee)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				// get totalRefundDoctorFeeCount according to the schedularId
				List<Model.OPD> mtOPDList = new List<Model.OPD>();
				List<Model.Invoice> mtInvoiceList = new List<Model.Invoice>();

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
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus == ItemInvoiceStatus.Remove && o.billingItemsType == BillingItemsType.Consultant && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();


				decimal refundTotal = 0;
				if (mtInvoiceItemList != null && mtInvoiceItemList.Count > 0)
				{
					foreach (var invoice in mtInvoiceItemList)
					{
						decimal invT = invoice.Total != null ? invoice.Total : 0;
						refundTotal = refundTotal + invT;
					}

				}

				return refundTotal;


			}
		}

		public decimal GetTotalRefundHospitalFeeAmount(int id, decimal hospitalFee)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				List<Model.OPD> mtOPDList = new List<Model.OPD>();
				List<Model.Invoice> mtInvoiceList = new List<Model.Invoice>();

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
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus == ItemInvoiceStatus.Remove && o.billingItemsType == BillingItemsType.Hospital && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();

				decimal refundTotal = 0;
				if (mtInvoiceItemList != null && mtInvoiceItemList.Count > 0)
				{
					foreach (var invoice in mtInvoiceItemList)
					{
						decimal invT = invoice.Total != null ? invoice.Total : 0;
						refundTotal = refundTotal + invT;
					}

				}

				return refundTotal;

			}
		}

		public decimal GetTotalPaidHospitalFeeAmount(int id, decimal hospitalFee)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				List<Model.OPD> mtOPDList = new List<Model.OPD>();
				List<Model.Invoice> mtInvoiceList = new List<Model.Invoice>();

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
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus != ItemInvoiceStatus.Remove && o.billingItemsType == BillingItemsType.Hospital && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();


				decimal totalPaidHospitalFeeAmount = 0;
				if (mtInvoiceItemList != null && mtInvoiceItemList.Count > 0)
				{
					foreach (var invoice in mtInvoiceItemList)
					{
						decimal invT = invoice.Total != null ? invoice.Total : 0;
						totalPaidHospitalFeeAmount = totalPaidHospitalFeeAmount + invT;
					}

				}

				return totalPaidHospitalFeeAmount;
			}
		}

		public decimal GetTotalPaidDoctorFeeAmount(int id, decimal hospitalFee)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				List<Model.OPD> mtOPDList = new List<Model.OPD>();
				List<Model.Invoice> mtInvoiceList = new List<Model.Invoice>();

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
					 .Where(o => o.Status == 0 && o.itemInvoiceStatus != ItemInvoiceStatus.Remove && o.billingItemsType == BillingItemsType.Consultant && invoiceIds.Contains(o.InvoiceId))
					 .GroupBy(o => o.InvoiceId)
					 .Select(g => new { InvoiceId = g.Key, Total = g.Sum(o => o.price * o.qty) })
					 .ToList();


				decimal totalPaidDoctorFeeAmount = 0;
				if (mtInvoiceItemList != null && mtInvoiceItemList.Count > 0)
				{
					foreach (var invoice in mtInvoiceItemList)
					{
						decimal invT = invoice.Total != null ? invoice.Total : 0;
						totalPaidDoctorFeeAmount = totalPaidDoctorFeeAmount + invT;
					}

				}

				return totalPaidDoctorFeeAmount;
			}
		}

		public decimal GetTotalHospitalFeeAmount(int id, decimal hospitalFee)
		{
			/*decimal totalRefundHospitalFeeAmount = GetTotalRefundHospitalFeeAmount(id, hospitalFee);
            decimal totalPaidHospitalFeeAmount = GetTotalPaidHospitalFeeAmount(id, hospitalFee);

            decimal totalAmount = totalPaidHospitalFeeAmount - totalRefundHospitalFeeAmount;

            return totalAmount;*/

			return GetTotalPaidHospitalFeeAmount(id, hospitalFee);
		}

		public decimal GetTotalDoctorFeeAmount(int id, decimal hospitalFee)
		{
			/*decimal totalRefundDoctorFeeAmount = GetTotalRefundDoctorFeeAmount(id, hospitalFee);
            decimal totalPaidDoctorFeeAmount = GetTotalPaidDoctorFeeAmount(id, hospitalFee);

            decimal totalAmount = totalPaidDoctorFeeAmount - totalRefundDoctorFeeAmount;

            return totalAmount;*/

			return GetTotalPaidDoctorFeeAmount(id, hospitalFee);
		}

		public Model.ChannelingSchedule OnlyScheduleGetById(int id)
		{
			using var dbContext = new DataAccess.HospitalDBContext();

			var result = dbContext.ChannelingSchedule
				.Include(c => c.Consultant)
				.Include(c => c.Consultant.Specialist)
				.Include(c => c.Consultant)
				.Include(c => c.Room)
				.Where(o => o.Id == id)
				.SingleOrDefault();


			return result;
		}

        public decimal GetDoctorPaidAppoinment(int schedularID)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                // get totalRefundDoctorFeeCount according to the schedularId
                List<Model.OtherTransactions> mtOtherTransactionsList = new List<Model.OtherTransactions>();
                List<Model.Invoice> mtInvoiceList = new List<Model.Invoice>();

                mtOtherTransactionsList = dbContext.OtherTransactions
                    .Where(o => o.Status == 0 && o.SchedularId == schedularID  && o.InvoiceType == InvoiceType.DOCTOR_PAYMENT && o.otherTransactionsStatus == OtherTransactionsStatus.Cashier_Out)
                    .OrderByDescending(o => o.Id)
                    .ToList();
                var otherIds = mtOtherTransactionsList.Select(o => o.Id).ToList();

                mtInvoiceList = dbContext.Invoices
                     .Where(o => o.Status == 0 && o.InvoiceType == InvoiceType.DOCTOR_PAYMENT && otherIds.Contains(o.ServiceID))
                     .OrderByDescending(o => o.Id)
                     .ToList();

                List<int> invoiceIds = mtInvoiceList.Select(o => o.Id).ToList();

                decimal totalPaidAppoiment = dbContext.Payments.Where(o => o.BillingType == BillingType.OTHER_OUT  && invoiceIds.Contains(o.InvoiceID))
                                .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                return totalPaidAppoiment;


            }
        }



        public Model.ChannelingSchedule SheduleGetById(int id)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				// get number of patients according to the schedularId
				int patientCount = dbContext.OPD
					.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.schedularId == id)
					.GroupBy(o => o.schedularId)
					.Select(g => g.Count())
					.SingleOrDefault();

				// get Actual patient count according to the schedularId
				int actualPatientCount = dbContext.OPD
					.Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.paymentStatus != PaymentStatus.NOT_PAID &&
								o.schedularId == id)
					.GroupBy(o => o.schedularId)
					.Select(g => g.Count())
					.SingleOrDefault();

				Model.ChannelingSchedule result = dbContext.ChannelingSchedule
					.Include(c => c.Consultant)
					.Include(c => c.Consultant.Specialist)
					.Include(c => c.Consultant)
					.Include(c => c.Room)
					.Where(o => o.Id == id)
					.SingleOrDefault();

				result.Status = 0;
				result.patientCount = patientCount;
				result.totalPatientCount = actualPatientCount;
				result.totalRefundHospitalFeeCount = GetTotalRefundHospitalFeeCount(id);
				result.totalRefundDoctorFeeCount = GetTotalRefundDoctorFeeCount(id);
				result.totalRefundDoctorFeeAmount = GetTotalRefundDoctorFeeAmount(id, result.HospitalFee);
				result.totalRefundHospitalFeeAmount = GetTotalRefundHospitalFeeAmount(id, result.HospitalFee);
				result.totalHospitalFeeAmount = GetTotalHospitalFeeAmount(id, result.HospitalFee);
				result.totalDoctorFeeAmount = GetTotalDoctorFeeAmount(id, result.HospitalFee);
                result.doctorPaidAppoinment = GetDoctorPaidAppoinment(id);
				result.actualPatientCount = result.totalPatientCount - result.totalRefundDoctorFeeCount;
                dbContext.SaveChanges();

				return result;
			}
		}

		public Model.Scan GetChannelingItemById(int id)
		{
			using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
			{
				Model.Scan result = (from p in dbContext.ChannelingItems where p.Id == id select p).SingleOrDefault();
				return result;
			}
		}

		public HospitalMgrSystem.Model.ChannelingSchedule DeleteChannelingShedule(int id)
		{
			using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext =
				   new HospitalMgrSystem.DataAccess.HospitalDBContext())
			{
				HospitalMgrSystem.Model.ChannelingSchedule result =
					(from p in dbContext.ChannelingSchedule where p.Id == id select p).SingleOrDefault();
				result.Status = CommonStatus.Inactive;
				dbContext.SaveChanges();
				return result;
			}
		}
	}
}