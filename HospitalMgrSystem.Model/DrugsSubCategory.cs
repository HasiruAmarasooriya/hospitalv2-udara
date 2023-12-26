using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class DrugsSubCategory
    {
        public int DrugsSubCategoryId { get; set; }
        public int DrugsCategoryId { get; set; }
        [ForeignKey("DrugsCategoryId")]
        public DrugsCategory? DrugsCategory { get; set; }
        public string DrugsSubCategoryName { get; set; }
        public CommonStatus Status { get; set; }
    }
}
