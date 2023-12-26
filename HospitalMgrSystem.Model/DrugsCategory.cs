using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
