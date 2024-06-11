namespace HospitalMgrSystem.Service.Investigation
{
    public interface IInvestigationService
    {
        public List<Model.InvestigationCategory> GetAllInvestigationCategory();
        public List<Model.InvestigationSubCategory> GetAllInvestigationSubCategoryByID(int Id);
        public HospitalMgrSystem.Model.Investigation CreateInvestigation(HospitalMgrSystem.Model.Investigation investigation);
        public List<Model.Investigation> GetAllInvestigationByStatus();
        public Model.Investigation GetAllInvestigationByID(int? id);
        public HospitalMgrSystem.Model.Investigation DeleteInvestigation(HospitalMgrSystem.Model.Investigation investigation);
        public List<Model.Investigation> SearchInvestigation(string value);
    }
}
