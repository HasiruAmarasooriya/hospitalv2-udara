using HospitalMgrSystem.Model;
using System.Collections.Generic;

namespace HospitalMgrSystemUI.Models
{
    public class DrugsDto
    {
        public List<DrugsCategory> DrugsCategory { get; set; }
        public List<DrugsSubCategory> DrugsSubCategory { get; set; }
        public List<Drug> ListDrogs { get; set; }
        public int CategoryID { get; set; }
        public Drug Drug { get; set; }
        public string SearchValue { get; set; }
    }
}
