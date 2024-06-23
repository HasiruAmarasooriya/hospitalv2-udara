using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model.DTO
{
	public class ClaimBillDto
	{
		public int Id { get; set; }
		public string IssuedCashier { get; set; }
		public string PatientName { get; set; }
		public string MobileNumber { get; set; }
		public string RefNo { get; set; }
		public string ConsuntantName { get; set; }
		public string Specialist { get; set; }
		public int AppoimentNo { get; set; }
		public string Description { get; set; }
		public decimal SubTotal { get; set; }
		public decimal Discount { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal CashAmount { get; set; }
		public decimal Balance { get; set; }
		public DateTime IssuedDate { get; set; }
	}
}
