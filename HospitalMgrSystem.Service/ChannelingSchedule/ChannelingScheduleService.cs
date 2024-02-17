using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
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
                    Model.ChannelingSchedule result = (from p in dbContext.ChannelingSchedule where p.Id == channelingSchedule.Id select p).SingleOrDefault();
                    
                    if (channelingSchedule.NoOfAppointment >= result.NoOfAppointment)
                    {
                        result.NoOfAppointment = channelingSchedule.NoOfAppointment;
                        result.Status = channelingSchedule.Status;
                        result.scheduleStatus = channelingSchedule.scheduleStatus;

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
                     .Include(c => c.Consultant)
                     .Include(c => c.Consultant.Specialist)
                     .Include(c => c.Consultant)
                    .Where(o => o.Status == 0)
                    .OrderByDescending(o => o.DateTime)
                    .ToList();

                var bookedChannelCount = dbContext.OPD
                    .Where(o => o.invoiceType == InvoiceType.CHE)
                    .GroupBy(o => o.schedularId)
                    .Select(g => new { ScheduleId = g.Key, Count = g.Count() })
                    .ToList();

                var bookedAndPaidCount = dbContext.OPD
                    .Where(o => o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID)
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
                }

            }
            return mtList;
        }

        public HospitalMgrSystem.Model.ChannelingSchedule SheduleGetByConsultantIdandDate(int id, string date)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                DateTime DateTime = DateTime.Parse(date); // Convert string date to DateTime

                HospitalMgrSystem.Model.ChannelingSchedule result = (from p in dbContext.ChannelingSchedule
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
                    .Where(o => o.ConsultantId == id && o.DateTime >= currentTime)
                    .ToList();
            }

            return mtList;
        }

        public HospitalMgrSystem.Model.ChannelingSchedule SheduleGetById(int id)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {


                HospitalMgrSystem.Model.ChannelingSchedule result = (from p in dbContext.ChannelingSchedule.
                                                                     Include(c => c.Consultant)
                                                                     where p.Id == id
                                                                     select p).SingleOrDefault();
                result.Status = 0;
                dbContext.SaveChanges();
                return result;
            }
        }

        public HospitalMgrSystem.Model.ChannelingSchedule DeleteChannelingShedule(int id)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.ChannelingSchedule result = (from p in dbContext.ChannelingSchedule where p.Id == id select p).SingleOrDefault();
                result.Status = CommonStatus.Inactive;
                dbContext.SaveChanges();
                return result;
            }
        }
    }
}
