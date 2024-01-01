using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.OPD
{
    public class OPDService : IOPDService
    {
        #region OPD Management 
        public Model.OPD CreateOPD(Model.OPD opd)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (opd.Id == 0)
                {
                    dbContext.OPD.Add(opd);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.OPD result = (from p in dbContext.OPD where p.Id == opd.Id select p).SingleOrDefault();
                    result.ModifiedDate = opd.ModifiedDate;
                    result.ModifiedUser = opd.ModifiedUser;
                    result.Id = opd.Id;
                    result.ConsultantFee = opd.ConsultantFee;
                    result.HospitalFee = opd.HospitalFee;
                    result.ConsultantID = opd.ConsultantID;
                    result.RoomID = opd.RoomID;
                    result.AppoimentNo = opd.AppoimentNo;
                    dbContext.SaveChanges();
                }
                return dbContext.OPD.Find(opd.Id);
            }
        }

        public Model.OPD GetAllOPDByID(int? id)
        {
            Model.OPD opd = new Model.OPD();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                opd = dbContext.OPD
             .Include(o => o.patient) // Load the Patient related to OPD
             .Include(o => o.consultant) // Load the Consultant related to OPD
             .SingleOrDefault(o => o.Id == id);

            }
            return opd;
        }

        public List<Model.OPD> GetAllOPDByStatus()
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD.Include(c => c.patient).Include(c => c.consultant).Include(c => c.room).Where(o => o.Status == 0).OrderByDescending(o => o.Id).ToList<Model.OPD>();

            }
            return mtList;
        }

        public List<Model.OPD> GetAllOPDBySessionID()
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD.Include(c => c.patient).Include(c => c.consultant).Include(c => c.room).Where(o => o.Status == 0).OrderByDescending(o => o.Id).ToList<Model.OPD>();

            }
            return mtList;
        }

        public List<Model.OPD> GetAllOPDByDateRange(DateTime startDate, DateTime endDate)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate)
                    .OrderByDescending(o => o.Id)
                    .ToList<Model.OPD>();
            }
            return mtList;
        }

        public List<Model.OPD> GetAllOPDByAndDateRangePaidStatus(DateTime startDate, DateTime endDate)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            // List<Model.OPD> invoiceList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.PAID)
                    .OrderByDescending(o => o.Id)
                    .ToList<Model.OPD>();

                // invoiceList = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.OPD && o.ServiceID == mtList[0].Id).Select(r => r.SubTotal).ToList<Model.OPD>();
                var opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
                var invoiceList = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.OPD && opdIds.Contains(o.ServiceID)).ToList();

                foreach (var item in invoiceList)
                {
                    foreach (var opd in mtList)
                    {
                        if (opd.Id == item.ServiceID)
                        {
                            opd.TotalAmount = item.SubTotal;
                        }
                    }
                }

            }
            return mtList;
        }

        public List<Model.OPD> GetAllOPDByAndDateRangePaidStatusAndOnOPD(DateTime startDate, DateTime endDate)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate)
                    .OrderByDescending(o => o.Id)
                    .ToList<Model.OPD>();
            }
            return mtList;
        }

        public Model.OPD DeleteOPD(Model.OPD opd)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.OPD result = (from p in dbContext.OPD where p.Id == opd.Id select p).SingleOrDefault();
                result.Status = CommonStatus.Delete;
                dbContext.SaveChanges();
                return result;
            }
        }

        public List<Model.OPD> SearchOPD(string value)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Drugs Management 
        public List<Model.OPDDrugus> GetOPDDrugus(int id)
        {
            List<Model.OPDDrugus> mtList = new List<Model.OPDDrugus>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPDDrugus.Include(c => c.Drug).Include(c => c.opd).Where(c => c.Status == Model.Enums.CommonStatus.Active && c.opdId == id).ToList<Model.OPDDrugus>();

            }
            return mtList;
        }

        public List<Model.OPDDrugus> GetOPDDrugusByInvoiceStatus(int id, ItemInvoiceStatus invoiceStatus)
        {
            List<Model.OPDDrugus> mtList = new List<Model.OPDDrugus>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPDDrugus.Include(c => c.Drug).Include(c => c.opd).Where(c => c.Status == Model.Enums.CommonStatus.Active && c.opdId == id && c.itemInvoiceStatus == invoiceStatus).ToList<Model.OPDDrugus>();

            }
            return mtList;
        }
        public Model.OPDDrugus CreateOPDDrugus(Model.OPDDrugus opdDrugus)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (opdDrugus.Id == 0)
                {
                    dbContext.OPDDrugus.Add(opdDrugus);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.OPDDrugus result = (from p in dbContext.OPDDrugus where p.Id == opdDrugus.Id select p).SingleOrDefault();
                    result.opdId = opdDrugus.opdId;
                    result.DrugId = opdDrugus.DrugId;
                    result.Amount = opdDrugus.Amount;
                    result.Price = opdDrugus.Price;
                    result.Qty = opdDrugus.Qty;
                    result.Type = opdDrugus.Type;
                    dbContext.SaveChanges();
                }
                return dbContext.OPDDrugus.Find(opdDrugus.Id);
            }
        }
        public Model.OPDDrugus DeleteOPDDrugus(int opdDruguID)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.OPDDrugus result = (from p in dbContext.OPDDrugus where p.Id == opdDruguID select p).SingleOrDefault();
                result.Status = Model.Enums.CommonStatus.Delete;
                dbContext.SaveChanges();
                return result;
            }
        }
        public Model.OPDDrugus RemoveOPDDrugus(int Id)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.OPDDrugus result = (from p in dbContext.OPDDrugus where p.Id == Id select p).SingleOrDefault();
                result.itemInvoiceStatus = Model.Enums.ItemInvoiceStatus.Remove;
                result.IsRefund = 1;
                dbContext.SaveChanges();
                return result;
            }
        }
        public Model.OPDDrugus AddOPDDrugus(int Id)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.OPDDrugus result = (from p in dbContext.OPDDrugus where p.Id == Id select p).SingleOrDefault();
                result.itemInvoiceStatus = Model.Enums.ItemInvoiceStatus.Add;
                dbContext.SaveChanges();
                return result;
            }
        }
        #endregion

        #region Investigation Management 
        public List<Model.OPDInvestigation> GetOPDInvestigation(int id)
        {

            List<Model.OPDInvestigation> mtList = new List<Model.OPDInvestigation>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPDInvestigations.Include(c => c.Investigation).Include(c => c.opd).Where(c => c.Status == Model.Enums.CommonStatus.Active && c.opdId == id).ToList<Model.OPDInvestigation>();

            }
            return mtList;
        }
        public List<Model.OPDInvestigation> GetOPDInvestigationByInvoiceStatus(int id, ItemInvoiceStatus invoiceStatus)
        {

            List<Model.OPDInvestigation> mtList = new List<Model.OPDInvestigation>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPDInvestigations.Include(c => c.Investigation).Include(c => c.opd).Where(c => c.Status == Model.Enums.CommonStatus.Active && c.opdId == id && c.itemInvoiceStatus == invoiceStatus).ToList<Model.OPDInvestigation>();

            }
            return mtList;
        }
        public Model.OPDInvestigation CreateOPDInvestigation(Model.OPDInvestigation opdnInvestigation)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (opdnInvestigation.Id == 0)
                {
                    opdnInvestigation.CreateDate = DateTime.Now;
                    opdnInvestigation.ModifiedDate = DateTime.Now;
                    dbContext.OPDInvestigations.Add(opdnInvestigation);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.OPDInvestigation result = (from p in dbContext.OPDInvestigations where p.Id == opdnInvestigation.Id select p).SingleOrDefault();
                    result.opdId = opdnInvestigation.opdId;
                    result.InvestigationId = opdnInvestigation.InvestigationId;
                    result.Amount = opdnInvestigation.Amount;
                    result.Price = opdnInvestigation.Price;
                    result.Qty = opdnInvestigation.Qty;
                    result.Type = opdnInvestigation.Type;
                    result.ModifiedDate = DateTime.Now;
                    dbContext.SaveChanges();
                }
                return dbContext.OPDInvestigations.Find(opdnInvestigation.Id);
            }
        }

        public Model.OPDInvestigation DeleteOPDInvestigation(int opdDruguID)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.OPDInvestigation result = (from p in dbContext.OPDInvestigations where p.Id == opdDruguID select p).SingleOrDefault();
                result.Status = Model.Enums.CommonStatus.Delete;
                dbContext.SaveChanges();
                return result;
            }
        }
        public Model.OPDInvestigation RemoveOPDInvestigation(int Id)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.OPDInvestigation result = (from p in dbContext.OPDInvestigations where p.Id == Id select p).SingleOrDefault();
                result.itemInvoiceStatus = Model.Enums.ItemInvoiceStatus.Remove;
                dbContext.SaveChanges();
                return result;
            }
        }
        public Model.OPDInvestigation AddOPDInvestigation(int Id)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.OPDInvestigation result = (from p in dbContext.OPDInvestigations where p.Id == Id select p).SingleOrDefault();
                result.itemInvoiceStatus = Model.Enums.ItemInvoiceStatus.Add;
                dbContext.SaveChanges();
                return result;
            }
        }
        #endregion

        #region Items Management 
        public List<Model.OPDItem> GetOPDItems(int id)
        {
            List<Model.OPDItem> mtList = new List<Model.OPDItem>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPDItems.Include(c => c.Item).Include(c => c.opd).Where(c => c.Status == Model.Enums.CommonStatus.Active && c.opdId == id).ToList<Model.OPDItem>();

            }
            return mtList;
        }
        public List<Model.OPDItem> GetOPDItemsByInvoiceStatus(int id, ItemInvoiceStatus invoiceStatus)
        {
            List<Model.OPDItem> mtList = new List<Model.OPDItem>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPDItems.Include(c => c.Item).Include(c => c.opd).Where(c => c.Status == Model.Enums.CommonStatus.Active && c.opdId == id && c.itemInvoiceStatus == invoiceStatus).ToList<Model.OPDItem>();

            }
            return mtList;
        }
        public Model.OPDItem CreateOPDItems(Model.OPDItem opdItems)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (opdItems.Id == 0)
                {
                    opdItems.CreateDate = DateTime.Now;
                    opdItems.ModifiedDate = DateTime.Now;
                    dbContext.OPDItems.Add(opdItems);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.OPDItem result = (from p in dbContext.OPDItems where p.Id == opdItems.Id select p).SingleOrDefault();
                    result.opdId = opdItems.opdId;
                    result.ItemId = opdItems.ItemId;
                    result.Amount = opdItems.Amount;
                    result.Price = opdItems.Price;
                    result.Qty = opdItems.Qty;
                    result.Type = opdItems.Type;
                    result.ModifiedDate = DateTime.Now;
                    dbContext.SaveChanges();
                }
                return dbContext.OPDItems.Find(opdItems.Id);
            }
        }
        public Model.OPDItem DeleteOPDItems(int opdDruguID)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.OPDItem result = (from p in dbContext.OPDItems where p.Id == opdDruguID select p).SingleOrDefault();
                result.Status = Model.Enums.CommonStatus.Delete;
                dbContext.SaveChanges();
                return result;
            }
        }
        public Model.OPDItem RemoveOPDItems(int Id)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.OPDItem result = (from p in dbContext.OPDItems where p.Id == Id select p).SingleOrDefault();
                result.itemInvoiceStatus = Model.Enums.ItemInvoiceStatus.Remove;
                dbContext.SaveChanges();
                return result;
            }
        }

        public Model.OPDItem AddOPDItems(int Id)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.OPDItem result = (from p in dbContext.OPDItems where p.Id == Id select p).SingleOrDefault();
                result.itemInvoiceStatus = Model.Enums.ItemInvoiceStatus.Add;
                dbContext.SaveChanges();
                return result;
            }
        }
        #endregion


        public void printme(string opdID)
        {
            PrintDocument recordDoc = new PrintDocument();
            recordDoc.DocumentName = "QR";
            recordDoc.PrintController = new StandardPrintController();

            recordDoc.Print();

        }
    }
}
