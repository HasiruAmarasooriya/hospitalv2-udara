using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Model.DTO
{
	[Keyless]
	public class ReportOpdXrayOtherRefundDTO
	{
		public int InvoiceId { get; set; }
		public int OpdId { get; set; }
		public DateTime RefundedDate { get; set; }
		public string Cashier { get; set; }
		public string PatientName { get; set; }
		public string? Description { get; set; }
		public decimal Amount { get; set; }
	}
}