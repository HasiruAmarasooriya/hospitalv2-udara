using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model.DTO
{
    [Keyless]
    public class RequestDetailsDto
    {
        public int RequestID { get; set; }
        public int StockRequestItemID { get; set; }
        public string DrugName { get; set; }
        public decimal Quantity { get; set; }
        public string CreatedBy { get; set; }
       
        public DateTime CreateDate { get; set; }
        [NotMapped]
        public List<RequestItemDetailsDto> Items { get; set; }

       
    }
}
