using HospitalMgrSystem.Model;
using System.Collections.Generic;

namespace HospitalMgrSystemUI.Models
{
    public class InvestigationDto
    {
        public Investigation Investigation { get; set; }
        public List<InvestigationCategory> InvestigationCategory { get; set; }
        public List<InvestigationSubCategory> InvestigationSubCategory { get; set; }
        public List<Investigation> InvestigationList { get; set; }
        public string SearchValue { get; set; }
    }
}
