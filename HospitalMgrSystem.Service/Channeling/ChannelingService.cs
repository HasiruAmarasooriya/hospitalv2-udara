using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace HospitalMgrSystem.Service.Channeling
{
    public class ChannelingService : IChannelingService
    {
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

        public List<Model.OPD> GetAllChannelingByStatus()
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD.Include(c => c.patient).Include(c => c.consultant).Include(c => c.room).Where(o => o.Status == 0 && o.invoiceType == InvoiceType.CHE).OrderByDescending(o => o.Id).ToList();

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

        public Model.Channeling DeleteChanneling(int id)
        {


            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.Channeling result = (from p in dbContext.Channels where p.Id == id select p).SingleOrDefault();
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
