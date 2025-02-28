using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Model.DTO
{
	[Keyless]
	public class ReportOpdXrayOtherDrugs
	{
		public int DrugId { get; set; }
		public string DrugName { get; set; }
		public decimal Price { get; set; }
		public decimal Quantity { get; set; }
		public decimal TotalAmount { get; set; }
	}
}
