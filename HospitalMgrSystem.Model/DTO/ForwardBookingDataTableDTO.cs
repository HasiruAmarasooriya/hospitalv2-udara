using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model.DTO
{
	[Keyless]
	public class ForwardBookingDataTableDTO
	{
		public int ScheduleId { get; set; }
		public string DoctorName { get; set; }
		public DateTime ScheduleDate { get; set; }
		public decimal PaidAmount { get; set; }
	}
}
