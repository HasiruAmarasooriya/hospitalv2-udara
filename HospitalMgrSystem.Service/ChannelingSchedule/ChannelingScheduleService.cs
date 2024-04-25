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

        public int GetTotalRefundHospitalFeeCount(int id, decimal hospitalFee)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                // get totalRefundDoctorFeeCount according to the schedularId
                List<int> opdIds = dbContext.OPD
                    .Where(o => o.schedularId == id)
                    .Select(o => o.Id)
                    .ToList();

                List<int> invoicesIds = dbContext.Invoices
                    .Where(o => opdIds.Contains(o.ServiceID))
                    .Select(o => o.Id)
                    .ToList();

                int count = dbContext.InvoiceItems
                    .Where(o => invoicesIds.Contains(o.InvoiceId) && o.price == hospitalFee &&
                                o.itemInvoiceStatus == ItemInvoiceStatus.Remove)
                    .Count();

                return count;
            }
        }

        public int GetTotalRefundDoctorFeeCount(int id, decimal hospitalFee)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                // get totalRefundDoctorFeeCount according to the schedularId
                List<int> opdIds = dbContext.OPD
                    .Where(o => o.schedularId == id)
                    .Select(o => o.Id)
                    .ToList();

                List<int> invoicesIds = dbContext.Invoices
                    .Where(o => opdIds.Contains(o.ServiceID))
                    .Select(o => o.Id)
                    .ToList();

                int count = dbContext.InvoiceItems
                    .Where(o => invoicesIds.Contains(o.InvoiceId) && o.price != hospitalFee &&
                                o.itemInvoiceStatus == ItemInvoiceStatus.Remove)
                    .Count();

                return count;
            }
        }

        public decimal GetTotalRefundDoctorFeeAmount(int id, decimal hospitalFee)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                // get totalRefundDoctorFeeCount according to the schedularId
                List<int> opdIds = dbContext.OPD
                    .Where(o => o.schedularId == id)
                    .Select(o => o.Id)
                    .ToList();

                List<int> invoicesIds = dbContext.Invoices
                    .Where(o => opdIds.Contains(o.ServiceID))
                    .Select(o => o.Id)
                    .ToList();

                decimal totalAmount = dbContext.InvoiceItems
                    .Where(o => invoicesIds.Contains(o.InvoiceId) && o.price != hospitalFee &&
                                o.itemInvoiceStatus == ItemInvoiceStatus.Remove)
                    .Sum(o => o.price);

                if (totalAmount == null)
                {
                    return 0;
                }

                return totalAmount;
            }
        }

        public decimal GetTotalRefundHospitalFeeAmount(int id, decimal hospitalFee)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                // get totalRefundDoctorFeeCount according to the schedularId
                List<int> opdIds = dbContext.OPD
                    .Where(o => o.schedularId == id)
                    .Select(o => o.Id)
                    .ToList();

                List<int> invoicesIds = dbContext.Invoices
                    .Where(o => opdIds.Contains(o.ServiceID))
                    .Select(o => o.Id)
                    .ToList();

                decimal totalAmount = dbContext.InvoiceItems
                    .Where(o => invoicesIds.Contains(o.InvoiceId) && o.price == hospitalFee &&
                                o.itemInvoiceStatus == ItemInvoiceStatus.Remove)
                    .Sum(o => o.price);

                if (totalAmount == null)
                {
                    return 0;
                }

                return totalAmount;
            }
        }

        public decimal GetTotalPaidHospitalFeeAmount(int id, decimal hospitalFee)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                List<int> opdIds = dbContext.OPD
                    .Where(o => o.schedularId == id)
                    .Select(o => o.Id)
                    .ToList();

                List<int> invoicesIds = dbContext.Invoices
                    .Where(o => opdIds.Contains(o.ServiceID))
                    .Select(o => o.Id)
                    .ToList();

                decimal totalPaidHospitalFeeAmount = dbContext.InvoiceItems
                    .Where(o => invoicesIds.Contains(o.InvoiceId) && o.price == hospitalFee &&
                                o.itemInvoiceStatus == ItemInvoiceStatus.BILLED)
                    .Sum(o => o.price);

                if (totalPaidHospitalFeeAmount == null)
                {
                    return 0;
                }

                return totalPaidHospitalFeeAmount;
            }
        }

        public decimal GetTotalPaidDoctorFeeAmount(int id, decimal hospitalFee)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                List<int> opdIds = dbContext.OPD
                    .Where(o => o.schedularId == id)
                    .Select(o => o.Id)
                    .ToList();

                List<int> invoicesIds = dbContext.Invoices
                    .Where(o => opdIds.Contains(o.ServiceID))
                    .Select(o => o.Id)
                    .ToList();

                decimal totalPaidDoctorFeeAmount = dbContext.InvoiceItems
                    .Where(o => invoicesIds.Contains(o.InvoiceId) && o.price != hospitalFee &&
                                o.itemInvoiceStatus == ItemInvoiceStatus.BILLED)
                    .Sum(o => o.price);

                if (totalPaidDoctorFeeAmount == null)
                {
                    return 0;
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

        public Model.ChannelingSchedule SheduleGetById(int id)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                // get number of patients according to the schedularId
                int patientCount = dbContext.OPD
                    .Where(o => o.invoiceType == InvoiceType.CHE && o.schedularId == id)
                    .GroupBy(o => o.schedularId)
                    .Select(g => g.Count())
                    .SingleOrDefault();

                // get Actual patient count according to the schedularId
                int actualPatientCount = dbContext.OPD
                    .Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus != PaymentStatus.NOT_PAID &&
                                o.schedularId == id)
                    .GroupBy(o => o.schedularId)
                    .Select(g => g.Count())
                    .SingleOrDefault();

                Model.ChannelingSchedule result = dbContext.ChannelingSchedule
                    .Include(c => c.Consultant)
                    .Include(c => c.Consultant.Specialist)
                    .Include(c => c.Consultant)
                    .Where(o => o.Id == id)
                    .SingleOrDefault();

                result.Status = 0;
                result.patientCount = patientCount;
                result.actualPatientCount = actualPatientCount;
                result.totalRefundHospitalFeeCount = GetTotalRefundHospitalFeeCount(id, result.HospitalFee);
                result.totalRefundDoctorFeeCount = GetTotalRefundDoctorFeeCount(id, result.HospitalFee);
                result.totalRefundDoctorFeeAmount = GetTotalRefundDoctorFeeAmount(id, result.HospitalFee);
                result.totalRefundHospitalFeeAmount = GetTotalRefundHospitalFeeAmount(id, result.HospitalFee);
                result.totalHospitalFeeAmount = GetTotalHospitalFeeAmount(id, result.HospitalFee);
                result.totalDoctorFeeAmount = GetTotalDoctorFeeAmount(id, result.HospitalFee);

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