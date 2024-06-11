namespace HospitalMgrSystem.Service.Item
{
    public interface IItemService
    {
        public List<Model.ItemCategory> GetAllItemCategory();
        public List<Model.ItemSubCategory> GetAllItemSubCategoryByID(int Id);
        public HospitalMgrSystem.Model.Item CreateItem(HospitalMgrSystem.Model.Item item);
        public List<Model.Item> GetAllItemByStatus();
        public Model.Item GetAllItemByID(int? id);
        public HospitalMgrSystem.Model.Item DeleteItem(HospitalMgrSystem.Model.Item item);
        public List<Model.Item> SearchItem(string value);
    }
}
