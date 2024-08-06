namespace HospitalMgrSystem.Model.DTO
{
	public class PreviousForwardBookingDataDto
	{
		public int Id { get; set; }
		public int OpdId { get; set; }
		public int InvoiceId { get; set; }
		public string FullName { get; set; }
		public int AppoimentNo { get; set; }
		public string DocName { get; set; }
		public string DoctorPaidBy { get; set; }
		public DateTime RegisteredDate { get; set; }
		public decimal TotalPaidAmount { get; set; }
	}
}
