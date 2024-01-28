using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.CashierSession
{
    public class CashierSessionService
    {

        public Model.CashierSession CreateCashierSession(Model.CashierSession cashierSession)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (cashierSession.Id == 0)
                {
                    dbContext.CashierSessions.Add(cashierSession);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.CashierSession result = (from p in dbContext.CashierSessions where p.Id == cashierSession.Id select p).SingleOrDefault();
                    result.EndBalence = cashierSession.EndBalence;
                    result.EndTime = cashierSession.EndTime;
                    result.cashierSessionStatus = cashierSession.cashierSessionStatus;
                    result.ModifiedUser = cashierSession.ModifiedUser;
                    result.ModifiedDate = cashierSession.ModifiedDate;



                    dbContext.SaveChanges();
                }
                return cashierSession;
            }
        }

        public List<Model.CashierSession> GetAllCashierSession()
        {
            List<Model.CashierSession> mtList = new List<Model.CashierSession>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.CashierSessions.Include(c => c.User).Where(o => o.Status == 0).OrderByDescending(o => o.Id).ToList<Model.CashierSession>();

            }
            return mtList;
        }

        public Model.CashierSession GetCashierSessionByID(int id)
        {


            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.CashierSession result = dbContext.CashierSessions.Include(c => c.User).Where(o => o.Status == 0 && o.Id == id).SingleOrDefault();
                return result;
            }
        }


        public List<Model.CashierSession> GetACtiveCashierSessions(int id)
        {
            List<Model.CashierSession> mtList = new List<Model.CashierSession>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.CashierSessions.Include(c => c.User).Where(o => o.Status == 0 && o.cashierSessionStatus == Model.Enums.CashierSessionStatus.START && o.userID == id).OrderByDescending(o => o.Id).ToList<Model.CashierSession>();

            }
            return mtList;
        }
    }
}
