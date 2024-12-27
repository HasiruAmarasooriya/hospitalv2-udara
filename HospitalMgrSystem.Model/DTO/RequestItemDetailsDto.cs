using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Model.DTO
{
    [Keyless]
    public class RequestItemDetailsDto
    {
       public int RequestID { get; set; }
        public string DrugName { get; set; }
        public decimal Quantity { get; set; }
       

       
    }
}
