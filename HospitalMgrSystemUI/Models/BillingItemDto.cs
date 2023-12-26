using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystemUI.Models
{
    public class BillingItemDto
    {
        public int BillingItemID { get; set; }
        public BillingItemsType billingItemsType { get; set; }
        public string billingItemsTypeName { get; set; }
        public string billingItemName { get; set; }
        public decimal qty { get; set; }
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public decimal discount { get; set; }

    }
}
