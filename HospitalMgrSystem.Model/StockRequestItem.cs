using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class StockRequestItem
    {
        public int Id { get; set; }
        public int RequestID { get; set; }
        [ForeignKey("RequestID")]StockRequest? StockRequest { get; set; }
        public int DrugID { get; set; }
        public Decimal Quntity { get; set; }
		public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
