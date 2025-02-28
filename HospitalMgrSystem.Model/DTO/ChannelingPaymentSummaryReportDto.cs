using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Model.DTO
{
	[Keyless]
	public class ChannelingPaymentSummaryReportDto
	{
		public decimal TotalPaid { get; set; }
		public decimal TotalRefund { get; set; }
	}
}
