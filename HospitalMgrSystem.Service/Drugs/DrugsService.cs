using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Drugs
{
    public class DrugsService : IDrugsService
    {
        public List<Model.DrugsCategory> GetAllDrugsCategory()
        {
            try
            {
                List<Model.DrugsCategory> mtList = new List<Model.DrugsCategory>();
                using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
                {
                    mtList = dbContext.DrugsCategory.Where(o => o.Status == 0).ToList<Model.DrugsCategory>();
                }
                return mtList;
            }
            catch (Exception ex)
            { return null; }
        }


        public List<Model.DrugsSubCategory> DrugsSubCategoryByID(int Id) {

            try
            {
                List<Model.DrugsSubCategory> mtList = new List<Model.DrugsSubCategory>();
                using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
                {
                    mtList = dbContext.DrugsSubCategory.Include(c => c.DrugsCategory).Where(o => o.DrugsCategory.DrugsCategoryId == Id && o.Status == 0).ToList<Model.DrugsSubCategory>();
                }
                return mtList;
            }
            catch (Exception ex)
            { return null; }
        }

        public HospitalMgrSystem.Model.Drug CreateDrugs(HospitalMgrSystem.Model.Drug drug)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (drug.Id == 0)
                {
                    dbContext.Drugs.Add(drug);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.Drug result = (from p in dbContext.Drugs where p.Id == drug.Id select p).SingleOrDefault();
                    result.CreateDate = DateTime.Now;
                    result.SNo = drug.SNo;
                    result.CreateUser = drug.CreateUser;
                    result.Description = drug.Description;
                    result.DrugName = drug.DrugName;
                    result.billingItemsType =(BillingItemsType) drug.DrugsCategoryId;
                    result.DrugsCategoryId = drug.DrugsCategoryId;
                    result.DrugsSubCategoryId = drug.DrugsSubCategoryId;
                    result.Status = drug.Status;
                    result.ModifiedDate = DateTime.Now;
                    result.ModifiedUser = drug.ModifiedUser;
                    result.Price = drug.Price;
                    
                    dbContext.SaveChanges();
                }
                return dbContext.Drugs.Find(drug.Id);
            }
        }

        public List<Model.Drug> GetAllDrugsByStatus()
        {
            List<Model.Drug> mtList = new List<Model.Drug>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Drugs.Include(c => c.DrugsCategory).Include(c => c.DrugsCategory).Where(o => o.Status == 0).ToList<Model.Drug>();

            }
            return mtList;
        }

        public Model.Drug GetAllDrugByID(int? id)
        {

            Model.Drug drug = new Model.Drug();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                drug = dbContext.Drugs.First(o => o.Id == id);

            }
            return drug;
        }

        public HospitalMgrSystem.Model.Drug DeleteDrug(HospitalMgrSystem.Model.Drug drug)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.Drug result = (from p in dbContext.Drugs where p.Id == drug.Id select p).SingleOrDefault();
                result.Status = 1;
                dbContext.SaveChanges();
                return result;
            }

        }

        public List<Model.Drug> SearchDrug(string value)
        {
            List<Model.Drug> mtList = new List<Model.Drug>();
            if (value == null) { value = ""; }
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Drugs.Include(c => c.DrugsCategory).Include(c => c.DrugsSubCategory).Where(o => (o.DrugName.Contains(value) || o.Description.Contains(value)
                || o.DrugsCategory.Category.Contains(value)) && o.Status == 0).ToList<Model.Drug>();

            }
            return mtList;
        }

    }
}
