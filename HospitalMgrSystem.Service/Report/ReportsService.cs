using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Report
{
    public class DrugDataTemp
    {
        public int DrugId { get; set; }
        public decimal Qty { get; set; }
        // Add other properties as needed
        public Drug drugData { get; set; }
    }

    public class ReportsService
    {
        #region PAYMENT REPORTS
        public Payment GetAllPaymentsData(DateTime startDate, DateTime endDate)
        {
            Payment paymentData = new Payment();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {


                var opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
                var invoiceList = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.OPD && opdIds.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                var opdIdsNeedToPay = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NEED_TO_PAY).Select(r => r.Id).ToList();
                var invoiceListNeedToPay = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.OPD && opdIdsNeedToPay.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                var opdIdsNotPaid = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NOT_PAID).Select(r => r.Id).ToList();
                var invoiceListNotPaid = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.OPD && opdIdsNotPaid.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                var totalCashierAmount = dbContext.Payments
                    .Where(o =>  o.BillingType == BillingType.CASHIER && invoiceList.Contains(o.InvoiceID))
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                var totalBalanceAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.BALENCE && invoiceList.Contains(o.InvoiceID))
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                var totalRefundAmount = dbContext.Payments
                    .Where(o =>  o.BillingType == BillingType.REFUND && invoiceList.Contains(o.InvoiceID))
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                var totalRefundAmountInNeedToPay = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.REFUND && invoiceListNeedToPay.Contains(o.InvoiceID))
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                var removedItemSumNeedToPay = dbContext.InvoiceItems.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNeedToPay.Contains(o.InvoiceId)).Sum(o => o.price * o.qty);
                var removedItemSumRefund = dbContext.InvoiceItems.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNotPaid.Contains(o.InvoiceId)).Sum(o => o.price * o.qty);
                //var totalOtherInAmount = dbContext.Payments
                //    .Where(o => o.ModifiedDate >= startDate && o.ModifiedDate <= endDate && o.BillingType == BillingType.OTHER_IN)
                //    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                //var totalOtherOutAmount = dbContext.Payments
                //    .Where(o => o.ModifiedDate >= startDate && o.ModifiedDate <= endDate && o.BillingType == BillingType.OTHER_OUT)
                //    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                //var totalAmount = totalCashierAmount + totalOtherInAmount + totalBalanceAmount;
                //var totalPaidAmount = totalAmount + totalRefundAmount + totalOtherOutAmount;

                var totalRefund = removedItemSumNeedToPay + removedItemSumRefund;
                var totalAmount = totalCashierAmount  + totalBalanceAmount + removedItemSumNeedToPay+ removedItemSumRefund;
                var totalPaidAmount = totalAmount + totalRefundAmount ;

                var oldPaidAmount = totalPaidAmount - (totalRefundAmount + totalRefundAmountInNeedToPay);
                var actRefundAmount = oldPaidAmount - totalPaidAmount;
                var needToPayTotal = totalRefund - actRefundAmount;

                paymentData.TotalAmountOld = oldPaidAmount;
                paymentData.TotalAmountNeedToPay = needToPayTotal;
                paymentData.TotalAmountRefunded = actRefundAmount;
                paymentData.TotalAmount = totalAmount;
                paymentData.TotalPaidAmount = totalPaidAmount;
                paymentData.TotalRefund = totalRefund;
            }
            return paymentData;
        }

        #endregion

        #region CHANNELING REPORTS

        public List<Model.OPD> GetAllChannelingByDateRangePaidStatus(DateTime startDate, DateTime endDate)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            List<Model.OPD> newmtList = new List<Model.OPD>();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.nightShiftSession.User)
                    .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.PAID)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                var opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
                var invoiceList = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID)).ToList();

                foreach (var item in invoiceList)
                {
                    Model.OPD opdObj = new Model.OPD();
                    opdObj = mtList.Where(o => o.Id == item.ServiceID).SingleOrDefault();

                    var invoiceItemList = dbContext.InvoiceItems.Where(o => o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED).ToList();
                    Model.User user = new Model.User();
                    user = GetUserById(opdObj.ModifiedUser);

                    opdObj.issuedUser = user;
                    opdObj.invoiceID = item.Id;
                    opdObj.TotalAmount = 0;
                    opdObj.cashier = opdObj.nightShiftSession.User;
                    opdObj.TotalPaidAmount = 0;
                    opdObj.deviation = 0;
                    foreach (var invoiceItem in invoiceItemList)
                    {
                        opdObj.TotalAmount = opdObj.TotalAmount + invoiceItem.Total;

                    }

                    var paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
                    foreach (var payment in paymentList)
                    {
                        decimal paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount + payment.ChequeAmount + payment.GiftCardAmount;
                        opdObj.TotalPaidAmount = opdObj.TotalPaidAmount + paidAmount;

                    }
                    opdObj.deviation = opdObj.TotalPaidAmount - opdObj.TotalAmount;
                    newmtList.Add(opdObj);
                }

            }
            return newmtList;
        }

        public List<Model.OPD> GetAllChannelingByDateRangeAndNeedToPayStatus(DateTime startDate, DateTime endDate)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            List<Model.OPD> newmtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.nightShiftSession.User)
                    .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NEED_TO_PAY)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                var opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NEED_TO_PAY).Select(r => r.Id).ToList();
                var invoiceList = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.CHE && opdIds.Contains(o.ServiceID)).ToList();

                foreach (var item in invoiceList)
                {
                    Model.OPD opdObj = new Model.OPD();
                    opdObj = mtList.Where(o => o.Id == item.ServiceID).SingleOrDefault();

                    var invoiceItemList = dbContext.InvoiceItems.Where(o => o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.Remove).ToList();
                    Model.User user = new Model.User();
                    user = GetUserById(opdObj.ModifiedUser);

                    opdObj.issuedUser = user;
                    opdObj.invoiceID = item.Id;
                    opdObj.TotalAmount = 0;
                    opdObj.TotalRefund = 0;
                    opdObj.cashier = opdObj.nightShiftSession.User;
                    opdObj.TotalPaidAmount = 0;
                    opdObj.deviation = 0;
                    foreach (var invoiceItem in invoiceItemList)
                    {
                        opdObj.TotalRefund = opdObj.TotalRefund + invoiceItem.Total;

                    }

                    var paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
                    decimal tRefund = 0;
                    foreach (var payment in paymentList)
                    {
                        decimal paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount + payment.ChequeAmount + payment.GiftCardAmount;
                        if (payment.BillingType == BillingType.REFUND)
                        {
                            tRefund = tRefund + paidAmount;
                        }
                        opdObj.TotalPaidAmount = opdObj.TotalPaidAmount + paidAmount;

                    }
                    opdObj.TotalNeedToRefund = opdObj.TotalRefund + tRefund;
                    opdObj.TotalOldAmount = opdObj.TotalPaidAmount - tRefund;
                    newmtList.Add(opdObj);
                }

            }
            return newmtList;
        }

        public List<Model.OPD> GetAllChannelingByDateRangeAndNotPaidStatus(DateTime startDate, DateTime endDate)
        {

            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NOT_PAID && o.invoiceType == InvoiceType.CHE)
                    .OrderByDescending(o => o.Id)
                    .ToList();
            }
            return mtList;
        }

        #endregion

        #region OPD REPORTS

        public List<Model.OPD> GetAllOPDByDateRange(DateTime startDate, DateTime endDate)
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
                    .ToList();
            }
            return mtList;
        }

        public List<OPDDrugus> GetAllOPDGrugsByDateRangePaidStatus(DateTime startDate, DateTime endDate)
        {
            List<OPDDrugus> mtList = new List<OPDDrugus>();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                var temp = dbContext.OPDDrugus
                   .Include(c => c.opd)
                   .Include(c => c.Drug)
                   .Where(o => o.Status == CommonStatus.Active && o.ModifiedDate >= startDate && o.ModifiedDate <= endDate && o.IsRefund == 0 && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED)
                   .GroupBy(o => o.DrugId)
                   .Select(r => new
                   {
                       DrugId = r.Key,
                       Qty = r.Sum(x => x.Qty),
                       DrugData = r.Select(x => x.Drug).FirstOrDefault(),
                       Total = r.Sum(x => x.Amount)
                   }).ToList();

                foreach (var item in temp)
                {
                    OPDDrugus opdDrug = new OPDDrugus();
                    opdDrug.DrugId = item.DrugId;
                    opdDrug.Qty = item.Qty;
                    opdDrug.Drug = item.DrugData;
                    opdDrug.Amount = item.Total;
                    mtList.Add(opdDrug);
                }

                return mtList;
            }
        }

        public List<Model.OPD> GetAllOPDByDateRangeAndNotPaidStatus(DateTime startDate, DateTime endDate)
        {

            List<Model.OPD> mtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NOT_PAID && o.invoiceType == InvoiceType.OPD)
                    .OrderByDescending(o => o.Id)
                    .ToList();
            }
            return mtList;
        }

        public List<Model.OPD> GetAllOPDByDateRangeAndNightShiftStatus(DateTime startDate, DateTime endDate)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            List<Model.OPD> newmtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.nightShiftSession.User)
                    .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.paymentStatus == PaymentStatus.OPD)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                var opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate  && o.paymentStatus == PaymentStatus.OPD).Select(r => r.Id).ToList();
                var invoiceList = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.OPD && opdIds.Contains(o.ServiceID)).ToList();

                foreach (var item in invoiceList)
                {
                    Model.OPD opdObj = new Model.OPD();
                    opdObj = mtList.Where(o => o.Id == item.ServiceID).SingleOrDefault();

                    var invoiceItemList = dbContext.InvoiceItems.Where(o => o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.BILLED).ToList();
                    Model.User user = new Model.User();
                    user = GetUserById(opdObj.ModifiedUser);

                    opdObj.issuedUser = user;
                    opdObj.invoiceID = item.Id;
                    opdObj.TotalAmount = 0;
                    opdObj.cashier = opdObj.nightShiftSession.User;
                    opdObj.TotalPaidAmount = 0;
                    opdObj.deviation = 0;
                    foreach (var invoiceItem in invoiceItemList)
                    {
                        opdObj.TotalAmount = opdObj.TotalAmount + invoiceItem.Total;

                    }

                    var paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
                    foreach (var payment in paymentList)
                    {
                        decimal paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount + payment.ChequeAmount + payment.GiftCardAmount;
                        opdObj.TotalPaidAmount = opdObj.TotalPaidAmount + paidAmount;

                    }
                    opdObj.deviation = opdObj.TotalPaidAmount - opdObj.TotalAmount;
                    newmtList.Add(opdObj);
                }

            }
            return newmtList;
        }


        public List<Model.OPD> GetAllOPDByDateRangeAndNeedToPayStatus(DateTime startDate, DateTime endDate)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            List<Model.OPD> newmtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.nightShiftSession.User)
                    .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NEED_TO_PAY)
                    .OrderByDescending(o => o.Id)
                    .ToList();

                var opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NEED_TO_PAY).Select(r => r.Id).ToList();
                var invoiceList = dbContext.Invoices.Where(o => o.InvoiceType == InvoiceType.OPD && opdIds.Contains(o.ServiceID)).ToList();

                foreach (var item in invoiceList)
                {
                    Model.OPD opdObj = new Model.OPD();
                    opdObj = mtList.Where(o => o.Id == item.ServiceID).SingleOrDefault();

                    var invoiceItemList = dbContext.InvoiceItems.Where(o => o.InvoiceId == item.Id && o.itemInvoiceStatus == ItemInvoiceStatus.Remove).ToList();
                    Model.User user = new Model.User();
                    user = GetUserById(opdObj.ModifiedUser);

                    opdObj.issuedUser = user;
                    opdObj.invoiceID = item.Id;
                    opdObj.TotalAmount = 0;
                    opdObj.TotalRefund = 0;
                    opdObj.cashier = opdObj.nightShiftSession.User;
                    opdObj.TotalPaidAmount = 0;
                    opdObj.deviation = 0;
                    foreach (var invoiceItem in invoiceItemList)
                    {
                        opdObj.TotalRefund = opdObj.TotalRefund+ invoiceItem.Total;        
                    }

                    var paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
                    decimal tRefund = 0;
                    foreach (var payment in paymentList)
                    {
                        decimal paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount + payment.ChequeAmount + payment.GiftCardAmount;
                        if(payment.BillingType == BillingType.REFUND)
                        {
                            tRefund = tRefund + paidAmount;
                        }
                        opdObj.TotalPaidAmount = opdObj.TotalPaidAmount + paidAmount;

                    }
                    opdObj.TotalNeedToRefund = opdObj.TotalRefund + tRefund;
                    opdObj.TotalOldAmount = opdObj.TotalPaidAmount - tRefund;
                    newmtList.Add(opdObj);
                }

            }
            return newmtList;
        }


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
                    .Include(c => c.nightShiftSession.User)
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
                    Model.User user = new Model.User();
                    user = GetUserById(opdObj.ModifiedUser);

                    opdObj.issuedUser = user;
                    opdObj.invoiceID = item.Id;
                    opdObj.TotalAmount = 0;
                    opdObj.cashier = opdObj.nightShiftSession.User;
                    opdObj.TotalPaidAmount = 0;
                    opdObj.deviation = 0;
                    foreach (var invoiceItem in invoiceItemList)
                    {
                        opdObj.TotalAmount = opdObj.TotalAmount + invoiceItem.Total;

                    }

                    var paymentList = dbContext.Payments.Where(o => o.InvoiceID == item.Id).ToList();
                    foreach (var payment in paymentList)
                    {
                        decimal paidAmount = payment.CashAmount + payment.DdebitAmount + payment.CreditAmount + payment.ChequeAmount + payment.GiftCardAmount;
                        opdObj.TotalPaidAmount = opdObj.TotalPaidAmount + paidAmount;

                    }
                    opdObj.deviation = opdObj.TotalPaidAmount - opdObj.TotalAmount;
                    newmtList.Add(opdObj);
                }

            }
            return newmtList;
        }


        public List<Model.OPD> GetAllOPDAndChannellingByAndDateRangeNotPaid(DateTime startDate, DateTime endDate,InvoiceType invoiceType)
        {
            List<Model.OPD> mtList = new List<Model.OPD>();
            List<Model.OPD> newmtList = new List<Model.OPD>();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.OPD
                    .Include(c => c.patient)
                    .Include(c => c.consultant)
                    .Include(c => c.room)
                    .Include(c => c.nightShiftSession.User)
                    .Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NOT_PAID && o.invoiceType == invoiceType)
                    .OrderByDescending(o => o.Id)
                    .ToList<Model.OPD>();

                var opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.DateTime >= startDate && o.DateTime <= endDate && o.paymentStatus == PaymentStatus.NOT_PAID && o.invoiceType == invoiceType).Select(r => r.Id).ToList();
                var drugsList = dbContext.OPDDrugus.Where(o => opdIds.Contains(o.opdId)).ToList();

                foreach (var item in mtList)
                {
                    Model.OPD opdObj = new Model.OPD();
                    opdObj = item;          
                    Model.User user = new Model.User();
                    user = GetUserById(opdObj.ModifiedUser);

                    opdObj.issuedUser = user;

                    opdObj.TotalAmount = 0;
                    opdObj.TotalAmount = opdObj.TotalAmount + opdObj.HospitalFee;
                    opdObj.TotalAmount = opdObj.TotalAmount + opdObj.ConsultantFee;
                    foreach (var drug in drugsList)
                    {
                        decimal price = drug.Price * drug.Qty;
                        opdObj.TotalAmount = opdObj.TotalAmount + price;

                    }
                    newmtList.Add(opdObj);
                }

            }
            return newmtList;
        }
        #endregion


        private Model.User GetUserById(int id)
        {
            Model.User user = new Model.User();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    user = new UserService().GetUserByID(id);

                }
                catch (Exception ex) { }
            }
            return user;
        }

    }
}
