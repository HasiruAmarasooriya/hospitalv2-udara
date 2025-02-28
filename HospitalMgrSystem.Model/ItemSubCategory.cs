using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class ItemSubCategory
    {
        public int ItemSubCategoryId { get; set; }
        public int ItemCategoryId { get; set; }
        [ForeignKey("ItemCategoryId")]
        public ItemCategory? ItemCategory { get; set; }
        public string ItemSubCategoryName { get; set; }
        public CommonStatus Status { get; set; }
    }
}
