using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class ClaimBill
    {
        public int Id { get; set; }
        public int PatientID { get; set; }

        [ForeignKey("PatientID")]
        public Patient? Patient { get; set; }
        public int ConsultantId { get; set; }

        [ForeignKey("ConsultantId")]
        
        public Consultant? Consultant { get; set; }
        public string? RefNo { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal CashAmount { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
