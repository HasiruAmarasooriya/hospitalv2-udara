using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
namespace HospitalMgrSystemUI.Models
{
    public class GRPVDto
    {
        public List<GRN> supplierList { get; set; }

        public List<Drug> drugList { get; set; }
        public List<RequestDetailsDto> RqeuestList { get; set; }
        public List<RequestItemDetailsDto> RqeuestItem { get; set; }
        public List<RequestDetailsByIdDto> RqeuestItemByID { get; set; }
        public List<GRPVItemDto> Items { get; set; }
        public List<DrugsCategory> DrugsCategory { get; set; }
        public List<DrugsSubCategory> DrugsSubCategory { get; set; }
        public DateTime PreviousDateTime { get; set; }
        public int ID { get; set; }
        public int DrugID { get; set; }
        public int RequestId { get; set; }
        public int Qty { get; set; }
        public int GRNId { get; set; }
        public string? BatchNumber { get; set; }
        public string? SerialNumber { get; set; }
        public decimal Price { get; set; }
        public decimal ReStockLevel { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime ProductDate { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public decimal Amount {  get; set; }
        public int Category { get; set; }
        public int SubCategory { get; set; }
        public bool? Discount { get; set; }
        public GRPV GRPV { get; set; }
    }
}
