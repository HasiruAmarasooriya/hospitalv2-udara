using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
namespace HospitalMgrSystemUI.Models
{
    public class GRPVItemDto
    {
        public int DrugID { get; set; }
        public int RequestId { get; set; }
        public int Qty { get; set; }
        public int GRNId { get; set; }
        public string? BatchNumber { get; set; }
        public string? SerialNumber { get; set; }
        public decimal Price { get; set; }
        public int Category { get; set; }
        public int SubCategory { get; set; }
        public bool? Discount { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime ProductDate { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
    }
}
