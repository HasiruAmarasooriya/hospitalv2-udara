using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class GRPV
    {
		public decimal ReStockLevel;

		public int Id { get; set; }
        public int DrugId { get; set; } 
        [ForeignKey("DrugId")]
        public Drug? Drug { get; set; }

        public int GRNId { get; set; }

        [ForeignKey("GRNId")]
        public GRN? GRN { get; set; }

        public String? BatchNumber { get; set; }
        public string SN {  get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal SellPrecentage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ProductDate { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
