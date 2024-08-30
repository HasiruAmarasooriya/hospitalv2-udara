namespace HospitalMgrSystem.Service.Patients
{
    public interface IPatientService
    {
        public Model.Patient CreatePatient(Model.Patient patient);
        public List<Model.Patient> GetAllPatientByStatus();
        public List<Model.Patient> SearchPatient(string value);
        public Model.Patient GetAllPatientByID(int? id);
        public Model.Patient DeletePatient(Model.Patient patient);
    }
}
