using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class User
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailAddr { get; set; }
        public int Status { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public UserRole userRole { get; set; }
    }
}
