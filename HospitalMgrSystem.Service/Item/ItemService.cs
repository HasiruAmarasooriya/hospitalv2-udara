using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Service.Item
{
    public class ItemService :IItemService
    {
        public List<Model.ItemCategory> GetAllItemCategory()
        {
            try
            {
                List<Model.ItemCategory> mtList = new List<Model.ItemCategory>();
                using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
                {
                    mtList = dbContext.ItemCategory.Where(o => o.Status == 0).ToList<Model.ItemCategory>();
                }
                return mtList;
            }
            catch (Exception ex)
            { return null; }
        }


        public List<Model.ItemSubCategory> GetAllItemSubCategoryByID(int Id)
        {

            try
            {
                List<Model.ItemSubCategory> mtList = new List<Model.ItemSubCategory>();
                using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
                {
                    mtList = dbContext.ItemSubCategory.Include(c => c.ItemCategory)
                        .Where(o => o.ItemCategory.ItemCategoryId == Id && o.Status == 0)
                        .ToList<Model.ItemSubCategory>();
                }
                return mtList;
            }
            catch (Exception ex)
            { return null; }
        }

        public HospitalMgrSystem.Model.Item CreateItem(HospitalMgrSystem.Model.Item item)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (item.Id == 0)
                {
                    dbContext.Item.Add(item);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.Item result = (from p in dbContext.Item where p.Id == item.Id select p).SingleOrDefault();
                    result.SNo = item.SNo;
                    result.CreateUser = item.CreateUser;
                    result.Description = item.Description;
                    result.ItemCategoryId = item.ItemCategoryId;
                    result.ItemSubCategoryId = item.ItemSubCategoryId;
                    result.ItemName = item.ItemName;
                    result.Type = item.Type;
                    result.Status = item.Status;
                    result.ModifiedDate = DateTime.Now;
                    result.ModifiedUser = item.ModifiedUser;
                    result.Price = item.Price;

                    dbContext.SaveChanges();
                }
                return dbContext.Item.Find(item.Id);
            }
        }

        public List<Model.Item> GetAllItemByStatus() 
        {
            List<Model.Item> mtList = new List<Model.Item>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Item
                    .Include(c => c.ItemCategory)
                    .Include(c => c.ItemSubCategory)
                    .Where(o => o.Status == 0).ToList<Model.Item>();

            }
            return mtList;
        }

        public Model.Item GetAllItemByID(int? id)
        {
            Model.Item item = new Model.Item();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                item = dbContext.Item.First(o => o.Id == id);

            }
            return item;
        }

        public HospitalMgrSystem.Model.Item DeleteItem(HospitalMgrSystem.Model.Item item)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.Item result = (from p in dbContext.Item where p.Id == item.Id select p).SingleOrDefault();
                result.Status = 1;
                dbContext.SaveChanges();
                return result;
            }
        }
        public List<Model.Item> SearchItem(string value)
        {
            List<Model.Item> mtList = new List<Model.Item>();
            if (value == null) { value = ""; }
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Item
                    .Include(c => c.ItemCategory)
                    .Include(c => c.ItemSubCategory)
                    .Where(o => (o.ItemName.Contains(value) || o.Description.Contains(value)
                || o.ItemCategory.ItemCategoryName.Contains(value)) && o.Status == 0).ToList<Model.Item>();

            }
            return mtList;
        }
    }
}
