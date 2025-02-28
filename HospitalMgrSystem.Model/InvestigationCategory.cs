using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace HospitalMgrSystem.Model
{
    public class InvestigationCategory
    {
        [Key]
        public int InvestigationCategoryId { get; set; }
        public string Investigation { get; set; }
        public CommonStatus Status { get; set; }
    }
}
