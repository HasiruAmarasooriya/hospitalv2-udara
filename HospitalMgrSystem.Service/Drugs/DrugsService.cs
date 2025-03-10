using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using HospitalMgrSystem.Model.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
                    result.IsDiscountAvailable = drug.IsDiscountAvailable;
                    result.Status = drug.Status;
                    result.Qty = drug.Qty;
                    result.isStock = drug.isStock;
                    result.ModifiedDate = DateTime.Now;
                    result.ModifiedUser = drug.ModifiedUser;
                    result.Price = drug.Price;
                    result.BatchNumber = drug.BatchNumber;
                    result.ReStockLevel = drug.ReStockLevel;
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
        //public Model.Drug GetDrugById(int drugId)
        //{
        //    using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
        //    {
        //        var drug = dbContext.Drugs.Include(c => c.DrugsCategory)
        //                                  .FirstOrDefault(d => d.Id == drugId && d.Status == 0);
        //        return drug;
        //    }
        //}

        public List<Model.Drug> GetAllXrayyStatus()
        {
            List<Model.Drug> mtList = new List<Model.Drug>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Drugs.Include(c => c.DrugsCategory).Include(c => c.DrugsCategory).Where(o => o.Status == 0 && o.DrugsSubCategoryId == 22).ToList<Model.Drug>();

            }
            return mtList;
        }

        public List<Model.Drug> GetAllDrugsByCategory(int drugCategory,int drugSubCategory)
        {
            List<Model.Drug> mtList = new List<Model.Drug>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Drugs.Include(c => c.DrugsCategory).Include(c => c.DrugsCategory).Where(o => o.Status == 0 && o.DrugsCategoryId == drugCategory && o.DrugsSubCategoryId == drugSubCategory).ToList<Model.Drug>();

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
                result.ModifiedDate = DateTime.Now;
                result.ModifiedUser = drug.ModifiedUser;
                dbContext.SaveChanges();
                return result;
            }

        }

        //public List<Model.Drug> SearchDrug(string value)
        //{
        //    List<Model.Drug> mtList = new List<Model.Drug>();
        //    if (value == null) { value = ""; }
        //    using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
        //    {
        //        mtList = dbContext.Drugs.Include(c => c.DrugsCategory).Include(c => c.DrugsSubCategory).Where(o => (o.DrugName.Contains(value) || o.Description.Contains(value)
        //        || o.DrugsCategory.Category.Contains(value)) && o.Status == 0).ToList<Model.Drug>();

        //    }
        //    return mtList;
        //}
        public List<Model.Drug> SearchDrug(string value)
        {
            List<Model.Drug> mtList = new List<Model.Drug>();
            if (value == null) { value = ""; }
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Drugs.Include(c => c.DrugsCategory).Include(c => c.DrugsSubCategory).Where(o => (o.DrugName.Contains(value)) && o.Status == 0).ToList<Model.Drug>();

            }
            return mtList;
        }
        public List<Model.Drug> GetDrugById(string value)
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
        public List<Model.Drug> GetOPDDrugus(int tranType)
        {
            List<Model.Drug> opdDrugs = new List<Model.Drug>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                var tranTypeParam = new SqlParameter("@TranType", tranType);

                // Get the OPDDrugus entries using the stored procedure
                var opdDrugusList = dbContext.Drugs
                    .FromSqlRaw("EXEC GetAvailableOPDDrugs @TranType", tranTypeParam)
                    .ToList<Model.Drug>();
                opdDrugs = opdDrugusList;
            }
            return opdDrugs;
        }

        public Model.Drug GetDrugById(int Id)
        {
            Model.Drug drug = null;

            using (var dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                
                var drugIdParam = new SqlParameter("@DrugId", Id);

                // Execute the stored procedure and convert the result to an enumerable
                drug = dbContext.Drugs
                    .FromSqlRaw("EXEC GetAvailableOPDDrugsQtyByID @DrugId", drugIdParam)
                    .AsEnumerable() // Switch to client-side evaluation here
                    .FirstOrDefault(); // Now apply FirstOrDefault on the client side
                if (drug == null)
                {
                    drug = dbContext.Drugs.First(o => o.Id == Id);
                }
            }

            return drug;
        }
        public LogTranDTO GetDrugDetailsById(int id)
        {
            using (var dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
              
                var drugIdParam = new SqlParameter("@DrugId", id);

                // Execute the stored procedure and map the result to the DTO
                var drugInfo = dbContext.Set<LogTranDTO>()
                    .FromSqlRaw("EXEC GetDrugBatchWithTransactionDetails @DrugId", drugIdParam)
                    .AsEnumerable()
                    .FirstOrDefault();

                return drugInfo;
            }
        }
        //public LogTranDTO GetDrugDetailsByBillId(int type, int id)
        //{
        //    using (var dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
        //    {
        //        //var tranTypeParam = new SqlParameter("@TranType", type);
        //        //var drugIdParam = new SqlParameter("@Bill", id);

        //        //// Execute the stored procedure and map the result to the DTO
        //        //var drugInfo = dbContext.Set<LogTranDTO>();

        //        //return 1;
        //    }
        //}

        public Drug? GetDrugByName(string drugName)
        {
            using (var dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                return dbContext.Drugs
                                .Include(d => d.DrugsCategory)
                                .Include(d => d.DrugsSubCategory)
                                .FirstOrDefault(d => d.DrugName.ToUpper() == drugName && d.Status == 0);
            }
        }



    }
}
