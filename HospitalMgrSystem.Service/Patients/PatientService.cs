using HospitalMgrSystem.DataAccess;
using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Service.Patients
{
    public class PatientService : IPatientService
    {
        public Patient CreatePatient(Patient patient)
        {

            using (var dbContext = new HospitalDBContext())
            {
                if (patient.Id == 0)
                {
                    dbContext.Patients.Add(patient);
                    dbContext.SaveChanges();
                }
                else
                {
                    var result = (from p in dbContext.Patients where p.Id == patient.Id select p).SingleOrDefault();

                    result.FullName = patient.FullName;
                    result.Address = patient.Address;
                    result.Age = patient.Age;
                    result.Months = patient.Months;
                    result.Days = patient.Days;
                    result.MobileNumber = patient.MobileNumber;
                    result.ModifiedDate = DateTime.Now;
                    result.ModifiedUser = patient.ModifiedUser;
                    result.Nationality = patient.Nationality;
                    result.NIC = patient.NIC;
                    result.Religion = patient.Religion;
                    result.Sex = patient.Sex;
                    result.TelephoneNumber = patient.TelephoneNumber;

                    dbContext.SaveChanges();
                }
                return dbContext.Patients.Find(patient.Id);
            }
        }

        public List<Model.Patient> GetAllPatientByStatus()
        {
            List<Model.Patient> mtList = new List<Model.Patient>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Patients.Where(o => o.Status == Model.Enums.PatientStatus.New).ToList<Model.Patient>();

            }
            return mtList;
        }

        public List<PatientsDataTableDTO> GetAllPatientByStatusSP()
        {
            using var context = new HospitalDBContext();

            return context.Set<PatientsDataTableDTO>()
                .FromSqlRaw("EXEC GetAllApprovedPatients")
                .ToList();
        }

        public List<Model.Patient> SearchPatient(string value)
        {
            List<Model.Patient> mtList = new List<Model.Patient>();
            if (value == null) { value = ""; }
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Patients.Where(o => (o.FullName.Contains(value) || o.MobileNumber.Contains(value) || o.NIC.Contains(value)) && o.Status == Model.Enums.PatientStatus.New).ToList<Model.Patient>();

            }
            return mtList;
        }

        public Model.Patient GetAllPatientByID(int? id)
        {
            Model.Patient patient = new Model.Patient();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                patient = dbContext.Patients.First(o => o.Id == id);

            }
            return patient;
        }

        public HospitalMgrSystem.Model.Patient DeletePatient(HospitalMgrSystem.Model.Patient patient)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Patient result = (from p in dbContext.Patients where p.Id == patient.Id select p).SingleOrDefault();
                result.Status = Model.Enums.PatientStatus.Reject;
                dbContext.SaveChanges();
                return result;
            }

        }
    }


}
