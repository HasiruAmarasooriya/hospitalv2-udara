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
        public decimal TotalAmount { get; set; }
    }
}
