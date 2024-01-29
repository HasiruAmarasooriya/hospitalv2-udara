using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace HospitalMgrSystemUI.Models
{
    public class OPDTbDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string roomName { get; set; }
        public string consaltantName { get; set; }
        public SexStatus Sex { get; set; }
        public string MobileNumber { get; set; }
        public int AppoimentNo { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public CommonStatus Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public PaymentStatus paymentStatus { get; set; }
        public int schedularId { get; set; }
    }
}
