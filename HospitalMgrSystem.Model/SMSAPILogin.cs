namespace HospitalMgrSystem.Model
{
    public class SMSAPILogin
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Comment { get; set; }
        public int RemainingCount { get; set; }
        public int Expiration { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
    }
}
