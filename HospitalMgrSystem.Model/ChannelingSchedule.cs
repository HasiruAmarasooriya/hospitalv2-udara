using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class ChannelingSchedule
    {
        public int Id { get; set; }
        public int NoOfAppointment { get; set; }
        public DateTime DateTime { get; set; }
        public int ConsultantId { get; set; }

        [ForeignKey("ConsultantId")] public Consultant? Consultant { get; set; }

        public int? RoomId { get; set; }

        [ForeignKey("RoomId")] public Room? Room { get; set; }
        public decimal ConsultantFee { get; set; }
        public decimal HospitalFee { get; set; }
        public decimal OtherFee { get; set; }
        public CommonStatus Status { get; set; }
        public ChannellingScheduleStatus scheduleStatus { get; set; }
        [NotMapped] public decimal doctorPaidAppoinment { get; set; }

        [NotMapped] public decimal allBookedAppoinment { get; set; }
        [NotMapped] public decimal totalAmount { get; set; }

        [NotMapped] public decimal? totalPaidAmount { get; set; }

        [NotMapped] public int booked { get; set; }

        [NotMapped] public int paid { get; set; }

        [NotMapped] public int refund { get; set; }

        [NotMapped] public int patientCount { get; set; }

        [NotMapped] public int refundPatientCount { get; set; }

        [NotMapped] public decimal totalPatientCount { get; set; }
        [NotMapped] public decimal actualPatientCount { get; set; }

        [NotMapped] public int totalRefundDoctorFeeCount { get; set; }

        [NotMapped] public int totalRefundHospitalFeeCount { get; set; }

        [NotMapped] public decimal totalRefundDoctorFeeAmount { get; set; }

        [NotMapped] public decimal? totalRefund { get; set; }
        [NotMapped] public decimal totalRefundHospitalFeeAmount { get; set; }

        [NotMapped] public decimal totalHospitalFeeAmount { get; set; }

        [NotMapped] public decimal totalDoctorFeeAmount { get; set; }

		[NotMapped] public List<Model.Scan> scanList { get; set; }


	}
}