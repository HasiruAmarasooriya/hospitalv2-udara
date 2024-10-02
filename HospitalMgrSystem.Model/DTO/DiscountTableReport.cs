using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Model.DTO;

[Keyless]
public class DiscountTableReport
{
    public int InvoiceId { get; set; }    
    public int OpdId { get; set; }    
    public string CustomerName { get; set; }
    public DateTime IssuedDate { get; set; }
    public string IssuedBy { get; set; }
    public decimal Discount { get; set; }
}