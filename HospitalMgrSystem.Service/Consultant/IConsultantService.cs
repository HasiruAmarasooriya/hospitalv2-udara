namespace HospitalMgrSystem.Service.Consultant
{
    public interface IConsultantService
    {
        public Model.Consultant CreateConsultant(Model.Consultant consultant);
        public List<Model.Consultant> GetAllConsultantByStatus();
        public List<Model.Consultant> ConsultantGetBySpecialistId(int id);
        public Model.Consultant GetAllConsultantByID(int? id);
        public Model.Consultant DeleteConsultant(Model.Consultant consultant);
        public List<Model.Consultant> SearchConsultant(string value);
        public List<Model.Consultant> GetAllOPDConsultantByStatus();

	}
}
