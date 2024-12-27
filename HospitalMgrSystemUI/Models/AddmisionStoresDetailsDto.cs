using HospitalMgrSystem.Model;


namespace HospitalMgrSystemUI.Models
{
    public class AddmisionStoresDetailsDto
    {
      public string DrugName { get; set; }
        public string BatchNumber {  get; set; }
        public string? RefNumber {  get; set; }
        public decimal StockIn { get; set; }
        public decimal StockOut { get; set; }
        public decimal RefundQty { get; set; }
        public decimal AvailableQuantity { get; set; }
        public  decimal Price { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
}
