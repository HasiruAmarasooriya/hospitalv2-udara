using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class Admission
    {
        public int Id { get; set; }
        public string BHTNumber { get; set; }
        public string DateAdmission { get; set; }
        public int RoomId { get; set; }
        public Room? Room { get; set; }
        public int ConsultantId { get; set; }
        public Consultant? Consultant { get; set; }
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }
        public string Guardian { get; set; }
        public decimal Temp { get; set; }
        public string Pluse { get; set; }
        public string Resp { get; set; }
        public string Weight { get; set; }
        public string BP { get; set; }
        public string Details { get; set; }
        public int CreateUser { get; set; }
        public int ModifiedUser { get; set; }
        public AdmissionStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
