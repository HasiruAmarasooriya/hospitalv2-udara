namespace HospitalMgrSystem.Model.DTO
{
    public class OpdOtherXrayDataTableDto
    {
        public int Id { get; set; }
        public string roomName { get; set; }
        public string consultantName { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateTime { get; set; }
        public int Sex { get; set; }
        public int Status { get; set; }
        public int paymentStatus { get; set; }
        public decimal Total { get; set; }

    }
}
