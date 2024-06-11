using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class Drug
    {
        public int Id { get; set; }
        public string? SNo { get; set; }
        public string? DrugName { get; set; }
        public BillingItemsType billingItemsType { get; set; }
        public int DrugsCategoryId { get; set; }
        [ForeignKey("DrugsCategoryId")]
        public DrugsCategory? DrugsCategory { get; set; }
        
        public int DrugsSubCategoryId { get; set; }
        [ForeignKey("DrugsSubCategoryId")]
        public DrugsSubCategory? DrugsSubCategory { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Qty { get; set; }
        public int isStock { get; set; }
        public int Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
