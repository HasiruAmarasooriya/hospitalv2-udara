using HospitalMgrSystem.Model;

namespace HospitalMgrSystemUI.Models
{
    public class OtherTransactionsDto
    {

        public List<OtherTransactions> otherTransactionsList { get; set; }
        public int otherTransactionsID { get; set; }
        public OtherTransactions otherTransactions { get; set; }
        public User user { get; set; }
        public DateTime sessionDate { get; set; }
        public string sessionDetails { get; set; }

        public string username { get; set; }
    }
}
