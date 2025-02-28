using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class OPDItem
    {
        public int Id { get; set; }
        public int opdId { get; set; }
        public OPD? opd { get; set; }
        public int ItemId { get; set; }
        public Item? Item { get; set; }
        public int Type { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public ItemInvoiceStatus itemInvoiceStatus { get; set; }
        public CommonStatus Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
