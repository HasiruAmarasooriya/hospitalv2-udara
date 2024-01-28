using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.Cashier;
using HospitalMgrSystem.Service.CashierSession;
using HospitalMgrSystem.Service.OtherTransactions;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HospitalMgrSystemUI.Controllers
{
    public class OtherIncomeController : Controller
    {


        [BindProperty]
        public OtherTransactionsDto viewOtherTransactionsDto { get; set; }
        public IActionResult Index()
        {
            OtherTransactionsDto otherTransactionsDto = new OtherTransactionsDto();
            otherTransactionsDto.otherTransactionsList = GetAllOtherTransactions();
            return View(otherTransactionsDto);
        }


        public ActionResult CreateOtherIncome(int id)
        {
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
            int userID = Convert.ToInt32(userIdCookie);

            OtherTransactionsDto otherTransactionsDto = new OtherTransactionsDto();

            List<CashierSession> CashierSessionList = new List<CashierSession>();
            CashierSessionList = GetActiveCashierSession(userID);
            if (CashierSessionList != null)
            {
                if (CashierSessionList.Count > 0)
                {
                    CashierSession cashierSessiont = new CashierSession();
                    cashierSessiont = CashierSessionList[0];
                    otherTransactionsDto.sessionDetails = cashierSessiont.CreateDate + " -" + cashierSessiont.User.FullName;
                    otherTransactionsDto.username = cashierSessiont.User.FullName;

                }
            }

            if (id > 0)
            {
                return PartialView("_PartialAddOtherIncome", otherTransactionsDto);
            }
            else
                return PartialView("_PartialAddOtherIncome", otherTransactionsDto);



        }


        public IActionResult CreateNewOtherIncome()
        {
            OtherTransactions otherTransactions = new OtherTransactions();
            using (var httpClient = new HttpClient())
            {

                try
                {
                    var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
                    if (viewOtherTransactionsDto != null)
                    {
                        if (viewOtherTransactionsDto.otherTransactions.Id == 0)
                        {

                            List<CashierSession> mtList = new List<CashierSession>();
                            mtList = GetActiveCashierSession(Convert.ToInt32(userIdCookie));
                            if (mtList.Count > 0)
                            {
                                viewOtherTransactionsDto.otherTransactions.SessionID = mtList[0].Id; 
                                viewOtherTransactionsDto.otherTransactions.ConvenerID = Convert.ToInt32(userIdCookie);

                                viewOtherTransactionsDto.otherTransactions.ApprovedByID = Convert.ToInt32(userIdCookie);

                                viewOtherTransactionsDto.otherTransactions.otherTransactionsStatus = OtherTransactionsStatus.Requested;

                                viewOtherTransactionsDto.otherTransactions.CreateUser = Convert.ToInt32(userIdCookie);
                                viewOtherTransactionsDto.otherTransactions.CreateDate = DateTime.Now;
                                viewOtherTransactionsDto.otherTransactions.ModifiedUser = Convert.ToInt32(userIdCookie);
                                viewOtherTransactionsDto.otherTransactions.ModifiedDate = DateTime.Now;

                                otherTransactions = new OtherTransactionsService().CreateOtherTransactions(viewOtherTransactionsDto.otherTransactions);

                            }
                        }


                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
               

            }
        }



        public IActionResult CompleteOtherIncome(int id)
        {
            OtherTransactions otherTransactions = new OtherTransactions();
            using (var httpClient = new HttpClient())
            {

                try
                {
                    var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];

                        if (id > 0)
                        {

                            List<CashierSession> mtList = new List<CashierSession>();
                            mtList = GetActiveCashierSession(Convert.ToInt32(userIdCookie));
                            if (mtList.Count > 0)
                            {
                                otherTransactions = GetOtherTransactionByID(id);

                                if (otherTransactions.otherTransactionsStatus == OtherTransactionsStatus.Approved)
                                {
                                    Invoice invoice = new Invoice();
                                    invoice.CustomerID = otherTransactions.ConvenerID;
                                    invoice.CustomerName = otherTransactions.Convener.FullName;
                                    invoice.InvoiceType = otherTransactions.InvoiceType;
                                    invoice.paymentStatus = PaymentStatus.NOT_PAID;
                                    invoice.Status = InvoiceStatus.New;
                                    invoice.CreateUser = Convert.ToInt32(userIdCookie);
                                    invoice.ModifiedUser = Convert.ToInt32(userIdCookie);
                                    invoice.CreateDate = DateTime.Now;
                                    invoice.ModifiedDate = DateTime.Now;
                                    invoice.ServiceID = otherTransactions.Id;
                                    Invoice resInvoice = new CashierService().AddInvoice(invoice);


                                    List<InvoiceItem> invoiceItems = new List<InvoiceItem>();

                                    InvoiceItem otherIncome = new InvoiceItem();
                                    otherIncome.itemInvoiceStatus = ItemInvoiceStatus.BILLED;
                                    otherIncome.billingItemsType = BillingItemsType.OTHER;
                                    otherIncome.ItemID = 1;
                                    otherIncome.Discount = 0;
                                    otherIncome.price = otherTransactions.Amount;
                                    otherIncome.qty = 1;
                                    otherIncome.Total = otherTransactions.Amount;
                                    otherIncome.InvoiceId = resInvoice.Id;
                                    invoiceItems.Add(otherIncome);
                                    new CashierService().AddInvoiceItems(invoiceItems, Convert.ToInt32(userIdCookie));


                                    Payment payments = new Payment();
                                    payments.InvoiceID = resInvoice.Id;                                 
                                    payments.CreditAmount = 0;
                                    payments.DdebitAmount = 0;
                                    payments.ChequeAmount = 0;
                                    payments.GiftCardAmount =0;
                                    payments.CreateUser = Convert.ToInt32(userIdCookie);
                                    payments.ModifiedUser = Convert.ToInt32(userIdCookie);
                                    payments.CreateDate = DateTime.Now;
                                    payments.ModifiedDate = DateTime.Now;
                                    payments.sessionID = mtList[0].Id;

                                 
                                    if (otherTransactions.InvoiceType == InvoiceType.OTHER_INCOME)
                                    {
                                        payments.CashierStatus = CashierStatus.CashierIn;
                                        payments.BillingType = BillingType.OTHER_IN;
                                        payments.CashAmount = otherTransactions.Amount;
                                    }
                                    if (otherTransactions.InvoiceType == InvoiceType.OTHER_EXPENSES)
                                    {
                                        payments.CashierStatus = CashierStatus.CashierOut;
                                        payments.BillingType = BillingType.OTHER_OUT;
                                        payments.CashAmount = otherTransactions.Amount * -1;
                                    }
                                    if (otherTransactions.InvoiceType == InvoiceType.CASHIER_TRANSFER_IN)
                                    {
                                        payments.CashierStatus = CashierStatus.CashierIn;
                                        payments.BillingType = BillingType.OTHER_IN;
                                        payments.CashAmount = otherTransactions.Amount;
                                    }
                                    if (otherTransactions.InvoiceType == InvoiceType.CASHIER_TRANSFER_OUT)
                                    {
                                        payments.CashierStatus = CashierStatus.CashierOut;
                                        payments.BillingType = BillingType.OTHER_OUT;
                                        payments.CashAmount = otherTransactions.Amount * -1;
                                    }

                                    Payment resPayment = new CashierService().AddPayments(payments);

                                    otherTransactions.otherTransactionsStatus = OtherTransactionsStatus.Completed;
                                    otherTransactions.ModifiedUser = Convert.ToInt32(userIdCookie);
                                    otherTransactions.ModifiedDate = DateTime.Now;

                                    otherTransactions = new OtherTransactionsService().CompleteeOtherTransactions(otherTransactions);

                                }


                            }
                        }


                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }


            }
        }



        public IActionResult ApproveOtherIncome(int id)
        {
            OtherTransactions otherTransactions = new OtherTransactions();
            using (var httpClient = new HttpClient())
            {

                try
                {
                    var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];

                        if (id > 0)
                        {

                        otherTransactions = GetOtherTransactionByID(id);

                        if (otherTransactions.otherTransactionsStatus == OtherTransactionsStatus.Requested)
                        {
                            otherTransactions.ApprovedByID = Convert.ToInt32(userIdCookie);
                            otherTransactions.otherTransactionsStatus = OtherTransactionsStatus.Approved;
                            otherTransactions.ModifiedUser = Convert.ToInt32(userIdCookie);
                            otherTransactions.ModifiedDate = DateTime.Now;

                            otherTransactions = new OtherTransactionsService().AproveOtherTransactions(otherTransactions);

                        }

                        }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }


            }
        }


        private OtherTransactions GetOtherTransactionByID(int id)
        {
            OtherTransactions otherTransactions = new OtherTransactions();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    otherTransactions = new OtherTransactionsService().GetOtherTransactionsByID(id);

                }
                catch (Exception ex) { }
            }
            return otherTransactions;
        }

        private List<CashierSession> GetActiveCashierSession(int id)
        {
            List<CashierSession> CashierSessionList = new List<CashierSession>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    CashierSessionList = new CashierSessionService().GetACtiveCashierSessions(id);

                }
                catch (Exception ex) { }
            }
            return CashierSessionList;
        }

        private List<OtherTransactions> GetAllOtherTransactions()
        {
            List<OtherTransactions> otherTransactionsList = new List<OtherTransactions>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    otherTransactionsList = new OtherTransactionsService().GetAllOtherTransactions();

                }
                catch (Exception ex) { }
            }
            return otherTransactionsList;
        }
    }
}
