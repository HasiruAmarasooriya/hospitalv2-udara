using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Model.DTO
{
    [Keyless]
    public class StockQuantityDto
    {
        public string? BatchNumber { get; set; }
        public decimal StockInQty { get; set; }
        public decimal StockOutQty { get; set; }
        public decimal AvailableQty { get; set; }




    }
}
