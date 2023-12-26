using HospitalMgrSystem.Model;
using System.Collections.Generic;

namespace HospitalMgrSystemUI.Models
{
    public class ItemDto
    {
        public  List<ItemCategory> ItemCategory { get; set; }
        public List<ItemSubCategory> ItemSubCategory { get; set; }
        public Item Item { get; set; }
        public List<Item> ItemList { get; set; }
        public string SearchValue { get; set; }
    }
}
