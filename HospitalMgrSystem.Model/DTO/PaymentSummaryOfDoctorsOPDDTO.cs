using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model.DTO
{
	[Keyless]
	public class PaymentSummaryOfDoctorsOPDDTO
	{
		public string ConsultantName { get; set; }
		public int BillCount { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal TotalPaidAmount { get; set; }
		public decimal TotalRefundAmount { get; set; }
	}
}
