using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace HospitalMgrSystem.Model
{
    public class DrugsCategory
    {
        [Key]
        public int DrugsCategoryId { get; set; }
        public string Category { get; set; }
        public CommonStatus Status { get; set; }
    }
}
