using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class StockRequest
    {
        public int Id { get; set; }
        public CommonStatus Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [NotMapped]
        public List<StockRequestItem> Items { get; set; } = new List<StockRequestItem>();
    }
}
