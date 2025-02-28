using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;

namespace HospitalMgrSystemUI.Models
{
    public class OtherTransactionsDto
    {

        public List<OtherTransactionsDTO> otherTransactionsList { get; set; }
        public int otherTransactionsID { get; set; }
        public OtherTransactions otherTransactions { get; set; }
        public User user { get; set; }
        public List<OtherTransactions> benificaryOutTransactionList { get; set; }
        public List<User> benificaryList { get; set; }

        public List<Consultant> benificaryDrList { get; set; }
        public DateTime sessionDate { get; set; }
        public string sessionDetails { get; set; }

        public string username { get; set; }

        public string BeneficiaryUsername { get; set; }
    }
}
