using System.Linq;
using System.Security.Cryptography;
using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
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
                    result.paymentStatus = admission.paymentStatus;
                    result.invoiceType = admission.invoiceType;
                    result.ModifiedDate = admission.ModifiedDate;
                    result.ModifiedUser = admission.ModifiedUser;
                    result.DischargeDate = admission.DischargeDate;
                    result.itemInvoiceStatus = admission.itemInvoiceStatus; 
                    result.IsRefund = admission.IsRefund;
                    result.Discription = admission.Discription;
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
                    Include(c => c.Room).Where(o => o.Id == id).SingleOrDefault<Model.Admission>();

            }
            return admission;
        }
        public Model.Admission UpdatePaidStatus(Model.Admission admission)
        {

            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (admission.Id != 0)
                    {
                        Model.Admission result = (from p in dbContext.Admissions where p.Id == admission.Id select p).SingleOrDefault();
                        result.paymentStatus = admission.paymentStatus;
                        result.itemInvoiceStatus = admission.itemInvoiceStatus;
                        result.ModifiedUser = admission.ModifiedUser;
                        result.ModifiedDate = DateTime.Now;
                        result.DischargeDate = admission.DischargeDate;
                        dbContext.SaveChanges();
                    }

                    return dbContext.Admissions.Find(admission.Id);
                }
                catch (System.Exception)
                {
                    return null;
                }
            }

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
                    result.Amount = (admissionDrugus.Qty + result.Qty) * admissionDrugus.Price;
                    result.Price = admissionDrugus.Price;
                    result.Qty = admissionDrugus.Qty + result.Qty;
                    result.Type = admissionDrugus.Type;
                    dbContext.SaveChanges();
                }
                return dbContext.AdmissionDrugus.Find(admissionDrugus.Id);
            }
        }

        public List<Model.AdmissionDrugus> GetAdmissionDrugus(int AdminId, PaymentStatus PayStatus)
        {
            List<Model.AdmissionDrugus> mtList = new List<Model.AdmissionDrugus>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionDrugus
                    .Include(c => c.Drug)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == AdminId && c.paymentStatus == PayStatus).ToList<Model.AdmissionDrugus>();
                foreach (var item in mtList)
                {
                    if (item.Drug != null)
                    {
                        item.DrugName = item.Drug.DrugName;
                    }
                }

            }
            return mtList;
        }
        public List<Model.AdmissionDrugus> GetAdmissionDrugusbyAdmissionID(int AdminId)
        {
            List<Model.AdmissionDrugus> mtList = new List<Model.AdmissionDrugus>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionDrugus
                    .Include(c => c.Drug)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == AdminId ).ToList<Model.AdmissionDrugus>();
                foreach (var item in mtList)
                {
                    if (item.Drug != null)
                    {
                        item.DrugName = item.Drug.DrugName;
                    }
                }

            }
            return mtList;
        }
        public List<Model.AdmissionDrugus> GetAdmissionDrugusRemove(int AdminId, ItemInvoiceStatus ItemStatus)
        {
            List<Model.AdmissionDrugus> mtList = new List<Model.AdmissionDrugus>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionDrugus
                    .Include(c => c.Drug)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == AdminId && c.itemInvoiceStatus == ItemStatus).ToList<Model.AdmissionDrugus>();
                foreach (var item in mtList)
                {
                    if (item.Drug != null)
                    {
                        item.DrugName = item.Drug.DrugName;
                    }
                }

            }
            return mtList;
        }
        public Model.AdmissionDrugus GetAdmissionDrugusbyId(int DrugId, int AdminId)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                return dbContext.AdmissionDrugus
                    .Include(c => c.Drug)
                    .Include(c => c.Admission)
                    .FirstOrDefault(c => c.Status == Model.Enums.CommonStatus.Active &&
                                        c.DrugId == DrugId &&
                                        c.AdmissionId == AdminId);
            }
        }
        public Model.AdmissionDrugus GetAdmissionDrugusbyrowId(int Id)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                return dbContext.AdmissionDrugus
                    .Include(c => c.Drug)
                    .Include(c => c.Admission)
                    .FirstOrDefault(c => c.Status == Model.Enums.CommonStatus.Active &&
                                        c.Id == Id);
            }
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
        public Model.AdmissionDrugus UpdateAdmissionDrugus(HospitalMgrSystem.Model.AdmissionDrugus admissionDrugus)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (admissionDrugus.Id != 0)
                    {
                        Model.Admission result = (from p in dbContext.Admissions where p.Id == admissionDrugus.Id select p).SingleOrDefault();
                        result.paymentStatus = admissionDrugus.paymentStatus;
                        result.ModifiedUser = admissionDrugus.ModifiedUser;
                        result.ModifiedDate = DateTime.Now;
                        dbContext.SaveChanges();
                    }

                    return dbContext.AdmissionDrugus.Find(admissionDrugus.Id);
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
        }
        public Model.AdmissionDrugus RefundAdmissionDrugus(int invoiceID, int Id, int userId)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    Invoice invoiceData = (from p in dbContext.Invoices where p.Id == invoiceID select p).SingleOrDefault();
                    AdmissionDrugus result = (from p in dbContext.AdmissionDrugus where p.Id == Id select p).SingleOrDefault();
                    result.paymentStatus = PaymentStatus.NEED_TO_PAY;
                    invoiceData.paymentStatus = PaymentStatus.NEED_TO_PAY;
                    invoiceData.ModifiedUser = userId;
                    invoiceData.ModifiedDate = DateTime.Now;
                    result.ModifiedUser = userId;
                    result.ModifiedDate = DateTime.Now;
                    dbContext.SaveChanges();
                    return result;



                }
                catch (System.Exception)
                {
                    return null;
                }
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
                    result.Amount = (admissionInvestigation.Qty + result.Qty) * admissionInvestigation.Price;
                    result.Price = admissionInvestigation.Price;
                    result.Qty = admissionInvestigation.Qty + result.Qty;
                    result.Type = admissionInvestigation.Type;
                    result.ModifiedDate = DateTime.Now;
                    dbContext.SaveChanges();
                }
                return dbContext.AdmissionInvestigations.Find(admissionInvestigation.Id);
            }
        }

        public List<Model.AdmissionInvestigation> GetAdmissionInvestigationRemove(int AdmissionId, ItemInvoiceStatus ItemStatus)
        {
            List<Model.AdmissionInvestigation> mtList = new List<Model.AdmissionInvestigation>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionInvestigations
                    .Include(c => c.Investigation)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == AdmissionId && c.itemInvoiceStatus == ItemStatus).ToList<Model.AdmissionInvestigation>();
                foreach (var item in mtList)
                {
                    if (item.Investigation != null)
                    {
                        item.InvestigationName = item.Investigation.InvestigationName;
                    }
                }

            }
            return mtList;
        }
        public List<Model.AdmissionInvestigation> GetAdmissionInvestigationbyAdmissionID(int AdmissionId)
        {
            List<Model.AdmissionInvestigation> mtList = new List<Model.AdmissionInvestigation>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionInvestigations
                    .Include(c => c.Investigation)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == AdmissionId ).ToList<Model.AdmissionInvestigation>();
                foreach (var item in mtList)
                {
                    if (item.Investigation != null)
                    {
                        item.InvestigationName = item.Investigation.InvestigationName;
                    }
                }

            }
            return mtList;
        }
        public List<Model.AdmissionInvestigation> GetAdmissionInvestigation(int AdmissionId, PaymentStatus PayStatus)
        {
            List<Model.AdmissionInvestigation> mtList = new List<Model.AdmissionInvestigation>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionInvestigations
                    .Include(c => c.Investigation)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == AdmissionId && c.paymentStatus == PayStatus).ToList<Model.AdmissionInvestigation>();
                foreach (var item in mtList)
                {
                    if (item.Investigation != null)
                    {
                        item.InvestigationName = item.Investigation.InvestigationName;
                    }
                }

            }
            return mtList;
        }
        public Model.AdmissionInvestigation GetAdmissionInvestigationById(int ItemID, int AdmissionID)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                return dbContext.AdmissionInvestigations
                    .Include(c => c.Investigation)
                    .Include(c => c.Admission)
                    .FirstOrDefault(c => c.Status == Model.Enums.CommonStatus.Active &&
                                        c.InvestigationId == ItemID &&
                                        c.AdmissionId == AdmissionID) ?? new Model.AdmissionInvestigation();
            }
        }
        public Model.AdmissionInvestigation RemoveADMInvestigation(int invoiceID,int ItemID,int UserID)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Invoice invoiceData = (from p in dbContext.Invoices where p.Id == invoiceID select p).SingleOrDefault();
                Model.AdmissionInvestigation result = (from p in dbContext.AdmissionInvestigations where p.Id == ItemID select p).SingleOrDefault();
                result.itemInvoiceStatus = ItemInvoiceStatus.Remove;
                result.paymentStatus = PaymentStatus.NEED_TO_PAY;
                result.IsRefund = 1;
                result.ModifiedDate = DateTime.Now;
                invoiceData.paymentStatus = PaymentStatus.NEED_TO_PAY;
                invoiceData.ModifiedDate = DateTime.Now;
                invoiceData.ModifiedUser = UserID;

                dbContext.SaveChanges();
                return result;
            }
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
        public Model.AdmissionInvestigation UpdateAdmissionInvestigation(HospitalMgrSystem.Model.AdmissionInvestigation admissionInvestigation)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (admissionInvestigation.Id != 0) {
                        Model.Admission result = (from p in dbContext.Admissions where p.Id == admissionInvestigation.Id select p).SingleOrDefault();
                        result.paymentStatus = admissionInvestigation.paymentStatus;
                        result.ModifiedUser = admissionInvestigation.ModifiedUser;
                        result.ModifiedDate = DateTime.Now;
                        dbContext.SaveChanges();
                    }
                    return dbContext.AdmissionInvestigations.Find(admissionInvestigation.Id);

                }



                catch (System.Exception)
                {
                    return null;
                }
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
                    result.AdmissionId = result.AdmissionId;
                    result.ConsultantId = result.ConsultantId;
                    result.Amount = admissionConsultant.Amount;
                    result.DoctorFee = admissionConsultant.DoctorFee;
                    result.HospitalFee = admissionConsultant.HospitalFee;
                    result.Type = admissionConsultant.Type;
                    result.ModifiedDate = DateTime.Now;
                    dbContext.SaveChanges();
                }
                return dbContext.AdmissionConsultants.Find(admissionConsultant.Id);
            }
        }

        public List<Model.AdmissionConsultant> GetAdmissionConsultant(int admissionId, PaymentStatus PayStatus)
        {
            List<Model.AdmissionConsultant> mtList = new List<Model.AdmissionConsultant>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionConsultants
                    .Include(c => c.Consultant)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == admissionId && c.paymentStatus == PayStatus)
                    .ToList<Model.AdmissionConsultant>();
                foreach (var item in mtList)
                {
                    if (item.Consultant != null)
                    {
                        item.ConsultantName = item.Consultant.Name;
                    }
                }

            }
            return mtList;
        }
        public List<Model.AdmissionConsultant> GetAdmissionConsultantRemove(int admissionId, ItemInvoiceStatus itemInvoiceStatus)
        {
            List<Model.AdmissionConsultant> mtList = new List<Model.AdmissionConsultant>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionConsultants
                    .Include(c => c.Consultant)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == admissionId && c.itemInvoiceStatus == itemInvoiceStatus)
                    .ToList<Model.AdmissionConsultant>();
                foreach (var item in mtList)
                {
                    if (item.Consultant != null)
                    {
                        item.ConsultantName = item.Consultant.Name;
                    }
                }

            }
            return mtList;
        }
        public List<Model.AdmissionConsultant> GetAdmissionConsultantbyAdmissionID(int admissionId)
        {
            List<Model.AdmissionConsultant> mtList = new List<Model.AdmissionConsultant>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionConsultants
                    .Include(c => c.Consultant)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == admissionId)
                    .ToList<Model.AdmissionConsultant>();
                foreach (var item in mtList)
                {
                    if (item.Consultant != null)
                    {
                        item.ConsultantName = item.Consultant.Name;
                    }
                }
                


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
        public Model.AdmissionConsultant UpdateAdmissionConsultant(HospitalMgrSystem.Model.AdmissionConsultant admissionConsultant)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (admissionConsultant.Id != 0)
                    {
                        Model.AdmissionConsultant result = (from p in dbContext.AdmissionConsultants where p.Id == admissionConsultant.Id select p).SingleOrDefault();
                        result.paymentStatus = admissionConsultant.paymentStatus;
                        result.ModifiedUser = admissionConsultant.ModifiedUser;
                        result.ModifiedDate = DateTime.Now;
                        dbContext.SaveChanges();
                    }
                    return dbContext.AdmissionConsultants.Find(admissionConsultant.Id);
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
        }
        public Model.AdmissionConsultant UpdatePaymentStatusForHospitalAndConsaltantFee(int invoiceID, int Id,int UserId)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    Invoice invoiceData = (from p in dbContext.Invoices where p.Id == invoiceID select p).SingleOrDefault();
                    Model.AdmissionConsultant result = (from p in dbContext.AdmissionConsultants where p.Id == Id select p).SingleOrDefault();
                    if (invoiceData != null && result != null)
                    {
                        result.paymentStatus = PaymentStatus.NEED_TO_PAY;
                        invoiceData.paymentStatus = PaymentStatus.NEED_TO_PAY;
                        invoiceData.ModifiedUser = UserId;
                        invoiceData.ModifiedDate = DateTime.Now;
                        result.ModifiedUser = UserId;
                        result.ModifiedDate = DateTime.Now;

                    }
                    dbContext.SaveChanges();

                    return result;
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
        }
        public Model.AdmissionsCharges UpdatePaymentStatusHospitalFee(int invoiceID, int Id, int UserId)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    Invoice invoiceData = (from p in dbContext.Invoices where p.Id == invoiceID select p).SingleOrDefault();
                    Model.AdmissionsCharges result = (from p in dbContext.AdmissionsCharges where p.Id == Id select p).SingleOrDefault();
                    if (invoiceData != null && result != null)
                    {
                        result.paymentStatus = PaymentStatus.NEED_TO_PAY;
                        invoiceData.paymentStatus = PaymentStatus.NEED_TO_PAY;
                        invoiceData.ModifiedUser = UserId;
                        invoiceData.ModifiedDate = DateTime.Now;
                        result.ModifiedUser = UserId;
                        result.ModifiedDate = DateTime.Now;

                    }
                    dbContext.SaveChanges();

                    return result;
                }
                catch (System.Exception)
                {
                    return null;
                }
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

        public List<Model.AdmissionItems> GetAdmissionItemsRemove(int AdmissionId, ItemInvoiceStatus ItemStatus)
        {
            List<Model.AdmissionItems> mtList = new List<Model.AdmissionItems>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionItems
                    .Include(c => c.Item)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == AdmissionId && c.itemInvoiceStatus == ItemStatus)
                    .ToList<Model.AdmissionItems>();
                foreach (var item in mtList)
                {
                    if (item.Item != null)
                    {
                        item.ItemName = item.Item.ItemName;
                    }
                }

            }
            return mtList;
        }
        public List<Model.AdmissionItems> GetAdmissionItemsbyAdmissionID(int AdmissionId)
        {
            List<Model.AdmissionItems> mtList = new List<Model.AdmissionItems>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionItems
                    .Include(c => c.Item)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == AdmissionId )
                    .ToList<Model.AdmissionItems>();
                foreach (var item in mtList)
                {
                    if (item.Item != null)
                    {
                        item.ItemName = item.Item.ItemName;
                    }
                }

            }
            return mtList;
        }
        public List<Model.AdmissionItems> GetAdmissionItems(int AdmissionId, PaymentStatus payment)
        {
            List<Model.AdmissionItems> mtList = new List<Model.AdmissionItems>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.AdmissionItems
                    .Include(c => c.Item)
                    .Include(c => c.Admission)
                    .Where(c => c.Status == Model.Enums.CommonStatus.Active && c.AdmissionId == AdmissionId && c.paymentStatus == payment)
                    .ToList<Model.AdmissionItems>();
                foreach (var item in mtList)
                {
                    if (item.Item != null)
                    {
                        item.ItemName = item.Item.ItemName;
                    }
                }

            }
            return mtList;
        }
        public Model.AdmissionItems GetAdmissionItemsById(int ItemId, int AdmissionId)
        {
            List<Model.AdmissionItems> mtList = new List<Model.AdmissionItems>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                return dbContext.AdmissionItems
                    .Include(c => c.Item)
                    .Include(c => c.Admission)
                    .FirstOrDefault(c => c.Status == Model.Enums.CommonStatus.Active &&
                                        c.ItemId == ItemId &&
                                        c.AdmissionId == AdmissionId) ?? new Model.AdmissionItems();


            }

        }
        public Model.AdmissionItems RemoveADMItems(int invoiceID, int ItemId, int UserId)
        {

            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Invoice invoiceData = (from p in dbContext.Invoices where p.Id == invoiceID select p).SingleOrDefault();
                Model.AdmissionItems result = (from p in dbContext.AdmissionItems where p.Id == ItemId select p).SingleOrDefault();
                result.paymentStatus = PaymentStatus.NEED_TO_PAY;
                result.ModifiedDate = DateTime.Now;
                result.ModifiedUser =UserId;
                invoiceData.paymentStatus = PaymentStatus.NEED_TO_PAY;
                invoiceData.ModifiedDate = DateTime.Now;
                invoiceData.ModifiedUser = UserId;
                dbContext.SaveChanges();
                return result;

            }

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
        public Model.AdmissionItems UpdateAdmissionItems(HospitalMgrSystem.Model.AdmissionItems admissionItems)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (admissionItems.Id != 0)
                    {
                        Model.AdmissionItems result = (from p in dbContext.AdmissionItems where p.Id == admissionItems.Id select p).SingleOrDefault();
                        result.paymentStatus = admissionItems.paymentStatus;
                        result.ModifiedUser = admissionItems.ModifiedUser;
                        result.ModifiedDate = DateTime.Now;
                        dbContext.SaveChanges();
                    }
                    return dbContext.AdmissionItems.Find(admissionItems.Id);
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
        }
        public Model.Admission GetAllADMByID(int? id)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                Model.Admission opdData = dbContext.Admissions
                                    .Include(o => o.Patient) // Load the Patient related to OPD
                                    .Include(o => o.Consultant) // Load the Consultant related to OPD
                                    .Include(o => o.Consultant!.Specialist)
                                    .SingleOrDefault(o => o.Id == id);

                decimal admDrugsTotal = dbContext.AdmissionDrugus
                                    .Where(o => o.AdmissionId == id && o.Status == CommonStatus.Active && o.paymentStatus == PaymentStatus.PAID)
                                    .Sum(o => o.Amount);
                decimal admInvestigationTotal = dbContext.AdmissionInvestigations
                                    .Where(o => o.AdmissionId == id && o.Status == CommonStatus.Active && o.paymentStatus == PaymentStatus.PAID)
                                    .Sum(o => o.Amount);
                decimal admItemTotal = dbContext.AdmissionItems
                                  .Where(o => o.AdmissionId == id && o.Status == CommonStatus.Active && o.paymentStatus == PaymentStatus.PAID)
                                  .Sum(o => o.Amount);
                decimal admConsultantTotal = dbContext.AdmissionConsultants
                                    .Where(o => o.AdmissionId == id && o.Status == CommonStatus.Active && o.paymentStatus == PaymentStatus.PAID)
                                    .Sum(o => o.Amount);
                opdData.ConsultantFee = dbContext.AdmissionConsultants
                                    .Where(o => o.AdmissionId == id && o.Status == CommonStatus.Active && o.paymentStatus == PaymentStatus.PAID)
                                    .Sum(o => o.DoctorFee);
                opdData.HospitalFee = dbContext.AdmissionConsultants
                                   .Where(o => o.AdmissionId == id && o.Status == CommonStatus.Active && o.paymentStatus == PaymentStatus.PAID)
                                   .Sum(o => o.HospitalFee);
                decimal admTotal = admDrugsTotal + admInvestigationTotal + admItemTotal + admConsultantTotal;


                opdData.TotalAmount = 0;
                opdData.TotalAmount = opdData.TotalAmount + admTotal;

                return opdData;

            }


        }
        #endregion
        #region Admission Hospital Fee

        public HospitalMgrSystem.Model.AdmissionsCharges CreateAdmissionCharge(HospitalMgrSystem.Model.AdmissionsCharges admission)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (admission.Id == 0)
                {

                    dbContext.AdmissionsCharges.Add(admission);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.AdmissionsCharges result = (from p in dbContext.AdmissionsCharges where p.Id == admission.Id select p).SingleOrDefault();
                    result.ItemName = admission.ItemName;
                    result.Price = admission.Price;
                    result.Qty = admission.Qty;
                    result.Amount = admission.Amount;
                    result.AdmissionId = admission.AdmissionId;
                    result.Status = admission.Status;
                    result.paymentStatus = admission.paymentStatus;
                    result.itemInvoiceStatus = admission.itemInvoiceStatus;
                    result.IsRefund = admission.IsRefund;
                    result.CreateUser = admission.CreateUser;
                    result.ModifiedUser = admission.ModifiedUser;
                    result.CreateDate = admission.CreateDate;
                    result.ModifiedDate = admission.ModifiedDate;

                    dbContext.SaveChanges();
                }
                return dbContext.AdmissionsCharges.Find(admission.Id);
            }
        }

        public List<Model.AdmissionHospitalFee> GetAdmissionHospitalChagers()
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                return dbContext.AdmissionHospitalFees
                    .Where(o => o.Status == CommonStatus.Active)
                    .ToList();
            }
        }
        public Model.AdmissionsCharges UpdateAdmissionChargers(HospitalMgrSystem.Model.AdmissionsCharges admissionfee)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (admissionfee.Id != 0)
                    {
                        Model.AdmissionsCharges result = (from p in dbContext.AdmissionsCharges where p.Id == admissionfee.Id select p).SingleOrDefault();
                        result.paymentStatus = admissionfee.paymentStatus;
                        result.ModifiedUser = admissionfee.ModifiedUser;
                        result.ModifiedDate = DateTime.Now;
                        dbContext.SaveChanges();
                    }
                    return dbContext.AdmissionsCharges.Find(admissionfee.Id);
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
        }
        public List<Model.AdmissionsCharges> GetAdmissionHospitalFeesbyId(int AdmissionId)
        {
            List<Model.AdmissionsCharges> mtList = new List<Model.AdmissionsCharges>();

            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                // Retrieve admission details
                var admission = dbContext.Admissions
                    .Where(a => a.Id == AdmissionId)
                    .Select(a => new { a.DischargeDate, a.paymentStatus })
                    .FirstOrDefault();

                if (admission == null)
                {
                    return mtList; // Return empty list if no admission is found
                }

                // Determine the date for calculation
                DateTime referenceDate = (admission.DischargeDate != DateTime.MinValue && admission.paymentStatus == PaymentStatus.NOT_PAID)
                    ? admission.DischargeDate
                    : DateTime.Now;

                // Fetch the charges
                mtList = dbContext.AdmissionsCharges
                    .Include(c => c.Item) // Include related hospital fee details
                    .Include(c => c.Admission) // Include admission details
                    .Where(c => c.AdmissionId == AdmissionId)
                    .ToList();

                // Calculate charges
                foreach (var item in mtList)
                {
                    if (item.Item == null)
                    {
                        continue; // Skip if item is null
                    }

                    // Set ItemName
                    item.ItemName = item.Item.Name;

                    // Calculate the number of days and round up
                    double totalDays = (referenceDate - item.CreateDate).TotalDays;
                    int numberOfDays = (int)Math.Ceiling(totalDays); // Round up to the nearest integer

                    // Calculate the amount based on the fee type
                    if (item.Item.IsDefault == DefaultStatus.IS_DEFAULT)
                    {
                        // For default fees, use the fixed price
                        item.Amount = item.Item.Price;
                    }
                    else
                    {
                        // For non-default fees, calculate based on per-day status and description
                        if (item.Item.PerDayStatus == AdmissionPerDayStatus.IS_Per_Day)
                        {
                            if (item.Item.Description == "Room_Fee")
                            {
                                // Special logic for Room Fee
                                item.Amount = numberOfDays == 0 ? item.Item.Price / 2 : item.Item.Price * numberOfDays;
                            }
                            else
                            {
                                // General per-day calculation
                                item.Amount = item.Item.Price * numberOfDays;
                            }
                        }
                        else
                        {
                            // Non-per-day calculation
                            item.Amount = item.Item.Price;
                        }
                    }
                }
            }

            return mtList;
        }


        public decimal GetAdmissionHospitalFee(int Id, DateTime admissionDate, DateTime dischargeDate)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                // Fetch all active admission fees
                Model.AdmissionHospitalFee admissionFees = dbContext.AdmissionHospitalFees
                    .Where(o => o.Id == Id && o.Status == CommonStatus.Active)
                    .SingleOrDefault();
                decimal price = 0;
                if (admissionFees.IsDefault == DefaultStatus.IS_DEFAULT)
                {
                    price = admissionFees.Price;
                }

                else
                {
                    int numberOfDays = (int)(dischargeDate - admissionDate).TotalDays;
                    if (admissionFees.PerDayStatus == AdmissionPerDayStatus.IS_Per_Day)
                    {
                        if (admissionFees.Description == "Room_Fee")
                        {

                            // If the patient is admitted for less than a day, charge for half a day
                            if (numberOfDays == 0)
                            {
                                price = admissionFees.Price / 2;
                            }
                            else
                            {
                                price = admissionFees.Price * numberOfDays;
                            }
                        }
                        else
                        {
                            price = admissionFees.Price * numberOfDays;
                        }

                    }
                    else
                    {
                        price = admissionFees.Price;
                    }
                }
                return price;
            }
        }

        public Model.AdmissionsCharges RemoveADMChargers(int invoiceId, int Id,int UserID)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                Invoice invoiceData = (from p in dbContext.Invoices where p.Id == invoiceId select p).SingleOrDefault();
                Model.AdmissionsCharges result = dbContext.AdmissionsCharges
                    .Where(o => o.Id == Id)
                    .SingleOrDefault();
                result.paymentStatus = PaymentStatus.NEED_TO_PAY;
                result.ModifiedDate = DateTime.Now;
                result.ModifiedUser = UserID;
                invoiceData.paymentStatus = PaymentStatus.NEED_TO_PAY;
                invoiceData.ModifiedUser = UserID;
                invoiceData.ModifiedDate = DateTime.Now;
                dbContext.SaveChanges();
                return result;
            }
        }
       
        public List<Model.AdmissionsCharges> GetAdmissionHospitalFees(int AdmissionId, PaymentStatus PayStatus)
        {
            List<Model.AdmissionsCharges> mtList = new List<Model.AdmissionsCharges>();

            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                // Retrieve admission details
                var admission = dbContext.Admissions
                    .Where(a => a.Id == AdmissionId && a.paymentStatus == PayStatus)
                    .Select(a => new { a.DischargeDate, a.paymentStatus })
                    .FirstOrDefault();

                if (admission == null)
                {
                    return mtList; // Return empty list if no admission is found
                }

                // Determine the date for calculation
                DateTime referenceDate;

                if (admission.paymentStatus == PaymentStatus.PARTIAL_PAID || admission.DischargeDate == DateTime.MinValue)
                {
                    referenceDate = DateTime.Now;
                }
                else 
                {
                    referenceDate = admission.DischargeDate;
                }
                
                // Fetch the charges
                mtList = dbContext.AdmissionsCharges
                    .Include(c => c.Item) // Include related hospital fee details
                    .Include(c => c.Admission) // Include admission details
                    .Where(c => c.AdmissionId == AdmissionId && c.paymentStatus == PayStatus)
                    .ToList();

                // Calculate charges
                foreach (var item in mtList)
                {
                    if (item.Item == null)
                    {
                        continue; // Skip if item is null
                    }

                    // Set ItemName
                    item.ItemName = item.Item.Name;

                    // Calculate the number of days and round up
                    double totalDays = (referenceDate - item.CreateDate).TotalDays;
                    int numberOfDays = (int)Math.Ceiling(totalDays); // Round up to the nearest integer

                    // Calculate the amount based on the fee type
                    if (item.Item.IsDefault == DefaultStatus.IS_DEFAULT)
                    {
                        // For default fees, use the fixed price
                        item.Amount = item.Item.Price;
                        item.Qty = 1;
                    }
                    else
                    {
                        // For non-default fees, calculate based on per-day status and description
                        if (item.Item.PerDayStatus == AdmissionPerDayStatus.IS_Per_Day)
                        {
                            if (item.Item.Description == "Room_Fee")
                            {
                                // Special logic for Room Fee
                                item.Amount = numberOfDays == 0 ? item.Item.Price / 2 : item.Item.Price * numberOfDays;
                                item.Qty = numberOfDays == 0 ? 0.50m : numberOfDays;
                            }
                            else
                            {
                                // General per-day calculation
                                
                                item.Qty = numberOfDays ==0 ? 1 : numberOfDays;
                                item.Amount = item.Item.Price * item.Qty;
                            }
                        }
                        else
                        {
                            // Non-per-day calculation
                            item.Amount = item.Item.Price;
                            item.Qty = 1;
                        }
                    }
                }
            }

            return mtList;
        }
        public List<Model.AdmissionsCharges> GetAdmissionHospitalFeesRemove(int AdmissionId, ItemInvoiceStatus ItemStatus)
        {
            List<Model.AdmissionsCharges> mtList = new List<Model.AdmissionsCharges>();

            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                // Retrieve admission details
                var admission = dbContext.Admissions
                    .Where(a => a.Id == AdmissionId && a.itemInvoiceStatus == ItemStatus)
                    .Select(a => new { a.DischargeDate, a.paymentStatus,a.CreateDate })
                    .FirstOrDefault();

                if (admission == null)
                {
                    return mtList; // Return empty list if no admission is found
                }

                // Determine the date for calculation
                DateTime referenceDate;

                if (admission.paymentStatus == PaymentStatus.PARTIAL_PAID || admission.paymentStatus ==PaymentStatus.NOT_PAID || admission.DischargeDate == DateTime.MinValue)
                {
                    referenceDate = DateTime.Now;
                }
                else
                {
                    referenceDate = admission.DischargeDate;
                }

                // Fetch the charges
                mtList = dbContext.AdmissionsCharges
                    .Include(c => c.Item) // Include related hospital fee details
                    .Include(c => c.Admission) // Include admission details
                    .Where(c => c.AdmissionId == AdmissionId && c.itemInvoiceStatus == ItemStatus)
                    .ToList();

                // Calculate charges
                foreach (var item in mtList)
                {
                    if (item.Item == null)
                    {
                        continue; // Skip if item is null
                    }

                    // Set ItemName
                    item.ItemName = item.Item.Name;

                    // Calculate the number of days and round up
                    double totalDays = (referenceDate - admission.CreateDate).TotalDays;
                    int numberOfDays = totalDays < 0.5 ? 0 : (int)Math.Ceiling(totalDays); // Round up to the nearest integer

                    // Calculate the amount based on the fee type
                    if (item.Item.IsDefault == DefaultStatus.IS_DEFAULT)
                    {
                        // For default fees, use the fixed price
                        item.Amount = item.Item.Price;
                        item.Qty = 1;
                    }
                    else
                    {
                        // For non-default fees, calculate based on per-day status and description
                        if (item.Item.PerDayStatus == AdmissionPerDayStatus.IS_Per_Day)
                        {
                            if (item.Item.Description == "Room_Fee")
                            {
                                // Special logic for Room Fee
                                item.Amount = numberOfDays == 0 ? item.Item.Price / 2 : item.Item.Price * numberOfDays;
                                item.Qty = numberOfDays == 0 ? 0.50m : numberOfDays;
                            }
                            else
                            {
                                // General per-day calculation

                                item.Qty = numberOfDays == 0 ? 1 : numberOfDays;
                                item.Amount = item.Item.Price * item.Qty;
                            }
                        }
                        else
                        {
                            // Non-per-day calculation
                            item.Amount = item.Item.Price;
                            item.Qty = 1;
                        }
                    }
                }
            }

            return mtList;
        }
        public List<Model.AdmissionsCharges> GetAdmissionHospitalFeesbyAdmissionId(int AdmissionId)
        {
            List<Model.AdmissionsCharges> mtList = new List<Model.AdmissionsCharges>();

            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                // Retrieve admission details
                var admission = dbContext.Admissions
                    .Where(a => a.Id == AdmissionId )
                    .Select(a => new { a.DischargeDate, a.paymentStatus, a.CreateDate })
                    .FirstOrDefault();

                if (admission == null)
                {
                    return mtList; // Return empty list if no admission is found
                }

                // Determine the date for calculation
                DateTime referenceDate;

                if (admission.paymentStatus == PaymentStatus.PARTIAL_PAID || admission.paymentStatus == PaymentStatus.NOT_PAID || admission.DischargeDate == DateTime.MinValue)
                {
                    referenceDate = DateTime.Now;
                }
                else
                {
                    referenceDate = admission.DischargeDate;
                }

                // Fetch the charges
                mtList = dbContext.AdmissionsCharges
                    .Include(c => c.Item) // Include related hospital fee details
                    .Include(c => c.Admission) // Include admission details
                    .Where(c => c.AdmissionId == AdmissionId)
                    .ToList();

                // Calculate charges
                foreach (var item in mtList)
                {
                    if (item.Item == null)
                    {
                        continue; // Skip if item is null
                    }

                    // Set ItemName
                    item.ItemName = item.Item.Name;

                    // Calculate the number of days and round up
                    double totalDays = (referenceDate - admission.CreateDate).TotalDays;
                    int numberOfDays = totalDays < 1 ? 0 : (int)Math.Ceiling(totalDays); // Round up to the nearest integer

                    // Calculate the amount based on the fee type
                    if (item.Item.IsDefault == DefaultStatus.IS_DEFAULT)
                    {
                        // For default fees, use the fixed price
                        item.Amount = item.Item.Price;
                        item.Qty = 1;
                    }
                    else
                    {
                        // For non-default fees, calculate based on per-day status and description
                        if (item.Item.PerDayStatus == AdmissionPerDayStatus.IS_Per_Day)
                        {
                            if (item.Item.Description == "Room_Fee")
                            {
                                // Special logic for Room Fee
                                item.Amount = numberOfDays == 0 ? item.Item.Price / 2 : item.Item.Price * numberOfDays;
                                item.Qty = numberOfDays == 0 ? 0.50m : numberOfDays;
                            }
                            else
                            {
                                // General per-day calculation

                                item.Qty = numberOfDays == 0 ? 1 : numberOfDays;
                                item.Amount = item.Item.Price * item.Qty;
                            }
                        }
                        else
                        {
                            // Non-per-day calculation
                            item.Amount = item.Item.Price;
                            item.Qty = 1;
                        }
                    }
                }
            }

            return mtList;
        }
        #endregion

    }
}
