using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class PurchaseRequestItem
	{
        public int Id { get; set; }
		public int RequestId { get; set; }
		[ForeignKey("RequestId ")] public PurchaseRequest? PurchaseRequest { get; set; }
		public int? DrugId { get; set; }
		[ForeignKey("DrugId ")] public Drug? Drug { get; set; }
		public int? Quantity { get; set; }
		public int? BatchNumber { get; set; }
		public int? SerialNumber { get; set; }
		public int? SupplierID { get; set; }
		[ForeignKey("SupplierID")]public GRN? GRN { get; set; }
		public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
