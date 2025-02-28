using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;

namespace HospitalMgrSystemUI.Models
{
    public class RequestDto
    {
        public int ID { get; set; }
        public int DrugID { get; set; }
        public List<RequestItemDto> Items { get; set; } = new List<RequestItemDto>();
        public int Qty { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
