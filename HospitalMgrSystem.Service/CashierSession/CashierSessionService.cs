using HospitalMgrSystem.Model.Enums;
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
                    result.col1 = cashierSession.col1;
                    result.col2 = cashierSession.col2;
                    result.col3 = cashierSession.col3;
                    result.col4 = cashierSession.col4;
                    result.col5 = cashierSession.col5;
                    result.col6 = cashierSession.col6;
                    result.col7 = cashierSession.col7;
                    result.col8 = cashierSession.col8;
                    result.col9 = cashierSession.col9;
                    result.col10 = cashierSession.col10;

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

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.CashierSessions.Include(c => c.User).Where(o => o.Status == 0).OrderByDescending(o => o.Id).ToList<Model.CashierSession>();
            
                // Get sum of all payments as Total
                var paymentsData = dbContext.Payments
                    .GroupBy(c => c.sessionID)
                    .Select(g => new
                    {
                        sessionID = g.Key,
                        Total = g.Sum(c => c.CashAmount)
                    }).ToList();

                foreach (var item in mtList)
                {
                    foreach (var payment in paymentsData)
                    {
                        if (item.Id == payment.sessionID)
                        {
                            item.TotalAmount = payment.Total;
                        }
                    }
                }
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

        public List<Model.CashierSession> GetAllNightsiftActiveCashierSession()
        {
            List<Model.CashierSession> mtList = new List<Model.CashierSession>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.CashierSessions.Include(c => c.User).Where(o => o.Status == 0 && o.cashierSessionStatus == Model.Enums.CashierSessionStatus.START && o.UserRole == UserRole.OPDNURSE).OrderByDescending(o => o.Id).ToList<Model.CashierSession>();

            }
            return mtList;
        }

    }
}
