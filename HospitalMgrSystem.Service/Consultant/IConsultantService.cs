namespace HospitalMgrSystem.Service.Consultant
{
    public interface IConsultantService
    {
        public HospitalMgrSystem.Model.Consultant CreateConsultant(HospitalMgrSystem.Model.Consultant consultant);
        public List<Model.Consultant> GetAllConsultantByStatus();

        public List<Model.Consultant> ConsultantGetBySpecialistId(int id);
        public Model.Consultant GetAllConsultantByID(int? id);
        public HospitalMgrSystem.Model.Consultant DeleteConsultant(HospitalMgrSystem.Model.Consultant consultant);
        public List<Model.Consultant> SearchConsultant(string value);
    }
}
