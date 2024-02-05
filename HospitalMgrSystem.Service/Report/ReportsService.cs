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
    public class ReportsService
    {

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
