using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.OPD
{
    public class OPDService : IOPDService
    {
        #region Shift Management

        public void UpdateShift()
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                NightShift nightShift = dbContext.NightShifts.First(o => o.Id == 1);

                if (nightShift.IsNightShift == Shift.DAY_SHIFT)
                {
                    nightShift.IsNightShift = Shift.NIGHT_SHIFT;
                }
                else
                {
                    nightShift.IsNightShift = Shift.DAY_SHIFT;
                }

                dbContext.SaveChanges();
            }

        }

        #endregion

        #region OPD Management 
        public Model.OPD CreateOPD(Model.OPD opd)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                if (opd.Id == 0)
                {
                    dbContext.OPD.Add(opd);
                    dbContext.SaveChanges();
                }
                else
                {
                    Model.OPD result = (from p in dbContext.OPD where p.Id == opd.Id select p).SingleOrDefault();
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

        public Model.OPD UpdateOPDStatus(Model.OPD opd, List<OPDDrugus> oPDDrugs)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {

                Model.OPD result = (from p in dbContext.OPD where p.Id == opd.Id select p).SingleOrDefault();
                Invoice invoiceData = (from p in dbContext.Invoices where p.ServiceID == opd.Id && p.InvoiceType == InvoiceType.OPD select p).SingleOrDefault();

                if (invoiceData != null)
                {
                    List<InvoiceItem> invoiceItemData = (from p in dbContext.InvoiceItems where p.InvoiceId == invoiceData.Id select p).ToList();
                    List<OPDDrugus> oPDDrugus = (from p in dbContext.OPDDrugus where p.opdId == opd.Id select p).ToList();

                    var invoiceItemDataTotal = invoiceItemData.Sum(o => o.Total);
                    var previousDrugsTotal = oPDDrugus.Sum(o => o.Amount);

                    var drugsTotal = 0;
                    foreach (var drugItem in oPDDrugs)
                    {
                        // Invoice table eketh update karanna
                        previousDrugsTotal += drugItem.Amount;
                    }

                    if (invoiceItemDataTotal < previousDrugsTotal)
                    {
                        result.paymentStatus = PaymentStatus.NOT_PAID;
                        invoiceData.paymentStatus = PaymentStatus.NOT_PAID;
                    }
                }

                result.ModifiedDate = opd.ModifiedDate;
                result.ModifiedUser = opd.ModifiedUser;
                result.Id = opd.Id;
                result.ConsultantFee = opd.ConsultantFee;
                result.HospitalFee = opd.HospitalFee;
                result.ConsultantID = opd.ConsultantID;
                result.RoomID = opd.RoomID;
                result.AppoimentNo = opd.AppoimentNo;
                result.invoiceType = opd.invoiceType;
                dbContext.SaveChanges();

                return dbContext.OPD.Find(opd.Id);
            }
        }

        public Model.OPD UpdatePaidStatus(Model.OPD opd)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (opd.Id != 0)
                    {
                        HospitalMgrSystem.Model.OPD result = (from p in dbContext.OPD where p.Id == opd.Id select p).SingleOrDefault();
                        result.paymentStatus = opd.paymentStatus; // Update only the "Price" column
                        result.ModifiedDate = DateTime.Now; // Optional, set the ModifiedDate if needed
                        result.ModifiedUser = opd.ModifiedUser; // Optional, set the ModifiedDate if needed
                        dbContext.SaveChanges();
                    }
                    return dbContext.OPD.Find(opd.Id);
                }
                catch (Exception ex)
                {
                    return null;
                }


            }
        }

        public Model.Invoice UpdateOPDDrugInvoiceStatus(List<Model.InvoiceItem> invoiceItems)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                OPDDrugus opdDrug = new OPDDrugus();
                try
                {

                    opdDrug.Id = invoiceItems[0].ItemID;
                    foreach (var item in invoiceItems)
                    {

                        if (item.billingItemsType == BillingItemsType.Drugs)
                        {
                            HospitalMgrSystem.Model.OPDDrugus result = (from p in dbContext.OPDDrugus where p.Id == item.ItemID select p).SingleOrDefault();
                            if (result != null)
                            {
                                result.itemInvoiceStatus = item.itemInvoiceStatus; // Update only the "Price" column
                                dbContext.SaveChanges();
                            }
                        }

                    }

                    return dbContext.Invoices.Find(opdDrug.Id);

                }
                catch (Exception ex)
                {
                    return null;
                }



            }
        }

        public Model.OPD GetAllOPDByID(int? id)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                Model.OPD opdData = dbContext.OPD
                                    .Include(o => o.patient) // Load the Patient related to OPD
                                    .Include(o => o.consultant) // Load the Consultant related to OPD
                                    .Include(o => o.nightShiftSession) // Load the nightShiftSession
                                    .SingleOrDefault(o => o.Id == id);

                opdData.TotalAmount = 0;
                opdData.TotalAmount = opdData.TotalAmount + opdData.HospitalFee;
                opdData.TotalAmount = opdData.TotalAmount + opdData.ConsultantFee;

                return opdData;

            }
            
        }

        public List<Model.OPD> GetAllOPDByStatus()
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD.Include(c => c.patient).Include(c => c.consultant).Include(c => c.room).Where(o => o.Status == 0 && o.invoiceType == InvoiceType.OPD).OrderByDescending(o => o.Id).ToList();

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

        public List<Model.OPD> GetAllOPDBySchedularID(int schedularId)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD.Include(c => c.patient).Include(c => c.consultant).Include(c => c.room).Where(o => o.Status == 0 && o.schedularId == schedularId).OrderByDescending(o => o.Id).ToList();

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

        //public List<Model.OPD> GetAllOPDByAndDateRangePaidStatus(DateTime startDate, DateTime endDate, PaymentStatus paymentStatus)
        //{
        //    List<Model.OPD> mtList = new List<Model.OPD>();
        //    // List<Model.OPD> invoiceList = new List<Model.OPD>();
        //    using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
        //    {
        //        mtList = dbContext.OPD
        //            .Include(c => c.patient)
        //            .Include(c => c.consultant)
        //            .Include(c => c.room)
        //            .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == paymentStatus)
        //            .OrderByDescending(o => o.Id)
        //            .ToList<Model.OPD>();

        //        // invoiceList = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.OPD && o.ServiceID == mtList[0].Id).Select(r => r.SubTotal).ToList<Model.OPD>();
        //        var opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
        //        var invoiceList = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.OPD && opdIds.Contains(o.ServiceID)).ToList();

        //        foreach (var item in invoiceList)
        //        {
        //            foreach (var opd in mtList)
        //            {
        //                if (opd.Id == item.ServiceID)
        //                {
        //                    opd.TotalAmount = 0;
        //                }
        //            }
        //        }

        //    }
        //    return mtList;
        //}


        public List<Model.OPD> GetAllOPDByAndDateRangePaidStatus(DateTime startDate, DateTime endDate, PaymentStatus paymentStatus)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            List<Model.OPD> newmtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == paymentStatus)
                    .OrderByDescending(o => o.Id)
                    .ToList<Model.OPD>();

                var opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
                var invoiceList = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.OPD && opdIds.Contains(o.ServiceID)).ToList();

                foreach (var item in invoiceList)
                {
                    Model.OPD opdObj = new Model.OPD();
                    opdObj = mtList.Where(o => o.Id == item.ServiceID).SingleOrDefault();

                    var invoiceItemList = dbContext.InvoiceItems.Where(o => o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED).ToList();
                    opdObj.TotalAmount = 0;
                    foreach (var invoiceItem in invoiceItemList)
                    {
                        opdObj.TotalAmount = opdObj.TotalAmount + invoiceItem.Total;

                    }

                    newmtList.Add(opdObj);
                }

            }
            return newmtList;
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

        public Model.OPD DeleteOPD(int Id)
        {
            using DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext();
            Model.OPD result = (from p in dbContext.OPD where p.Id == Id select p).SingleOrDefault();
            result.Status = CommonStatus.Delete;
            dbContext.SaveChanges();
            return result;
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
                mtList = dbContext.OPDDrugus.Include(c => c.Drug).Include(c => c.opd).Where(c => c.Status == Model.Enums.CommonStatus.Active && c.opdId == id && c.IsRefund == 0).ToList<Model.OPDDrugus>();

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
        public OPDDrugus CreateOPDDrugus(OPDDrugus opdDrugus)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                if (opdDrugus.Id == 0)
                {
                    opdDrugus.CreateDate = DateTime.Now;
                    opdDrugus.ModifiedDate = DateTime.Now;
                    dbContext.OPDDrugus.Add(opdDrugus);
                    dbContext.SaveChanges();
                }
                else
                {
                    OPDDrugus result = (from p in dbContext.OPDDrugus where p.Id == opdDrugus.Id select p).SingleOrDefault();
                    result.ModifiedDate = DateTime.Now;
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
        public Model.OPDDrugus RemoveOPDDrugus(int Id,int userId)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                OPDDrugus result = (from p in dbContext.OPDDrugus where p.Id == Id select p).SingleOrDefault();
                result.itemInvoiceStatus = ItemInvoiceStatus.Remove;
                result.IsRefund = 1;
                result.ModifiedDate = DateTime.Now;
                result.ModifiedUser= userId;
                dbContext.SaveChanges();
                return result;
            }
        }

        public Model.OPDDrugus UpdatePaymentStatus(int Id, decimal cashierSubTotal, InvoiceType invoiceType)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                Invoice invoiceData = new Invoice();
                OPDDrugus result = (from p in dbContext.OPDDrugus where p.Id == Id select p).SingleOrDefault();
                Model.OPD opdData = (from p in dbContext.OPD where p.Id == result.opdId select p).SingleOrDefault();
                if(invoiceType == InvoiceType.OPD)
                {
                    invoiceData = (from p in dbContext.Invoices where p.ServiceID == opdData.Id && p.InvoiceType == InvoiceType.OPD select p).SingleOrDefault();
                }
                if (invoiceType == InvoiceType.CHE)
                {
                    invoiceData = (from p in dbContext.Invoices where p.ServiceID == opdData.Id && p.InvoiceType == InvoiceType.CHE select p).SingleOrDefault();
                }
                if(invoiceData != null)
                {
                    if (cashierSubTotal < 0)
                    {
                        opdData.paymentStatus = PaymentStatus.NEED_TO_PAY;
                        invoiceData.paymentStatus = PaymentStatus.NEED_TO_PAY;
                    }

                    result.itemInvoiceStatus = ItemInvoiceStatus.Remove;
                    result.IsRefund = 1;
                    dbContext.SaveChanges();
                    return result;
                }

                return null;

            }
        }


        public Model.Invoice UpdatePaymentStatusForHospitalAndConsaltantFee(int invoiceID, int opdID)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {

                Model.OPD opdData = (from p in dbContext.OPD where p.Id == opdID select p).SingleOrDefault();
                Invoice invoiceData = (from p in dbContext.Invoices where p.Id == invoiceID select p).SingleOrDefault();

                if (invoiceData != null && opdData != null)
                {
                    opdData.paymentStatus = PaymentStatus.NEED_TO_PAY;
                    invoiceData.paymentStatus = PaymentStatus.NEED_TO_PAY;
                }
                dbContext.SaveChanges();
                return invoiceData;
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
