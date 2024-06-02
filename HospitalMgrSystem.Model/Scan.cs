using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class Scan
    {
        public int Id { get; set; }
        public string ItemName { get; set; }

        public int Tag1 { get; set; }
        public int Tag2{ get; set; }

        public decimal HospitalFee { get; set; }
        public decimal DoctorFee { get; set; }

		[NotMapped]
		public decimal TotalChannelingWithoutRefund { get; set; }

		[NotMapped]
        public decimal TotalAmount { get; set; }
		[NotMapped] public int totalDoctorFeeCount { get; set; }
		[NotMapped] public int totalHospitalFeeCount { get; set; }
		[NotMapped] public int totalDoctorFeeRefundCount { get; set; }
		[NotMapped] public int totalHospitalFeeRefundCount { get; set; }
		[NotMapped] public decimal totalDoctorFeeAmount { get; set; }
		[NotMapped] public decimal totalHospitalFeeAmount { get; set; }
		[NotMapped] public decimal totalRefundDoctorFeeAmount { get; set; }
		[NotMapped] public decimal totalRefundHospitalFeeAmount { get; set; }
	}
}
