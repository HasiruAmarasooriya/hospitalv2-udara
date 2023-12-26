using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class Item
    {
        public int Id { get; set; }
        public string? SNo { get; set; }
        public string? ItemName { get; set; }
        public ItemType? Type { get; set; }
        public int ItemCategoryId { get; set; }
        public ItemCategory? ItemCategory { get; set; }
        public int ItemSubCategoryId { get; set; }
        public ItemSubCategory? ItemSubCategory { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
