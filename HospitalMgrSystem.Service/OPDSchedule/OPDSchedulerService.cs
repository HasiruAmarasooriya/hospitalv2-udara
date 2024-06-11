using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Service.OPDSchedule
{
    public class OPDSchedulerService : IOPDSchedulerService
    {
        public Model.OPDScheduler CreateOPDSchedule(Model.OPDScheduler oPDScheduler)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (oPDScheduler.Id == 0)
                {
                    dbContext.OPDScheduler.Add(oPDScheduler);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.OPDScheduler result = (from p in dbContext.OPDScheduler where p.Id == oPDScheduler.Id select p).SingleOrDefault();
                    result.Id = oPDScheduler.Id;
                    result.ConsultantId = oPDScheduler.ConsultantId;
                    result.OPDSchedulerStatus = oPDScheduler.OPDSchedulerStatus;
                    result.OPDSession = oPDScheduler.OPDSession;
                    result.startTime = oPDScheduler.startTime;
                    result.endTime = oPDScheduler.endTime;
                    result.isActiveSession = oPDScheduler.isActiveSession;
                    result.isPaid = oPDScheduler.isPaid;
                    result.Status = oPDScheduler.Status;
                    result.ModifiedUser = oPDScheduler.ModifiedUser;
                    result.ModifiedDate = oPDScheduler.ModifiedDate;
                    dbContext.SaveChanges();



                }
                return dbContext.OPDScheduler.Find(oPDScheduler.Id);
            }
        }

        public Model.OPDScheduler GetOPDSheduleByID(int? id)
        {
            Model.OPDScheduler oPDScheduler = new Model.OPDScheduler();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                oPDScheduler = dbContext.OPDScheduler
                               .Include(o => o.Consultant) // Load the Consultant related to OPD
                               .SingleOrDefault(o => o.Id == id);

            }
            return oPDScheduler;
        }


        public void UpdateIsActive()
        {
            List<Model.OPDScheduler> mtList = new List<Model.OPDScheduler>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                    mtList = dbContext.OPDScheduler
                        .Include(o => o.Consultant)
                        .Where(o => o.Status == Model.Enums.CommonStatus.Active && o.isActiveSession == 1)
                        .ToList();

                if(mtList.Count > 0)
                {
                    foreach (OPDScheduler oPDScheduler in mtList)
                    {
                        HospitalMgrSystem.Model.OPDScheduler result = (from p in dbContext.OPDScheduler where p.Id == oPDScheduler.Id select p).SingleOrDefault();

                        result.OPDSchedulerStatus = OPDScheduleStatus.Inactive;
                        result.endTime =DateTime.Now;
                        result.isActiveSession = 0;
                        result.ModifiedUser = oPDScheduler.ModifiedUser;
                        result.ModifiedDate = oPDScheduler.ModifiedDate;
                        dbContext.SaveChanges();

                    }
                }



            }

        }

        public List<Model.OPDScheduler> GetAllOPDSchedulerDByStatus()
        {
            List<Model.OPDScheduler> mtList = new List<Model.OPDScheduler>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPDScheduler.Include(c => c.Consultant).Where(o => o.Status == 0).OrderByDescending(o => o.Id).ToList<Model.OPDScheduler>();

            }
            return mtList;
        }

        public List<Model.OPDScheduler> GetOPDSchedulersByCurrentDate(DateTime cDate)
        {
            List<Model.OPDScheduler> opdSchedulers = new List<Model.OPDScheduler>();

            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                DateTime currentDate = cDate;
                opdSchedulers = dbContext.OPDScheduler
                    .Include(o => o.Consultant)
                    .Where(o => o.cDate == currentDate && o.Status == Model.Enums.CommonStatus.Active)
                    .ToList();
            }

            return opdSchedulers;
        }


        public List<Model.OPDScheduler> GetTodayActiveOPDSchedulers()
        {
            List<Model.OPDScheduler> opdSchedulers = new List<Model.OPDScheduler>();

            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                DateTime currentDate = DateTime.Now.Date;
                opdSchedulers = dbContext.OPDScheduler
                    .Include(o => o.Consultant)
                    .Where(o => o.cDate == currentDate && o.Status == Model.Enums.CommonStatus.Active && o.isActiveSession == 1)
                    .ToList();
            }

            return opdSchedulers;
        }

        public List<Model.OPDScheduler> GetActiveOPDSchedulers()
        {
            List<Model.OPDScheduler> opdSchedulers = new List<Model.OPDScheduler>();

            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                opdSchedulers = dbContext.OPDScheduler
                    .Include(o => o.Consultant)
                    .Where(o =>  o.Status == Model.Enums.CommonStatus.Active && o.isActiveSession == 1)
                    .ToList();
            }

            return opdSchedulers;
        }

    }
}
