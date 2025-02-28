using HospitalMgrSystem.Model;

namespace HospitalMgrSystemUI.Models
{
    public class StockDto
    {
        public List<DrugsCategory> DrugsCategory { get; set; }
        public List<DrugsSubCategory> DrugsSubCategory { get; set; }
        public List<Drug> ListDrogs { get; set; }
        public int CategoryID { get; set; }
        public Drug Drug { get; set; }
        public GRN GRN { get; set; }
        public string SearchValue { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierContact { get; set; }

    }
}
