using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Model.DTO
{
	[Keyless]
	public class ReportOpdXrayOtherPaidDto
	{
		public int OpdId { get; set; }
		public int InvoiceId { get; set; }
		public string PatientName { get; set; }
		public string DoctorName { get; set; }
		public string CashierName { get; set; }
		public string OpdNurse { get; set; }
		public DateTime IssuedDate { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal TotalPaidAmount { get; set; }
		public decimal Deviation { get; set; }
	}
}
