using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Model.DTO
{
    [Keyless]
    public class RequestDetailsByIdDto
    {
        public int DrugID { get; set; }
        public int RequestId { get; set; }
        public string DrugName { get; set; }
        public decimal Quantity { get; set; }
       

       
    }
}
