﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model.DTO
{
	[Keyless]
	public class TotalPaidAmountOfForwardBookingDTO
	{
		public decimal TotalPaidAmount { get; set; }
	}
}
