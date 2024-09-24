using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model;

public class Discount
{
    public int Id { get; set; }
    public decimal Percentage { get; set; }
    public int Status { get; set; }
    public int? CreateUser { get; set; }
    [ForeignKey(nameof(CreateUser))] public User? CrUser { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public int? ModifiedUser { get; set; }
    [ForeignKey(nameof(ModifiedUser))] public User? MdUser { get; set; }
}