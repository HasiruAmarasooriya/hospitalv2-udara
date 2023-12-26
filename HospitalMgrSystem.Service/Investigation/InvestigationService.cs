using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Investigation
{
    public class InvestigationService : IInvestigationService
    {
        public List<Model.InvestigationCategory> GetAllInvestigationCategory()
        {
            try
            {
                List<Model.InvestigationCategory> mtList = new List<Model.InvestigationCategory>();
                using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
                {
                    mtList = dbContext.InvestigationCategory.Where(o => o.Status == 0).ToList<Model.InvestigationCategory>();
                }
                return mtList;
            }
            catch (Exception ex)
            { return null; }
        }


        public List<Model.InvestigationSubCategory> GetAllInvestigationSubCategoryByID(int Id)
        {

            try
            {
                List<Model.InvestigationSubCategory> mtList = new List<Model.InvestigationSubCategory>();
                using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
                {
                    mtList = dbContext.InvestigationSubCategory.Include(c => c.InvestigationCategory)
                        .Where(o => o.InvestigationCategory.InvestigationCategoryId == Id && o.Status == 0)
                        .ToList<Model.InvestigationSubCategory>();
                }
                return mtList;
            }
            catch (Exception ex)
            { return null; }
        }

        public HospitalMgrSystem.Model.Investigation CreateInvestigation(HospitalMgrSystem.Model.Investigation investigation)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (investigation.Id == 0)
                {
                    dbContext.Investigation.Add(investigation);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.Investigation result = (from p in dbContext.Investigation where p.Id == investigation.Id select p).SingleOrDefault();
                    result.SNo = investigation.SNo;
                    result.CreateUser = investigation.CreateUser;
                    result.Description = investigation.Description;
                    result.InvestigationName = investigation.InvestigationName;
                    result.InvestigationCategoryId = investigation.InvestigationCategoryId;
                    result.InvestigationSubCategoryId = investigation.InvestigationSubCategoryId;
                    result.Status = investigation.Status;
                    result.ModifiedDate = DateTime.Now;
                    result.ModifiedUser = investigation.ModifiedUser;
                    result.Price = investigation.Price;

                    dbContext.SaveChanges();
                }
                return dbContext.Investigation.Find(investigation.Id);
            }
        }

        public List<Model.Investigation> GetAllInvestigationByStatus()
        {
            List<Model.Investigation> mtList = new List<Model.Investigation>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Investigation
                    .Include(c => c.InvestigationCategory)
                    .Include(c => c.InvestigationSubCategory)
                    .Where(o => o.Status == 0).ToList<Model.Investigation>();

            }
            return mtList;
        }

        public Model.Investigation GetAllInvestigationByID(int? id)
        {

            Model.Investigation investigation = new Model.Investigation();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                investigation = dbContext.Investigation.First(o => o.Id == id);

            }
            return investigation;
        }

        public HospitalMgrSystem.Model.Investigation DeleteInvestigation(HospitalMgrSystem.Model.Investigation investigation)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.Investigation result = (from p in dbContext.Investigation where p.Id == investigation.Id select p).SingleOrDefault();
                result.Status = 1;
                dbContext.SaveChanges();
                return result;
            }
        }

        public List<Model.Investigation> SearchInvestigation(string value)
        {
            List<Model.Investigation> mtList = new List<Model.Investigation>();
            if (value == null) { value = ""; }
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Investigation
                    .Include(c => c.InvestigationCategory)
                    .Include(c => c.InvestigationSubCategory)
                    .Where(o => (o.InvestigationName.Contains(value) || o.Description.Contains(value)
                || o.InvestigationCategory.Investigation.Contains(value)) && o.Status == 0).ToList<Model.Investigation>();

            }
            return mtList;
        }
    }
}
