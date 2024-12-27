using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Model.DTO
{
    [Keyless]
    public class GRPVDetailsDto
    {
        public int Id { get; set; }
        public decimal Qty { get; set; }
        public int DrugId { get; set; }
        public string BatchNumber { get; set; }
        public int GRNId { get; set; }
        
        public decimal Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ProductDate { get; set; }
        public string DrugName { get; set; }
        public string SupplierName { get; set; }
    }
}
