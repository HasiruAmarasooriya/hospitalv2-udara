using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.NightShiftSession
{
    public class NightShiftSessionService
    {

        public Model.NightShiftSession CreateNightShiftSession(Model.NightShiftSession cightShiftSession)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (cightShiftSession.Id == 0)
                {
                    dbContext.NtShiftSessions.Add(cightShiftSession);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.NightShiftSession result = (from p in dbContext.NtShiftSessions where p.Id == cightShiftSession.Id select p).SingleOrDefault();

                    result.EndTime = cightShiftSession.EndTime;
                    result.shiftSessionStatus = cightShiftSession.shiftSessionStatus;
                    result.ModifiedUser = cightShiftSession.ModifiedUser;
                    result.ModifiedDate = cightShiftSession.ModifiedDate;



                    dbContext.SaveChanges();
                }
                return cightShiftSession;
            }
        }

        public List<Model.NightShiftSession> GetAllNightShiftSession()
        {
            List<Model.NightShiftSession> mtList = new List<Model.NightShiftSession>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.NtShiftSessions.Include(c => c.User).Where(o => o.Status == 0).OrderByDescending(o => o.Id).ToList<Model.NightShiftSession>();

            }
            return mtList;
        }

        public Model.NightShiftSession GetNightShiftSessionByID(int id)
        {


            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.NightShiftSession result = dbContext.NtShiftSessions.Include(c => c.User).Where(o => o.Status == 0 && o.Id == id).SingleOrDefault();
                return result;
            }
        }


        public List<Model.NightShiftSession> GetACtiveNtShiftSessions()
        {
            List<Model.NightShiftSession> mtList = new List<Model.NightShiftSession>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.NtShiftSessions.Include(c => c.User).Where(o => o.Status == 0 && o.shiftSessionStatus == Model.Enums.ShiftSessionStatus.START ).OrderByDescending(o => o.Id).ToList<Model.NightShiftSession>();

            }
            return mtList;
        }

        public List<Model.NightShiftSession> GetACtiveNtShiftSessionsByUserID(int userID,Shift shift)
        {
            List<Model.NightShiftSession> mtList = new List<Model.NightShiftSession>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.NtShiftSessions.Include(c => c.User).Where(o => o.Status == 0 && o.shiftSessionStatus == Model.Enums.ShiftSessionStatus.START && o.userID == userID && o.shift == shift).OrderByDescending(o => o.Id).ToList<Model.NightShiftSession>();

            }
            return mtList;
        }



        public bool checkIsNightShift()
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                NightShift nightShift = dbContext.NightShifts.First(o => o.Id == 1);

                if (nightShift.IsNightShift == Shift.DAY_SHIFT)
                {
                   return false;
                }
                else
                {
                    return true;
                }

            }

        }
    }
}
