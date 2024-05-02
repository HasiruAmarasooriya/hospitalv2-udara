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
                    int refundItem = 0;
                    mtList[i].channelingScheduleData = schedularIdList.Where(o => o.Id == mtList[i].schedularId).SingleOrDefault();
                    if(mtList[i].paymentStatus == PaymentStatus.PAID)
                    {
                        //get invoice by opd id
                        Invoice invoiceObj = new Invoice();
                        invoiceObj = GetInvoiceByServiceIDAndInvoiceType(mtList[i].Id, InvoiceType.CHE);
                        //get invoice item by invoice id
                        List<Model.InvoiceItem> mtListInvoiceItem = new List<Model.InvoiceItem>();
                        mtListInvoiceItem = GetInvoiceItemByInvoicedID(invoiceObj.Id);
                        //check invoice item refund or not
                        if (mtListInvoiceItem != null && mtListInvoiceItem.Count > 0)
                        {
                            for (int y = 0; y < mtListInvoiceItem.Count; y++)
                            {
                                if (mtListInvoiceItem[y].itemInvoiceStatus == ItemInvoiceStatus.Remove)
                                {
                                    refundItem = 1;
                                    if (y == 0)
                                    {
                                        mtList[i].refundedItem = mtList[i].refundedItem + "(" + mtListInvoiceItem[y].billingItemsType.ToString();
                                    }
                                    else
                                    {
                                        if(y == mtListInvoiceItem.Count - 1)
                                        {
                                            mtList[i].refundedItem = mtList[i].refundedItem + "," + mtListInvoiceItem[y].billingItemsType.ToString() + ")";
                                        }
                                        else
                                        {
                                            mtList[i].refundedItem = mtList[i].refundedItem + "," + mtListInvoiceItem[y].billingItemsType.ToString();
                                        }
                                       
                                    }
                                   
                                }
                            }
                            mtList[i].isRefund =refundItem;
                        }
                        else
                        {
                            mtList[i].refundedItem ="-";
                        }
                    }

                }

            }
            return mtList;
        }



        public List<Model.OPD> ChannelingGetBySheduleId(int id)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                var channelingScheduleIds = dbContext.ChannelingSchedule
                                            .Where(o => o.Status == CommonStatus.Active)
                                            .Select(o => o.Id)
                                            .ToList();

                List<Model.ChannelingSchedule> schedularIdList = dbContext.ChannelingSchedule.ToList();
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.consultant.Specialist)
                    .Where(o => o.schedularId == id && o.Status == 0 && o.invoiceType == InvoiceType.CHE).ToList();

                for (int i = 0; i < mtList.Count; i++)
                {
                    mtList[i].channelingScheduleData = schedularIdList.Where(o => o.Id == mtList[i].schedularId).SingleOrDefault();
                }

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

        public List<Model.Consultant> GetAllConsultantThatHaveSchedulingsByDate(DateTime scheduleDate)
        {
            List<Model.Consultant> mtList = new List<Model.Consultant>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                // Get doctor ids which have schedulings
                var doctorIds = dbContext.ChannelingSchedule
                    .Where(x => x.Status == Model.Enums.CommonStatus.Active && x.DateTime > scheduleDate)
                    .Select(x => x.ConsultantId)
                    .Distinct()
                    .ToList();


                mtList = dbContext.Consultants
                    .Include(c => c.Specialist)
                    .Where(o => o.Status == 0 && doctorIds.Contains(o.Id))
                    .ToList();
            }

            return mtList;
        }

        public Model.Invoice GetInvoiceByServiceIDAndInvoiceType(int serviceID, InvoiceType invoiceType)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.Invoice result = (from p in dbContext.Invoices where p.ServiceID == serviceID && p.InvoiceType == invoiceType select p).SingleOrDefault();
                return result;
            }

        }

        public List<Model.InvoiceItem> GetInvoiceItemByInvoicedID(int invoiceID)
        {

            List<Model.InvoiceItem> mtList = new List<Model.InvoiceItem>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.InvoiceItems.Where(c => c.InvoiceId == invoiceID).ToList<Model.InvoiceItem>();

            }
            return mtList;
        }
    }
}
