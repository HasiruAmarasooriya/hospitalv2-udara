using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.CashierSession
{
    public class CashierSessionService
    {
        public Model.CashierSession GetCashierSessionPaymentData(int sessionId)
        {
            // Handle not implemented exception
            Model.CashierSession cashierSessionAmounts = new Model.CashierSession();

            Model.CashierSession OPDPayementData = OPDTotalAmount(sessionId);
            Model.CashierSession XRAYPayementData = XRAYTotalAmount(sessionId);
            Model.CashierSession OTHERPayementData = OtherTotalAmount(sessionId);
            Model.CashierSession ChannelingPayementData = ChannelingTotalAmount(sessionId);

            cashierSessionAmounts.OPDTotalAmount = OPDPayementData.OPDTotalAmount;
            cashierSessionAmounts.OPDTotalPaidAmount = OPDPayementData.OPDTotalPaidAmount;
            cashierSessionAmounts.OPDTotalRefund = OPDPayementData.OPDTotalRefund;

            cashierSessionAmounts.XRAYTotalAmount = XRAYPayementData.XRAYTotalAmount;
            cashierSessionAmounts.XRAYTotalPaidAmount = XRAYPayementData.XRAYTotalPaidAmount;
            cashierSessionAmounts.XRAYTotalRefund = XRAYPayementData.XRAYTotalRefund;

            cashierSessionAmounts.OtherTotalAmount = OTHERPayementData.OtherTotalAmount;
            cashierSessionAmounts.OtherTotalPaidAmount = OTHERPayementData.OtherTotalPaidAmount;
            cashierSessionAmounts.OtherTotalRefund = OTHERPayementData.OtherTotalRefund;

            cashierSessionAmounts.ChannelingTotalAmount = ChannelingPayementData.ChannelingTotalAmount;
            cashierSessionAmounts.ChannelingTotalPaidAmount = ChannelingPayementData.ChannelingTotalPaidAmount;
            cashierSessionAmounts.ChannelingTotalRefund = ChannelingPayementData.ChannelingTotalRefund;

            return cashierSessionAmounts;

        }

        private Model.CashierSession ChannelingTotalAmount(int sessionId)
        {
            Model.CashierSession cashierSessionAmounts = new Model.CashierSession();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                /*List<int> opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.invoiceType == InvoiceType.CHE).Select(r => r.Id).ToList();
                List<int> invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                decimal totalCashierAmount = dbContext.Payments
                    .Where(o => invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                return totalCashierAmount;*/

                List<int> opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
                List<int> invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                List<int> opdIdsNeedToPay = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.NEED_TO_PAY).Select(r => r.Id).ToList();
                List<int> invoiceListNeedToPay = dbContext.Invoices.Where(o => opdIdsNeedToPay.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                List<int> opdIdsNotPaid = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.invoiceType == InvoiceType.CHE && o.paymentStatus == PaymentStatus.NOT_PAID).Select(r => r.Id).ToList();
                List<int> invoiceListNotPaid = dbContext.Invoices.Where(o => opdIdsNotPaid.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                decimal totalCashierAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.CASHIER && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalBalanceAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.BALENCE && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalRefundAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.REFUND && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalRefundAmountInNeedToPay = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.REFUND && invoiceListNeedToPay.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                List<int> invoiceListNotPaidHasSessionId = dbContext.Payments
                    .Where(o => invoiceListNotPaid.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Select(o => o.InvoiceID).ToList();

                List<int> invoiceListNeedToPayHasSessionId = dbContext.Payments
                    .Where(o => invoiceListNeedToPay.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Select(o => o.InvoiceID).ToList();

                decimal removedItemSumNeedToPay = dbContext.InvoiceItems.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNeedToPayHasSessionId.Contains(o.InvoiceId)).Sum(o => o.price * o.qty);
                decimal removedItemSumRefund = dbContext.InvoiceItems.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNotPaidHasSessionId.Contains(o.InvoiceId)).Sum(o => o.price * o.qty);

                decimal totalRefund = removedItemSumNeedToPay + removedItemSumRefund;
                decimal totalAmount = totalCashierAmount + totalBalanceAmount + removedItemSumNeedToPay + removedItemSumRefund;
                decimal totalPaidAmount = totalAmount + totalRefundAmount;

                decimal oldPaidAmount = totalPaidAmount - (totalRefundAmount + totalRefundAmountInNeedToPay);
                decimal actRefundAmount = oldPaidAmount - totalPaidAmount;
                decimal needToPayTotal = totalRefund - actRefundAmount;

                cashierSessionAmounts.ChannelingTotalAmount = oldPaidAmount;
                cashierSessionAmounts.ChannelingTotalRefund = actRefundAmount;
                cashierSessionAmounts.ChannelingTotalPaidAmount = totalPaidAmount;

                return cashierSessionAmounts;
            }
        }

        private Model.CashierSession OPDTotalAmount(int sessionId)
        {
            Model.CashierSession cashierSessionAmounts = new Model.CashierSession();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                /*List<int> opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "OPD").Select(r => r.Id).ToList();
                List<int> invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                decimal totalCashierAmount = dbContext.Payments
                    .Where(o => invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                return totalCashierAmount;*/

                List<int> opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "OPD" && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
                List<int> invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                List<int> opdIdsNeedToPay = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "OPD" && o.paymentStatus == PaymentStatus.NEED_TO_PAY).Select(r => r.Id).ToList();
                List<int> invoiceListNeedToPay = dbContext.Invoices.Where(o => opdIdsNeedToPay.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                List<int> opdIdsNotPaid = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "OPD" && o.paymentStatus == PaymentStatus.NOT_PAID).Select(r => r.Id).ToList();
                List<int> invoiceListNotPaid = dbContext.Invoices.Where(o => opdIdsNotPaid.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                decimal totalCashierAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.CASHIER && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalBalanceAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.BALENCE && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalRefundAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.REFUND && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalRefundAmountInNeedToPay = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.REFUND && invoiceListNeedToPay.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                List<int> invoiceListNotPaidHasSessionId = dbContext.Payments
                    .Where(o => invoiceListNotPaid.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Select(o => o.InvoiceID).ToList();

                List<int> invoiceListNeedToPayHasSessionId = dbContext.Payments
                    .Where(o => invoiceListNeedToPay.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Select(o => o.InvoiceID).ToList();

                decimal removedItemSumNeedToPay = dbContext.InvoiceItems.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNeedToPayHasSessionId.Contains(o.InvoiceId)).Sum(o => o.price * o.qty);
                decimal removedItemSumRefund = dbContext.InvoiceItems.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNotPaidHasSessionId.Contains(o.InvoiceId)).Sum(o => o.price * o.qty);

                decimal totalRefund = removedItemSumNeedToPay + removedItemSumRefund;
                decimal totalAmount = totalCashierAmount + totalBalanceAmount + removedItemSumNeedToPay + removedItemSumRefund;
                decimal totalPaidAmount = totalAmount + totalRefundAmount;

                decimal oldPaidAmount = totalPaidAmount - (totalRefundAmount + totalRefundAmountInNeedToPay);
                decimal actRefundAmount = oldPaidAmount - totalPaidAmount;
                decimal needToPayTotal = totalRefund - actRefundAmount;

                cashierSessionAmounts.OPDTotalAmount = oldPaidAmount;
                cashierSessionAmounts.OPDTotalRefund = actRefundAmount;
                cashierSessionAmounts.OPDTotalPaidAmount = totalPaidAmount;

                return cashierSessionAmounts;
            }
        }

        private Model.CashierSession XRAYTotalAmount(int sessionId)
        {
            Model.CashierSession cashierSessionAmounts = new Model.CashierSession();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                /*List<int> opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "X-RAY").Select(r => r.Id).ToList();
                List<int> invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                decimal totalCashierAmount = dbContext.Payments
                    .Where(o => invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                return totalCashierAmount;*/

                List<int> opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "X-RAY" && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
                List<int> invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                List<int> opdIdsNeedToPay = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "X-RAY" && o.paymentStatus == PaymentStatus.NEED_TO_PAY).Select(r => r.Id).ToList();
                List<int> invoiceListNeedToPay = dbContext.Invoices.Where(o => opdIdsNeedToPay.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                List<int> opdIdsNotPaid = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "X-RAY" && o.paymentStatus == PaymentStatus.NOT_PAID).Select(r => r.Id).ToList();
                List<int> invoiceListNotPaid = dbContext.Invoices.Where(o => opdIdsNotPaid.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                decimal totalCashierAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.CASHIER && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalBalanceAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.BALENCE && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalRefundAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.REFUND && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalRefundAmountInNeedToPay = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.REFUND && invoiceListNeedToPay.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                List<int> invoiceListNotPaidHasSessionId = dbContext.Payments
                    .Where(o => invoiceListNotPaid.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Select(o => o.InvoiceID).ToList();

                List<int> invoiceListNeedToPayHasSessionId = dbContext.Payments
                    .Where(o => invoiceListNeedToPay.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Select(o => o.InvoiceID).ToList();

                decimal removedItemSumNeedToPay = dbContext.InvoiceItems.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNeedToPayHasSessionId.Contains(o.InvoiceId)).Sum(o => o.price * o.qty);
                decimal removedItemSumRefund = dbContext.InvoiceItems.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNotPaidHasSessionId.Contains(o.InvoiceId)).Sum(o => o.price * o.qty);

                decimal totalRefund = removedItemSumNeedToPay + removedItemSumRefund;
                decimal totalAmount = totalCashierAmount + totalBalanceAmount + removedItemSumNeedToPay + removedItemSumRefund;
                decimal totalPaidAmount = totalAmount + totalRefundAmount;

                decimal oldPaidAmount = totalPaidAmount - (totalRefundAmount + totalRefundAmountInNeedToPay);
                decimal actRefundAmount = oldPaidAmount - totalPaidAmount;
                decimal needToPayTotal = totalRefund - actRefundAmount;

                cashierSessionAmounts.XRAYTotalAmount = oldPaidAmount;
                cashierSessionAmounts.XRAYTotalRefund = actRefundAmount;
                cashierSessionAmounts.XRAYTotalPaidAmount = totalPaidAmount;

                return cashierSessionAmounts;
            }
        }

        private Model.CashierSession OtherTotalAmount(int sessionId)
        {
            Model.CashierSession cashierSessionAmounts = new Model.CashierSession();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                /*List<int> opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "Other").Select(r => r.Id).ToList();
                List<int> invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                decimal totalCashierAmount = dbContext.Payments
                    .Where(o => invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                return totalCashierAmount;*/

                List<int> opdIds = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "Other" && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();
                List<int> invoiceList = dbContext.Invoices.Where(o => opdIds.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                List<int> opdIdsNeedToPay = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "Other" && o.paymentStatus == PaymentStatus.NEED_TO_PAY).Select(r => r.Id).ToList();
                List<int> invoiceListNeedToPay = dbContext.Invoices.Where(o => opdIdsNeedToPay.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                List<int> opdIdsNotPaid = dbContext.OPD.Where(o => o.Status == CommonStatus.Active && o.Description == "Other" && o.paymentStatus == PaymentStatus.NOT_PAID).Select(r => r.Id).ToList();
                List<int> invoiceListNotPaid = dbContext.Invoices.Where(o => opdIdsNotPaid.Contains(o.ServiceID)).Select(r => r.Id).ToList();

                decimal totalCashierAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.CASHIER && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalBalanceAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.BALENCE && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalRefundAmount = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.REFUND && invoiceList.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal totalRefundAmountInNeedToPay = dbContext.Payments
                    .Where(o => o.BillingType == BillingType.REFUND && invoiceListNeedToPay.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                List<int> invoiceListNotPaidHasSessionId = dbContext.Payments
                    .Where(o => invoiceListNotPaid.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Select(o => o.InvoiceID).ToList();

                List<int> invoiceListNeedToPayHasSessionId = dbContext.Payments
                    .Where(o => invoiceListNeedToPay.Contains(o.InvoiceID) && o.sessionID == sessionId)
                    .Select(o => o.InvoiceID).ToList();

                decimal removedItemSumNeedToPay = dbContext.InvoiceItems.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNeedToPayHasSessionId.Contains(o.InvoiceId)).Sum(o => o.price * o.qty);
                decimal removedItemSumRefund = dbContext.InvoiceItems.Where(o => o.itemInvoiceStatus == ItemInvoiceStatus.Remove && invoiceListNotPaidHasSessionId.Contains(o.InvoiceId)).Sum(o => o.price * o.qty);

                decimal totalRefund = removedItemSumNeedToPay + removedItemSumRefund;
                decimal totalAmount = totalCashierAmount + totalBalanceAmount + removedItemSumNeedToPay + removedItemSumRefund;
                decimal totalPaidAmount = totalAmount + totalRefundAmount;

                decimal oldPaidAmount = totalPaidAmount - (totalRefundAmount + totalRefundAmountInNeedToPay);
                decimal actRefundAmount = oldPaidAmount - totalPaidAmount;
                decimal needToPayTotal = totalRefund - actRefundAmount;

                cashierSessionAmounts.OtherTotalAmount = oldPaidAmount;
                cashierSessionAmounts.OtherTotalRefund = actRefundAmount;
                cashierSessionAmounts.OtherTotalPaidAmount = totalPaidAmount;

                return cashierSessionAmounts;
            }
        }

        public Model.CashierSession CreateCashierSession(Model.CashierSession cashierSession)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                if (cashierSession.Id == 0)
                {
                    dbContext.CashierSessions.Add(cashierSession);
                    dbContext.SaveChanges();
                }
                else
                {
                    HospitalMgrSystem.Model.CashierSession result = (from p in dbContext.CashierSessions where p.Id == cashierSession.Id select p).SingleOrDefault();
                    result.EndBalence = cashierSession.EndBalence;
                    result.EndTime = cashierSession.EndTime;
                    result.col1 = cashierSession.col1;
                    result.col2 = cashierSession.col2;
                    result.col3 = cashierSession.col3;
                    result.col4 = cashierSession.col4;
                    result.col5 = cashierSession.col5;
                    result.col6 = cashierSession.col6;
                    result.col7 = cashierSession.col7;
                    result.col8 = cashierSession.col8;
                    result.col9 = cashierSession.col9;
                    result.col10 = cashierSession.col10;

                    result.cashierSessionStatus = cashierSession.cashierSessionStatus;
                    result.ModifiedUser = cashierSession.ModifiedUser;
                    result.ModifiedDate = cashierSession.ModifiedDate;



                    dbContext.SaveChanges();
                }
                return cashierSession;
            }
        }

        public List<Model.CashierSession> GetAllCashierSession()
        {
            List<Model.CashierSession> mtList = new List<Model.CashierSession>();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                mtList = dbContext.CashierSessions.Include(c => c.User).Where(o => o.Status == 0).OrderByDescending(o => o.Id).ToList<Model.CashierSession>();

                // Get sum of all payments as Total
                var paymentsData = dbContext.Payments
                    .GroupBy(c => c.sessionID)
                    .Select(g => new
                    {
                        sessionID = g.Key,
                        Total = g.Sum(c => c.CashAmount)
                    }).ToList();

                foreach (var item in mtList)
                {
                    foreach (var payment in paymentsData)
                    {
                        if (item.Id == payment.sessionID)
                        {
                            item.TotalAmount = payment.Total;
                        }
                    }
                }
            }

            return mtList;
        }

        public Model.CashierSession GetCashierSessionByID(int id)
        {


            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.CashierSession result = dbContext.CashierSessions.Include(c => c.User).Where(o => o.Status == 0 && o.Id == id).SingleOrDefault();
                return result;
            }
        }


        public List<Model.CashierSession> GetACtiveCashierSessions(int id)
        {
            List<Model.CashierSession> mtList = new List<Model.CashierSession>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.CashierSessions.Include(c => c.User).Where(o => o.Status == 0 && o.cashierSessionStatus == Model.Enums.CashierSessionStatus.START && o.userID == id).OrderByDescending(o => o.Id).ToList<Model.CashierSession>();

            }
            return mtList;
        }

        public List<Model.CashierSession> GetAllNightsiftActiveCashierSession()
        {
            List<Model.CashierSession> mtList = new List<Model.CashierSession>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.CashierSessions.Include(c => c.User).Where(o => o.Status == 0 && o.cashierSessionStatus == Model.Enums.CashierSessionStatus.START && o.UserRole == UserRole.OPDNURSE).OrderByDescending(o => o.Id).ToList<Model.CashierSession>();

            }
            return mtList;
        }

    }
}
