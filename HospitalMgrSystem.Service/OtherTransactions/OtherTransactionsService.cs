using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.OtherTransactions
{
    public class OtherTransactionsService
    {
        public Model.OtherTransactions CreateOtherTransactions(Model.OtherTransactions otherTransactions)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (otherTransactions.Id == 0)
                {
                    dbContext.OtherTransactions.Add(otherTransactions);
                    dbContext.SaveChanges();
                }
                else
                {
                    //HospitalMgrSystem.Model.otherTransactions result = (from p in dbContext.CashierSessions where p.Id == cashierSession.Id select p).SingleOrDefault();
                    //result.EndBalence = cashierSession.EndBalence;
                    //result.EndTime = cashierSession.EndTime;
                    //result.cashierSessionStatus = cashierSession.cashierSessionStatus;
                    //result.ModifiedUser = cashierSession.ModifiedUser;
                    //result.ModifiedDate = cashierSession.ModifiedDate;



                    //dbContext.SaveChanges();
                }
                return otherTransactions;
            }
        }


        public Model.OtherTransactions AproveOtherTransactions(Model.OtherTransactions otherTransactions)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (otherTransactions.Id != 0)
                {

                    HospitalMgrSystem.Model.OtherTransactions result = (from p in dbContext.OtherTransactions where p.Id == otherTransactions.Id select p).SingleOrDefault();
                    result.ApprovedByID = otherTransactions.ApprovedByID;
                    result.otherTransactionsStatus =OtherTransactionsStatus.Approved;
                    dbContext.SaveChanges();
                }
                return otherTransactions;
            }
        }

        public Model.OtherTransactions CompleteeOtherTransactions(Model.OtherTransactions otherTransactions)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (otherTransactions.Id != 0)
                {

                    HospitalMgrSystem.Model.OtherTransactions result = (from p in dbContext.OtherTransactions where p.Id == otherTransactions.Id select p).SingleOrDefault();
                    result.ModifiedDate = otherTransactions.ModifiedDate;
                    result.ModifiedUser = otherTransactions.ModifiedUser;
                    result.otherTransactionsStatus = OtherTransactionsStatus.Completed;
                    dbContext.SaveChanges();
                }
                return otherTransactions;
            }
        }

        public List<Model.OtherTransactions> GetAllOtherTransactions()
        {
            List<Model.OtherTransactions> mtList = new List<Model.OtherTransactions>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OtherTransactions.Include(c => c.Convener).Include(c => c.ApprovedBy).Include(c => c.cashierSession).Where(o => o.Status == 0).OrderByDescending(o => o.Id).ToList<Model.OtherTransactions>();

            }
            return mtList;
        }

        public Model.OtherTransactions GetOtherTransactionsByID(int id)
        {


            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.OtherTransactions result = dbContext.OtherTransactions.Include(c => c.Convener).Include(c => c.ApprovedBy).Include(c => c.cashierSession).Where(o => o.Status == 0 && o.Id == id).SingleOrDefault();
                return result;
            }
        }
    }
}
