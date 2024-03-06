using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using HospitalMgrSystem.Model;

namespace HospitalMgrSystem.Service.Channeling
{
    public class ChannelingService : IChannelingService
    {
        public List<Scan> LoadChannelingItems()
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                return dbContext.ChannelingItems.ToList();
            }
        }

        public Model.Channeling CreateChanneling(Model.Channeling channeling)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (channeling.Id == 0)
                {
                    dbContext.Channels.Add(channeling);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.Channeling result = (from p in dbContext.Channels where p.Id == channeling.Id select p).SingleOrDefault();
                    result.PatientID = channeling.PatientID;
                    result.ChannelingScheduleID = channeling.ChannelingScheduleID;
                    result.AppointmentNo = channeling.AppointmentNo;
                    result.Status = channeling.Status;
                    result.CreateDate = channeling.CreateDate;
                    result.ModifiedDate = channeling.ModifiedDate;



                    dbContext.SaveChanges();
                }
                return null;
            }
        }

        public List<Model.OPD> GetAllChannelingByDateTime(DateTime startDate, DateTime endDate)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule.ToList();

                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.consultant.Specialist)
                    .Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.DateTime >= startDate && o.DateTime <= endDate)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                for (int i = 0; i < mtList.Count; i++)
                {
                    mtList[i].channelingScheduleData = schedularIdList.Where(o => o.Id == mtList[i].schedularId).SingleOrDefault();
                }

            }
            return mtList;
        }

        public List<Model.Specialist> GetAllSpecialists()
        {
            List<Model.Specialist> mtList = new List<Model.Specialist>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Specialists.ToList();
            }
            return mtList;
        }

        public List<Model.OPD> GetAllChannelingByDoctorSpeciality(DateTime startDate, DateTime endDate, int specialistId)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule.ToList();

                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.consultant.Specialist)
                    .Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.consultant.SpecialistId == specialistId && o.DateTime >= startDate && o.DateTime <= endDate)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                for (int i = 0; i < mtList.Count; i++)
                {
                    mtList[i].channelingScheduleData = schedularIdList.Where(o => o.Id == mtList[i].schedularId).SingleOrDefault();
                }

            }
            return mtList;
        }

        public List<Model.OPD> GetAllChannelingByAllFilters(DateTime startDate, DateTime endDate, int specialistId, PaymentStatus paymentStatus, ChannellingScheduleStatus channellingScheduleStatus)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule.Where(o => o.scheduleStatus == channellingScheduleStatus).ToList();

                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.consultant.Specialist)
                    .Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.consultant.SpecialistId == specialistId && o.paymentStatus == paymentStatus && o.DateTime >= startDate && o.DateTime <= endDate)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                for (int i = 0; i < mtList.Count; i++)
                {
                    mtList[i].channelingScheduleData = schedularIdList.Where(o => o.Id == mtList[i].schedularId).SingleOrDefault();
                }

            }
            return mtList;
        }

        public List<Model.OPD> GetAllChannelingByPaymentStatusAndChannelingScheduleStatus(DateTime startDate, DateTime endDate, PaymentStatus paymentStatus, ChannellingScheduleStatus channellingScheduleStatus)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule.Where(o => o.scheduleStatus == channellingScheduleStatus).ToList();

                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.consultant.Specialist)
                    .Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.paymentStatus == paymentStatus && o.DateTime >= startDate && o.DateTime <= endDate)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                for (int i = 0; i < mtList.Count; i++)
                {
                    mtList[i].channelingScheduleData = schedularIdList.Where(o => o.Id == mtList[i].schedularId).SingleOrDefault();
                }

            }
            return mtList;
        }

        public List<Model.OPD> GetAllChannelingByDoctorSpecialityAndScheduleStatus(DateTime startDate, DateTime endDate, int specialistId, ChannellingScheduleStatus channellingScheduleStatus)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule.Where(o => o.scheduleStatus == channellingScheduleStatus).ToList();

                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.consultant.Specialist)
                    .Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.consultant.SpecialistId == specialistId && o.DateTime >= startDate && o.DateTime <= endDate)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                for (int i = 0; i < mtList.Count; i++)
                {
                    mtList[i].channelingScheduleData = schedularIdList.Where(o => o.Id == mtList[i].schedularId).SingleOrDefault();
                }

            }
            return mtList;
        }

        public List<Model.OPD> GetAllChannelingBySpecialistIdAndPaymentStatus(DateTime startDate, DateTime endDate, PaymentStatus paymentStatus, int specialistId)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                var channelingScheduleIds = dbContext.ChannelingSchedule
                    .Select(o => o.Id)
                    .ToList();

                List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule.ToList();

                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.consultant.Specialist)
                    .Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.DateTime >= startDate && o.DateTime <= endDate && o.consultant.SpecialistId == specialistId && o.paymentStatus == paymentStatus && channelingScheduleIds.Contains(o.schedularId))
                    .OrderByDescending(o => o.Id)
                    .ToList();

                for (int i = 0; i < mtList.Count; i++)
                {
                    mtList[i].channelingScheduleData = schedularIdList.Where(o => o.Id == mtList[i].schedularId).SingleOrDefault();
                }

            }
            return mtList;
        }

        public List<Model.OPD> GetAllChannelingByPaymentStatus(DateTime startDate, DateTime endDate, PaymentStatus paymentStatus)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                var channelingScheduleIds = dbContext.ChannelingSchedule
                    .Select(o => o.Id)
                    .ToList();

                List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule.ToList();

                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.consultant.Specialist)
                    .Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == paymentStatus && channelingScheduleIds.Contains(o.schedularId))
                    .OrderByDescending(o => o.Id)
                    .ToList();

                for (int i = 0; i < mtList.Count; i++)
                {
                    mtList[i].channelingScheduleData = schedularIdList.Where(o => o.Id == mtList[i].schedularId).SingleOrDefault();
                }

            }
            return mtList;
        }

        public List<Model.OPD> GetAllChannelingByChannelingScheduleStatus(DateTime startDate, DateTime endDate, ChannellingScheduleStatus channellingScheduleStatus)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                var channelingScheduleIds = dbContext.ChannelingSchedule
                    .Where(o => o.scheduleStatus == channellingScheduleStatus)
                    .Select(o => o.Id)
                    .ToList();

                List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule.ToList();

                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.consultant.Specialist)
                    .Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE && o.DateTime >= startDate && o.DateTime <= endDate && channelingScheduleIds.Contains(o.schedularId))
                    .OrderByDescending(o => o.Id)
                    .ToList();

                for (int i = 0; i < mtList.Count; i++)
                {
                    mtList[i].channelingScheduleData = schedularIdList.Where(o => o.Id == mtList[i].schedularId).SingleOrDefault();
                }

            }
            return mtList;
        }

        public List<Model.OPD> GetAllChannelingByStatus()
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule.ToList();

                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.consultant.Specialist)
                    .Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                for (int i = 0; i < mtList.Count; i++)
                {
                    mtList[i].channelingScheduleData = schedularIdList.Where(o => o.Id == mtList[i].schedularId).SingleOrDefault();
                }

            }
            return mtList;
        }



        public List<Model.OPD> ChannelingGetBySheduleId(int id)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD.Where(o => o.schedularId == id && o.Status == 0 && o.invoiceType == InvoiceType.CHE).ToList();

            }
            return mtList;
        }

        public Model.OPD DeleteChanneling(int id)
        {


            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.OPD result = (from p in dbContext.OPD where p.Id == id select p).SingleOrDefault();
                result.Status = CommonStatus.Inactive;
                dbContext.SaveChanges();
                return result;
            }
        }
        public Model.Channeling GetChannelByID(int id)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                //result = (from p in dbContext.Channels where p.Id == id select p).SingleOrDefault();
                HospitalMgrSystem.Model.Channeling result = dbContext.Channels.Include(c => c.Patient).Include(c => c.ChannelingSchedule.Consultant).Where(o => o.Status == 0 && o.Id == id).SingleOrDefault();
                return result;
            }
        }

        public Model.OPD GetChannelByIDNew(int id)
        {
            Model.OPD opd = new Model.OPD();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                opd = dbContext.OPD
             .Include(o => o.patient) // Load the Patient related to OPD
             .Include(o => o.consultant) // Load the Consultant related to OPD
             .SingleOrDefault(o => o.Id == id);

            }
            return opd;
        }

    }
}
