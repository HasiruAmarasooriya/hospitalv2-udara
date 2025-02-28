using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Model.DTO
{
	[Keyless]
	public class PaymentSummaryOpdXrayOtherDTO
	{
		public decimal TotalAmount { get; set; }
		public decimal TotalRefundAmount { get; set; }
		public decimal TotalPaidAmount { get; set; }
	}
}
