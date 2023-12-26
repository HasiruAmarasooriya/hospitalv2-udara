using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
