using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Model
{
    public class NightShift
    {
        public int Id { get; set; }
        public Shift IsNightShift { get; set; }
    }
}
