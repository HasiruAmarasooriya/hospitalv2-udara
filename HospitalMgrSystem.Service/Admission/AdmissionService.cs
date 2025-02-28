using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Service.Admission
{
    public class AdmissionService : IAdmissionService
    {
        public HospitalMgrSystem.Model.Admission CreateAdmission(HospitalMgrSystem.Model.Admission admission)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (admission.Id == 0)
                {
                    dbContext.Admissions.Add(admission);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.Admission result = (from p in dbContext.Admissions where p.Id == admission.Id select p).SingleOrDefault();
                    result.BHTNumber = admission.BHTNumber;
                    result.BP = admission.BP;
                    result.ConsultantId = admission.ConsultantId;
                    result.DateAdmission = admission.DateAdmission;
                    result.Details = admission.Details;
                    result.Guardian = admission.Guardian;
                    result.PatientId = admission.PatientId;
                    result.Pluse = admission.Pluse;
                    result.Resp = admission.Resp;
                    result.RoomId = admission.RoomId;
                    result.Status = admission.Status;
                    result.Temp = admission.Temp;
                    result.Weight = admission.Weight;
   
                    dbContext.SaveChanges();
                }
                return dbContext.Admissions.Find(admission.Id);
            }
        }

        public List<Model.Admission> GetAllAdmission()
        {
            List<Model.Admission> mtList = new List<Model.Admission>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Admissions.Include(c => c.Patient).Include(c => c.Consultant).Include(c => c.Room).ToList<Model.Admission>();

            }
            return mtList;
        }

        public Model.Admission GetAdmissionByID(int id)
        {
            Model.Admission admission = new Model.Admission();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                admission = dbContext.Admissions.Include(c => c.Patient).Include(c => c.Consultant).
                    Include(c => c.Room).Where(o=> o.Id == id).SingleOrDefault<Model.Admission>();

            }
            return admission;
        }

        public HospitalMgrSystem.Model.Admission DeleteAdmission(HospitalMgrSystem.Model.Admission admission)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.Admission result = (from p in dbContext.Admissions where p.Id == admission.Id select p).SingleOrDefault();
                result.Status = Model.Enums.AdmissionStatus.Reject;
                dbContext.SaveChanges();
                return result;
            }

        }
        public List<Model.Admission> SearchAdmission(string value)
        {
            List<Model.Admission> mtList = new List<Model.Admission>();
            if (value == null) { value = ""; }
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Admissions.Include(c => c.Patient).Include(c => c.Consultant).
                     Include(c => c.Room).Where(o => o.Consultant.Name.Contains(value) ||
                     o.BHTNumber.Contains(value) || o.Details.Contains(value) || o.Patient.FullName.Contains(value)).ToList<Model.Admission>();

            }
            return mtList;
        }

        public HospitalMgrSystem.Model.AdmissionDrugus CreateAdmissionDrugus(HospitalMgrSystem.Model.AdmissionDrugus admissionDrugus)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (admissionDrugus.Id == 0)
                {
                    dbContext.AdmissionDrugus.Add(admissionDrugus);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.AdmissionDrugus result = (from p in dbContext.AdmissionDrugus where p.Id == admissionDrugus.Id select p).SingleOrDefault();
                    result.AdmissionId = admissionDrugus.AdmissionId;
                    result.DrugId = admissionDrugus.DrugId;
                    result.Amount = admissionDrugus.Amount;
                    result.Price = admissionDrugus.Price;
                    result.Qty = admissionDrugus.Qty;
                    result.Type = admissionDrugus.Type;
                    dbContext.SaveChanges();
                }
                return dbContext.AdmissionDrugus.Find(admissionDrugus.Id);
            }
        }

        public List<Model.AdmissionDrugus> GetAdmissionDrugus()
        {
            List<Model.AdmissionDrugus> mtList = new List<Model.AdmissionDrugus>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionDrugus.Include(c => c.Drug).Include(c => c.Admission).Where(c => c.Status == Model.Enums.CommonStatus.Active).ToList<Model.AdmissionDrugus>();

            }
            return mtList;
        }

        public Model.AdmissionDrugus DeleteAdmissionDrugus(HospitalMgrSystem.Model.AdmissionDrugus admissionDrugus)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.AdmissionDrugus result = (from p in dbContext.AdmissionDrugus where p.Id == admissionDrugus.Id select p).SingleOrDefault();
                result.Status = Model.Enums.CommonStatus.Delete;
                dbContext.SaveChanges();
                return result;
            }
        }

        public HospitalMgrSystem.Model.AdmissionInvestigation CreateAdmissionInvestigation(HospitalMgrSystem.Model.AdmissionInvestigation admissionInvestigation)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (admissionInvestigation.Id == 0)
                {
                    admissionInvestigation.CreateDate = DateTime.Now;
                    admissionInvestigation.ModifiedDate = DateTime.Now;
                    dbContext.AdmissionInvestigations.Add(admissionInvestigation);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.AdmissionInvestigation result = (from p in dbContext.AdmissionInvestigations where p.Id == admissionInvestigation.Id select p).SingleOrDefault();
                    result.AdmissionId = admissionInvestigation.AdmissionId;
                    result.InvestigationId = admissionInvestigation.InvestigationId;
                    result.Amount = admissionInvestigation.Amount;
                    result.Price = admissionInvestigation.Price;
                    result.Qty = admissionInvestigation.Qty;
                    result.Type = admissionInvestigation.Type;
                    result.ModifiedDate = DateTime.Now;
                    dbContext.SaveChanges();
                }
                return dbContext.AdmissionInvestigations.Find(admissionInvestigation.Id);
            }
        }

        public List<Model.AdmissionInvestigation> GetAdmissionInvestigation()
        {
            List<Model.AdmissionInvestigation> mtList = new List<Model.AdmissionInvestigation>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionInvestigations.Include(c => c.Investigation).Include(c => c.Admission).Where(c => c.Status == Model.Enums.CommonStatus.Active).ToList<Model.AdmissionInvestigation>();

            }
            return mtList;
        }

        public Model.AdmissionInvestigation DeleteAdmissionInvestigation(HospitalMgrSystem.Model.AdmissionInvestigation admissionInvestigation)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.AdmissionInvestigation result = (from p in dbContext.AdmissionInvestigations where p.Id == admissionInvestigation.Id select p).SingleOrDefault();
                result.Status = Model.Enums.CommonStatus.Delete;
                dbContext.SaveChanges();
                return result;
            }
        }

        #region Consultant Management 
        public HospitalMgrSystem.Model.AdmissionConsultant CreateAdmissionConsultant(HospitalMgrSystem.Model.AdmissionConsultant admissionConsultant)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (admissionConsultant.Id == 0)
                {
                    admissionConsultant.CreateDate = DateTime.Now;
                    admissionConsultant.ModifiedDate = DateTime.Now;
                    dbContext.AdmissionConsultants.Add(admissionConsultant);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.AdmissionConsultant result = (from p in dbContext.AdmissionConsultants where p.Id == admissionConsultant.Id select p).SingleOrDefault();
                    result.AdmissionId = admissionConsultant.AdmissionId;
                    result.ConsultantId = admissionConsultant.ConsultantId;
                    result.Amount = admissionConsultant.Amount;
                    result.Price = admissionConsultant.Price;
                    result.Qty = admissionConsultant.Qty;
                    result.Type = admissionConsultant.Type;
                    result.ModifiedDate = DateTime.Now;
                    dbContext.SaveChanges();
                }
                return dbContext.AdmissionConsultants.Find(admissionConsultant.Id);
            }
        }

        public List<Model.AdmissionConsultant> GetAdmissionConsultant()
        {
            List<Model.AdmissionConsultant> mtList = new List<Model.AdmissionConsultant>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionConsultants.Include(c => c.Consultant).Include(c => c.Admission).Where(c => c.Status == Model.Enums.CommonStatus.Active).ToList<Model.AdmissionConsultant>();

            }
            return mtList;
        }

        public Model.AdmissionConsultant DeleteAdmissionConsultant(HospitalMgrSystem.Model.AdmissionConsultant admissionConsultant)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.AdmissionConsultant result = (from p in dbContext.AdmissionConsultants where p.Id == admissionConsultant.Id select p).SingleOrDefault();
                result.Status = Model.Enums.CommonStatus.Delete;
                dbContext.SaveChanges();
                return result;
            }
        }

        #endregion

        #region Item Management 
        public HospitalMgrSystem.Model.AdmissionItems CreateAdmissionItems(HospitalMgrSystem.Model.AdmissionItems admissionItems)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (admissionItems.Id == 0)
                {
                    admissionItems.CreateDate = DateTime.Now;
                    admissionItems.ModifiedDate = DateTime.Now;
                    dbContext.AdmissionItems.Add(admissionItems);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.AdmissionItems result = (from p in dbContext.AdmissionItems where p.Id == admissionItems.Id select p).SingleOrDefault();
                    result.AdmissionId = admissionItems.AdmissionId;
                    result.ItemId = admissionItems.ItemId;
                    result.Amount = admissionItems.Amount;
                    result.Price = admissionItems.Price;
                    result.Qty = admissionItems.Qty;
                    result.Type = admissionItems.Type;
                    result.ModifiedDate = DateTime.Now;
                    dbContext.SaveChanges();
                }
                return dbContext.AdmissionItems.Find(admissionItems.Id);
            }
        }

        public List<Model.AdmissionItems> GetAdmissionItems()
        {
            List<Model.AdmissionItems> mtList = new List<Model.AdmissionItems>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionItems.Include(c => c.Item).Include(c => c.Admission).Where(c => c.Status == Model.Enums.CommonStatus.Active).ToList<Model.AdmissionItems>();

            }
            return mtList;
        }

        public Model.AdmissionItems DeleteAdmissionItems(HospitalMgrSystem.Model.AdmissionItems admissionItems)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.AdmissionItems result = (from p in dbContext.AdmissionItems where p.Id == admissionItems.Id select p).SingleOrDefault();
                result.Status = Model.Enums.CommonStatus.Delete;
                dbContext.SaveChanges();
                return result;
            }
        }
        #endregion

    }
}
