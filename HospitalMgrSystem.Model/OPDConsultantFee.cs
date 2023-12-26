using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class OPDConsultantFee
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DefaultStatus IsDefault { get; set; }
        public CommonStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
