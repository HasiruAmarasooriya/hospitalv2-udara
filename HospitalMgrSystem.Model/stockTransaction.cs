using HospitalMgrSystem.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model
{
    public class stockTransaction
    {
        public int Id { get; set; }
        public int itemID { get; set; }
        public Drug? drugID { get; set; }
        public TransactionMethods TransactionMethods { get; set; }
        public int qty { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
