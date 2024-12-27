namespace HospitalMgrSystem.Service.Drugs
{
    public interface IDrugsService
    {
        public List<Model.DrugsCategory> GetAllDrugsCategory();
        public List<Model.DrugsSubCategory> DrugsSubCategoryByID(int Id);
        public HospitalMgrSystem.Model.Drug CreateDrugs(HospitalMgrSystem.Model.Drug drug);
        public List<Model.Drug> GetAllDrugsByStatus();
        public Model.Drug GetAllDrugByID(int? id);
        public HospitalMgrSystem.Model.Drug DeleteDrug(HospitalMgrSystem.Model.Drug drug);
        public List<Model.Drug> SearchDrug(string value);
        public Model.Drug GetDrugById(int Id);
    }
}
