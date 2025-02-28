namespace HospitalMgrSystem.Service.Channeling
{
    public interface IChannelingService
    {
        public HospitalMgrSystem.Model.Channeling CreateChanneling(HospitalMgrSystem.Model.Channeling channeling);
        public List<Model.OPD> GetAllChannelingByStatus();
        //public HospitalMgrSystem.Model.Channeling ChannelingGetById(int Id);

        public List<Model.OPD> ChannelingGetBySheduleId(int id);
        public HospitalMgrSystem.Model.OPD DeleteChanneling(int id);
        public List<Model.Scan> LoadChannelingItems();
        public List<Model.Consultant> GetAllConsultantThatHaveSchedulingsByDate(DateTime scheduleDate);
    }
}
