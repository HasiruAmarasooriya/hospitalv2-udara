using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class InvestigationSubCategory
    {
        public int InvestigationSubCategoryId { get; set; }
        public int InvestigationCategoryId { get; set; }
        [ForeignKey("InvestigationCategoryId")]
        public InvestigationCategory? InvestigationCategory { get; set; }
        public string InvestigationSubCategoryName { get; set; }
        public CommonStatus Status { get; set; }
    }
}
