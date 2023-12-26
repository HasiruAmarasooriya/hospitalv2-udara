using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Patients
{
    public interface IPatientService
    {
        public HospitalMgrSystem.Model.Patient CreatePatient(HospitalMgrSystem.Model.Patient patient);
        public List<Model.Patient> GetAllPatientByStatus();
        public List<Model.Patient> SearchPatient(string value);
        public Model.Patient GetAllPatientByID(int? id);
        public HospitalMgrSystem.Model.Patient DeletePatient(HospitalMgrSystem.Model.Patient patient);
    }
}
