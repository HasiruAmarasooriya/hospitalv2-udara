using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Service.Consultant
{
    public class ConsultantService : IConsultantService
    {
        public HospitalMgrSystem.Model.Consultant CreateConsultant(HospitalMgrSystem.Model.Consultant consultant)
        {

            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (consultant.Id == 0)
                {
                    dbContext.Consultants.Add(consultant);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.Consultant result = (from p in dbContext.Consultants where p.Id == consultant.Id select p).SingleOrDefault();
                    result.Age = consultant.Age;
                    result.ContectNumber = consultant.ContectNumber;
                    result.Email = consultant.Email;
                    result.Gender = consultant.Gender;
                    result.ModifiedDate = DateTime.Now;
                    result.Name = consultant.Name;
                    result.Specialist = consultant.Specialist;
                    result.SpecialistId = consultant.SpecialistId;
                    result.Status = consultant.Status;
                    result.Address = consultant.Address;
                    dbContext.SaveChanges();
                }
                return dbContext.Consultants.Find(consultant.Id);
            }
        }

        public List<Model.Consultant> GetAllConsultantThatHaveSchedulings()
        {
            List<Model.Consultant> mtList = new List<Model.Consultant>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                // Get doctor ids which have schedulings
                var doctorIds = dbContext.ChannelingSchedule
                    .Where(x => x.Status == Model.Enums.CommonStatus.Active)
                    .Select(x => x.ConsultantId)
                    .Distinct()
                    .ToList();


                mtList = dbContext.Consultants
                    .Include(c => c.Specialist)
                    .Where(o => o.Status == 0 && doctorIds.Contains(o.Id))
                    .ToList();
            }

            return mtList;
        }

        public List<Model.Consultant> GetAllConsultantThatHaveSchedulingsByDate(DateTime scheduleDate)
        {
            List<Model.Consultant> mtList = new List<Model.Consultant>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                // Get doctor ids which have schedulings
                var doctorIds = dbContext.ChannelingSchedule
                    .Where(x => x.Status == Model.Enums.CommonStatus.Active && x.DateTime > scheduleDate)
                    .Select(x => x.ConsultantId)
                    .Distinct()
                    .ToList();


                mtList = dbContext.Consultants
                    .Include(c => c.Specialist)
                    .Where(o => o.Status == 0 && doctorIds.Contains(o.Id))
                    .ToList();
            }

            return mtList;
        }

        public List<Model.Consultant> GetAllConsultantByStatus()
        {
            List<Model.Consultant> mtList = new List<Model.Consultant>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Consultants.Include(c => c.Specialist).Where(o => o.Status == 0).ToList();

            }
            return mtList;
        }

        public List<Model.Consultant> GetAllNotInSystemConsultant()
        {
            List<Model.Consultant> mtList = new List<Model.Consultant>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Consultants.Include(c => c.Specialist).Where(o => o.Status == 0 && o.isSystemDr==0).ToList();

            }
            return mtList;
        }

        public List<Model.Consultant> GetAllOPDConsultantByStatus()
        {
            List<Model.Consultant> mtList = new List<Model.Consultant>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Consultants.Include(c => c.Specialist).Where(o => o.Status == 0 && o.SpecialistId == 43).ToList<Model.Consultant>();

            }
            return mtList;
        }
        public List<Model.Consultant> ConsultantGetBySpecialistId(int id)
        {
            List<Model.Consultant> mtList = new List<Model.Consultant>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Consultants.Include(c => c.Specialist).Where(o => o.Status == 0 && o.SpecialistId == id).ToList<Model.Consultant>();

            }
            return mtList;
        }
        public Model.Consultant GetAllConsultantByID(int? id)
        {
            Model.Consultant consultant = new Model.Consultant();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                consultant = dbContext.Consultants.First(o => o.Id == id);

            }
            return consultant;
        }

        public HospitalMgrSystem.Model.Consultant DeleteConsultant(HospitalMgrSystem.Model.Consultant consultant)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.Consultant result = (from p in dbContext.Consultants where p.Id == consultant.Id select p).SingleOrDefault();
                result.Status = 1;
                dbContext.SaveChanges();
                return result;
            }

        }

        public List<Model.Consultant> SearchConsultant(string value)
        {
            List<Model.Consultant> mtList = new List<Model.Consultant>();
            if (value == null) { value = ""; }
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Consultants.Include(c => c.Specialist).Where(o => (o.Name.Contains(value) || o.ContectNumber.Contains(value)
                || o.Specialist.Name.Contains(value)) && o.Status == 0).ToList<Model.Consultant>();

            }
            return mtList;
        }
    }
}
