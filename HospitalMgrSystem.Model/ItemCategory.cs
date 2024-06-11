using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace HospitalMgrSystem.Model
{
    public class ItemCategory
    {
        [Key]
        public int ItemCategoryId { get; set; }
        public string ItemCategoryName { get; set; }
        public CommonStatus Status { get; set; }
    }
}
