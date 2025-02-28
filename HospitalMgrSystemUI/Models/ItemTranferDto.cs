using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;

namespace HospitalMgrSystemUI.Models
{
    public class ItemTranferDto
    {
        public int ID { get; set; }
        public List<Warehouse>Warehouses { get; set; }
        public int DrugId { get; set; }
        public int GRPVId { get; set; }
        public string? BatchNumber { get; set; }
        public int GRNId { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ProductDate { get; set; }
        public  int FromWarehouses { get; set; }
        public int ToWarehouse { get;set; }
        public List<GRPVDetailsDto> GRPV { get; set; }
        public decimal Qty { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
      
        
    }
}
