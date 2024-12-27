using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class PurchaseRequest
	{
        public int Id { get; set; }
		public DateTime RequestDate { get; set; }
		public CommonStatus Status { get; set; }
		public string? ApproverComments { get; set; }
		public string? ApprovedBy { get; set; }
		public int ItemId { get; set; }
		[ForeignKey("ItemId ")] public PurchaseRequestItem? PurchaseRequestItem { get; set; }
		public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
