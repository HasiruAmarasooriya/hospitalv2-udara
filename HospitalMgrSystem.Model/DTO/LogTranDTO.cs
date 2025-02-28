using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model.DTO
{
    [Keyless]
    public class LogTranDTO
    {
        public int GrpvId { get; set; }
        public int BillId { get; set; }
        public int DrugIdRef { get; set; }
        public decimal Qty { get; set; }
        public string RefNumber { get; set; }
        public string Remark { get; set; }
        public string BatchNumber { get; set; }
    }
}
