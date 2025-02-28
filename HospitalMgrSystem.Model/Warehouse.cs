using HospitalMgrSystem.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalMgrSystem.Model
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public decimal ReStockLevel { get; set; }

	}
}
