using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
