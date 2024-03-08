using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class CashierSession
    {
        public int Id { get; set; }
        public int userID { get; set; }

        [ForeignKey("userID")]
        public User? User { get; set; }

        public decimal StartBalence { get; set; }
        public decimal EndBalence { get; set; }
        public decimal Deviation { get; set; }

        public DateTime StartingTime { get; set; }
        public DateTime EndTime { get; set; }

        public CashierSessionStatus cashierSessionStatus { get; set; }

        public CommonStatus Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public UserRole UserRole { get; set; }
        public int col1 { get; set; } // 1
        public int col2 { get; set; } // 2
        public int col3 { get; set; } // 5
        public int col4 { get; set; } // 10
        public int col5 { get; set; } // 20
        public int col6 { get; set; } // 50
        public int col7 { get; set; } // 100
        public int col8 { get; set; } // 500
        public int col9 { get; set; } // 1000
        public int col10 { get; set; }// 5000

        [NotMapped]
        public decimal TotalAmount { get; set; }

    }
}
