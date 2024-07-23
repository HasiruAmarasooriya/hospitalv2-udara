using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model.DTO
{
    public class CashierSessionDTO
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public int UserId { get; set; }
        public CashierSessionStatus cashierSessionStatus { get; set; }
        public DateTime StartingTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal StartBalence { get; set; }
        public decimal EndBalence { get; set; }
        public UserRole userRole { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Variation { get; set; }
    }
}
