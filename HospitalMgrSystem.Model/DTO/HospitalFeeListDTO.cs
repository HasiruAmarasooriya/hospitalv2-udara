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
    public class HospitalFeeListDto
    {
        public int FeeId { get; set; }
        public decimal Qty { get; set; }
        public string FeeName { get; set; }
        public decimal Price { get; set; }
       
    }
}
