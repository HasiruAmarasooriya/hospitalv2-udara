namespace HospitalMgrSystem.Service.OPD
{
    public interface IOPDService
    {
        #region OPD Management 
        public HospitalMgrSystem.Model.OPD CreateOPD(HospitalMgrSystem.Model.OPD opd);
        public List<Model.OPD> GetAllOPDByStatus();
        public Model.OPD GetAllOPDByID(int? id);
        public Model.OPD DeleteOPD(int opdId, int userId);
        public List<Model.OPD> SearchOPD(string value);
        #endregion

        #region Drugs Management 
        public Model.OPDDrugus CreateOPDDrugus(HospitalMgrSystem.Model.OPDDrugus opdDrugus);
        public List<Model.OPDDrugus> GetOPDDrugus(int id);
        public Model.OPDDrugus DeleteOPDDrugus(int opdDruguID);
        #endregion

        #region Investigation Management 
        public HospitalMgrSystem.Model.OPDInvestigation CreateOPDInvestigation(HospitalMgrSystem.Model.OPDInvestigation opdnInvestigation);
        public List<Model.OPDInvestigation> GetOPDInvestigation(int id);
        public Model.OPDInvestigation DeleteOPDInvestigation(int opdDruguID);
        #endregion

        #region Items Management 
        public HospitalMgrSystem.Model.OPDItem CreateOPDItems(HospitalMgrSystem.Model.OPDItem opdItems);
        public List<Model.OPDItem> GetOPDItems(int id);
        public Model.OPDItem DeleteOPDItems(int opdDruguID);
        #endregion
    }
}
