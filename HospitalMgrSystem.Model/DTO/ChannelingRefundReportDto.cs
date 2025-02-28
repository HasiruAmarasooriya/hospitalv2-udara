namespace HospitalMgrSystem.Model.DTO
{
	public class ChannelingRefundReportDto
	{
		public int Id { get; set; }
		public int InvoiceId { get; set; }
		public int SchedularId { get; set; }
		public string PatientName { get; set; }
		public string DocName { get; set; }
		public string RefundedBy { get; set; }
		public DateTime RefundedDate { get; set; }
		public decimal Total { get; set; }
	}
}
