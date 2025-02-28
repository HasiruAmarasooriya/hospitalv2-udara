using HospitalMgrSystem.Model;


namespace HospitalMgrSystemUI.Models
{
    public class MainStoresDto
    {
      public string DrugName { get; set; }
        public string BatchNumber {  get; set; }
        public decimal StockIn { get; set; }
        public decimal StockOut { get; set; }
        public decimal AvailableQuantity { get; set; }
        public  decimal Price { get; set; }
        public decimal Amount { get; set; }
        public string SupplierName { get; set; }
        public DateTime ExpiryDate { get; set; }

		public decimal ReStockLevel { get; set; }

	}
}
