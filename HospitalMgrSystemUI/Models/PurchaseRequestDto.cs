using HospitalMgrSystem.Model;
namespace HospitalMgrSystemUI.Models
{
    public class PurchaseRequestDto
	{
        public List<GRN> supplierList { get; set; }

        public List<Drug> drugList { get; set; }
        public DrugRequestDto request { get; set; }
        public DateTime PreviousDateTime { get; set; }
        public int ID { get; set; }
        public int DrugID { get; set; }
       
        public string? BatchNumber{get;set;}
        public string? SerialNumber { get; set; }
        public int Qty {  get; set; }
		public int ItemId { get; set; }
		public int RequestId { get; set; }
		public string DrugName { get; set; }
		public int Quantity { get; set; }
		public int SupplierID { get; set; }
        public string Comments { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public GRPV GRPV { get; set; }
    }
}
