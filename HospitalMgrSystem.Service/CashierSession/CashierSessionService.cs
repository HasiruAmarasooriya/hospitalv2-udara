using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.DTO;
using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Service.CashierSession
{
    public class CashierSessionService
    {
	    public List<ForwardBookingDataTableDTO>? GetForwardBookingDataByCashierSessionId(int sessionId)
	    {
		    using var context = new DataAccess.HospitalDBContext();

		    var forwardBookingData = context.Set<ForwardBookingDataTableDTO>()
			    .FromSqlRaw("EXEC GetAllForwardBookingDataByCashierSessionId @sessionId = {0}", sessionId)
			    .ToList();

		    return forwardBookingData.Count == 0 ? null : forwardBookingData;
	    }

	    public TotalPaidAmountOfForwardBookingDTO? GetTotalAmountOfForwardBookingByCashierSessionId(int sessionId)
	    {
		    using var context = new DataAccess.HospitalDBContext();

		    List<TotalPaidAmountOfForwardBookingDTO?> forwardBookingPayment = context.Set<TotalPaidAmountOfForwardBookingDTO>()
			    .FromSqlRaw("EXEC GetAllForwardBookingDataWhenDoctorSessionEndsBySessionId @sessionId = {0}", sessionId)
			    .ToList()!;

		    return forwardBookingPayment[0]!.TotalPaidAmount == 0 ? null : forwardBookingPayment[0];
		}

		public Model.CashierSession GetCashierSessionPaymentData(int sessionId)
        {
            // Handle not implemented exception
            Model.CashierSession cashierSessionAmounts = new Model.CashierSession();
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                List<int> InvoiceIds = dbContext.Payments.Where(o => o.sessionID == sessionId)
                                       .Select(r => r.InvoiceID).Distinct().ToList();

                Model.CashierSession cashierSession = GetCashierSessionByID(sessionId);

                Model.CashierSession OPDPayementData = PaymentDetailsOPD(sessionId, InvoiceIds);
                Model.CashierSession XRAYPayementData = PaymentDetailsXRAY(sessionId, InvoiceIds);
                Model.CashierSession OTHERPayementData = PaymentDetailsOtherMS(sessionId,InvoiceIds);
                Model.CashierSession ChannelingPayementData = PaymentDetailsCHE(sessionId, InvoiceIds);
                //Model.CashierSession otherHospitalIncome = PaymentDetailsOtherTransactrion(sessionId, InvoiceIds);
                Model.CashierSession otherHospitalIncome = PaymentDetailsOtherIcome(sessionId, InvoiceIds);
                Model.CashierSession cashierTransferInAndOut = CashierTransferDetails(sessionId, InvoiceIds);

                cashierSessionAmounts.OPDTotalAmount = OPDPayementData.OPDTotalAmount;
                cashierSessionAmounts.OPDTotalPaidAmount = OPDPayementData.OPDTotalPaidAmount;
                cashierSessionAmounts.OPDTotalRefund = OPDPayementData.OPDTotalRefund;
                cashierSessionAmounts.OPDCashBalence = OPDPayementData.OPDCashBalence;
                cashierSessionAmounts.OPDTotalPaidCardAmount = OPDPayementData.OPDTotalPaidCardAmount;

                cashierSessionAmounts.XRAYTotalAmount = XRAYPayementData.XRAYTotalAmount;
                cashierSessionAmounts.XRAYTotalPaidAmount = XRAYPayementData.XRAYTotalPaidAmount;
                cashierSessionAmounts.XRAYTotalRefund = XRAYPayementData.XRAYTotalRefund;
                cashierSessionAmounts.XRAYCashBalence = XRAYPayementData.XRAYCashBalence;
                cashierSessionAmounts.XRAYTotalPaidCardAmount = XRAYPayementData.XRAYTotalPaidCardAmount;

                cashierSessionAmounts.OtherTotalAmount = OTHERPayementData.OtherTotalAmount;
                cashierSessionAmounts.OtherTotalPaidAmount = OTHERPayementData.OtherTotalPaidAmount;
                cashierSessionAmounts.OtherTotalRefund = OTHERPayementData.OtherTotalRefund;
                cashierSessionAmounts.OtherCashBalence = OTHERPayementData.OtherCashBalence;
                cashierSessionAmounts.OtherTotalPaidCardAmount = OTHERPayementData.OtherTotalPaidCardAmount;

                cashierSessionAmounts.ChannelingTotalAmount = ChannelingPayementData.ChannelingTotalAmount;
                cashierSessionAmounts.ChannelingTotalPaidAmount = ChannelingPayementData.ChannelingTotalPaidAmount;
                cashierSessionAmounts.ChannelingTotalRefund = ChannelingPayementData.ChannelingTotalRefund;
                cashierSessionAmounts.ChannelingTotalDoctorPayment = ChannelingPayementData.ChannelingTotalDoctorPayment;
                cashierSessionAmounts.ChannelingTotalPaidCardAmount = ChannelingPayementData.ChannelingTotalPaidCardAmount;
                cashierSessionAmounts.ChannelingCashBalence = ChannelingPayementData.ChannelingCashBalence;

                cashierSessionAmounts.AllServiceTotalPaidAmount = cashierSessionAmounts.OPDTotalPaidAmount + cashierSessionAmounts.XRAYTotalPaidAmount + cashierSessionAmounts.OtherTotalPaidAmount + cashierSessionAmounts.ChannelingTotalPaidAmount;
                cashierSessionAmounts.AllServiceTotalPaidCardAmount = cashierSessionAmounts.OPDTotalPaidCardAmount + cashierSessionAmounts.XRAYTotalPaidCardAmount + cashierSessionAmounts.OtherTotalPaidCardAmount + cashierSessionAmounts.ChannelingTotalPaidCardAmount;
                cashierSessionAmounts.AllServiceTotalRefund = cashierSessionAmounts.OPDTotalRefund + cashierSessionAmounts.XRAYTotalRefund + cashierSessionAmounts.OtherTotalRefund + cashierSessionAmounts.ChannelingTotalRefund;
                cashierSessionAmounts.AllServiceTotalAmount = cashierSessionAmounts.OPDTotalAmount + cashierSessionAmounts.XRAYTotalAmount + cashierSessionAmounts.OtherTotalAmount + cashierSessionAmounts.ChannelingTotalAmount+ cashierTransferInAndOut.totalCashierTransferIn+ cashierTransferInAndOut.totalCashierTransferOut+ otherHospitalIncome.totalHospitaOtherIncome;
                cashierSessionAmounts.AllServiceCashBalence = cashierSession.EndBalence-cashierSession.StartBalence;


                //cashierSessionAmounts.notInSyestemConsultantList = otherHospitalIncome.notInSyestemConsultantList;
                cashierSessionAmounts.totalCashierTransferIn = cashierTransferInAndOut.totalCashierTransferIn;
                cashierSessionAmounts.totalCashierTransferOut = cashierTransferInAndOut.totalCashierTransferOut;
                cashierSessionAmounts.totalHospitaOtherIncome = otherHospitalIncome.totalHospitaOtherIncome;
                cashierSessionAmounts.otherIncomeList = otherHospitalIncome.otherIncomeList;
                cashierSessionAmounts.totalHospitaOtherIncome = otherHospitalIncome.totalHospitaOtherIncome;
                cashierSessionAmounts.totalGrandIncome = cashierSessionAmounts.OPDTotalAmount + cashierSessionAmounts.XRAYTotalAmount + cashierSessionAmounts.OtherTotalAmount + cashierSessionAmounts.ChannelingTotalAmount+ otherHospitalIncome.totalHospitaOtherIncome; ;
                return cashierSessionAmounts;
            }





        }

        private Model.CashierSession PaymentDetailsOtherTransactrion(int sessionId, List<int> InvoiceIds)
        {
            Model.CashierSession cashierSession = new Model.CashierSession();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                List<Model.Consultant> notInSyestemConsultantList = new List<Model.Consultant>();
                Model.Consultant notInSyestemConsultant = new Model.Consultant();
                List<int> DrPaymentsServiceIdsList = dbContext.Invoices.Where(o => InvoiceIds.Contains(o.Id) && o.InvoiceType == InvoiceType.DOCTOR_PAYMENT && o.Status == 0).Select(r => r.ServiceID).ToList();

                int ptCount = 0;

                //for doctor payments
                List<Model.Consultant> doctorOfNotInSystem = dbContext.Consultants.Include(c => c.Specialist).Where(o => o.isSystemDr == 0 && o.Status == 0).ToList();
                List<int> doctorIdsOfNotInSystem = doctorOfNotInSystem.Select(r => r.Id).ToList();

                List<Model.OtherTransactions> otherServiceList = dbContext.OtherTransactions.Where(o => doctorIdsOfNotInSystem.Contains(o.BeneficiaryID) && DrPaymentsServiceIdsList.Contains(o.Id) && o.Status == CommonStatus.Active && o.InvoiceType == InvoiceType.DOCTOR_PAYMENT).ToList();
                List<int> otherServiceIds = otherServiceList.Select(r => r.Id).ToList();

                List<Invoice> doctorPaymentInvoiceList = dbContext.Invoices.Where(o => otherServiceIds.Contains(o.ServiceID) && o.InvoiceType == InvoiceType.DOCTOR_PAYMENT && o.Status == 0).ToList();

                List<int> doctorPaymentServiceIdsList = doctorPaymentInvoiceList.Select(r => r.Id).ToList();

                decimal totalOtherHospitalIncome = 0;
                foreach (var itemDr in doctorOfNotInSystem)
                {
                    decimal totalOtherHospitalIncomeByDr = 0;
                    foreach (var itemDrIn in doctorPaymentInvoiceList)
                    {
                        int drId = otherServiceList.Where(o => o.BeneficiaryID == itemDr.Id && o.Id == itemDrIn.ServiceID && o.Status == CommonStatus.Active).Select(o => o.BeneficiaryID).FirstOrDefault();
                        if (drId == itemDr.Id)
                        {

                            totalOtherHospitalIncomeByDr = totalOtherHospitalIncomeByDr + dbContext.Payments
                                                           .Where(o => o.BillingType == BillingType.OTHER_IN && o.sessionID == sessionId && o.InvoiceID == itemDrIn.Id)
                                                           .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                        }

                    }
                    notInSyestemConsultant = itemDr;
                    notInSyestemConsultant.hospitalIncome = totalOtherHospitalIncomeByDr;
                    notInSyestemConsultantList.Add(notInSyestemConsultant);
                    totalOtherHospitalIncome = totalOtherHospitalIncome + totalOtherHospitalIncomeByDr;
                }
                cashierSession.totalHospitaOtherIncome = totalOtherHospitalIncome;
                cashierSession.notInSyestemConsultantList = notInSyestemConsultantList;



                //cashierSessionAmounts.ChannelingTotalPaidCardAmount = totalCashierCardAmount;
                return cashierSession;
            }
        }


        private Model.CashierSession PaymentDetailsOtherIcome(int sessionId, List<int> InvoiceIds)
        {
            Model.CashierSession cashierSession = new Model.CashierSession();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                List<Model.OtherTransactions> otherIncomeList = new List<Model.OtherTransactions>();



                List<Model.OtherTransactions> otherServiceList = dbContext.OtherTransactions.Where(o =>  o.Status == CommonStatus.Active && o.InvoiceType == InvoiceType.OTHER_INCOME && o.SessionID==sessionId).ToList();
                List<int> otherServiceIds = otherServiceList.Select(r => r.Id).ToList();

                List<Invoice> otherIncomeInvoiceList = dbContext.Invoices.Where(o => otherServiceIds.Contains(o.ServiceID)).ToList();

                List<int> otherIncomeInvoiceListIdsList = otherIncomeInvoiceList.Select(r => r.Id).ToList();

                decimal totalOtherHospitalIncome = 0;

                foreach (var itemOther in otherServiceList)
                {

                    decimal totalOtherHospitalIncomeByServiceID = 0;
                    foreach (var item in otherIncomeInvoiceList)
                    {
                        if(itemOther.Id== item.ServiceID)
                        {
                            totalOtherHospitalIncomeByServiceID = totalOtherHospitalIncomeByServiceID + dbContext.Payments
                               .Where(o => o.BillingType == BillingType.OTHER_IN && o.sessionID == sessionId && o.InvoiceID == item.Id)
                               .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);
                        }




                    }
                    totalOtherHospitalIncome = totalOtherHospitalIncome + totalOtherHospitalIncomeByServiceID;
                    itemOther.TotalOtherIncome= totalOtherHospitalIncomeByServiceID;
                    otherIncomeList.Add(itemOther);

                }



                cashierSession.totalHospitaOtherIncome = totalOtherHospitalIncome;
                cashierSession.otherIncomeList = otherIncomeList;        

                //cashierSessionAmounts.ChannelingTotalPaidCardAmount = totalCashierCardAmount;
                return cashierSession;
            }
        }


        private Model.CashierSession CashierTransferDetails(int sessionId, List<int> InvoiceIds)
        {
            Model.CashierSession cashierSession = new Model.CashierSession();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                Model.CashierSession otherTransferInAndOut = new Model.CashierSession();



                List<Model.OtherTransactions> otherServiceList = dbContext.OtherTransactions.Where(o => o.Status == CommonStatus.Active && o.InvoiceType == InvoiceType.CASHIER_TRANSFER_OUT && o.SessionID == sessionId).ToList();
                List<int> otherServiceIds = otherServiceList.Select(r => r.Id).ToList();

                List<Invoice> otherIncomeInvoiceList = dbContext.Invoices.Where(o => otherServiceIds.Contains(o.ServiceID)).ToList();

                List<int> otherIncomeInvoiceListIdsList = otherIncomeInvoiceList.Select(r => r.Id).ToList();



                List<Model.OtherTransactions> otherServiceListIn = dbContext.OtherTransactions.Where(o => o.Status == CommonStatus.Active && o.InvoiceType == InvoiceType.CASHIER_TRANSFER_IN && o.SessionID == sessionId).ToList();
                List<int> otherServiceIdsIn = otherServiceListIn.Select(r => r.Id).ToList();

                List<Invoice> otherIncomeInvoiceListIn = dbContext.Invoices.Where(o => otherServiceIdsIn.Contains(o.ServiceID)).ToList();

                List<int> otherIncomeInvoiceListIdsListIn = otherIncomeInvoiceListIn.Select(r => r.Id).ToList();


                decimal totalOtherTransferOut = 0;

                foreach (var itemOther in otherServiceList)
                {

                    decimal totalOtherTransferOutByServiceID = 0;
                    foreach (var item in otherIncomeInvoiceList)
                    {
                        if (itemOther.Id == item.ServiceID)
                        {
                            totalOtherTransferOutByServiceID = totalOtherTransferOutByServiceID + dbContext.Payments
                               .Where(o => o.sessionID == sessionId && o.InvoiceID == item.Id)
                               .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);
                        }




                    }
                    totalOtherTransferOut = totalOtherTransferOut + totalOtherTransferOutByServiceID;



                }




                decimal totalOtherTransferIn= 0;

                foreach (var itemOther in otherServiceListIn)
                {

                    decimal totalOtherTransferInByServiceIDIn = 0;
                    foreach (var item in otherIncomeInvoiceListIn)
                    {
                        if (itemOther.Id == item.ServiceID)
                        {
                            totalOtherTransferInByServiceIDIn = totalOtherTransferInByServiceIDIn + dbContext.Payments
                               .Where(o => o.sessionID == sessionId && o.InvoiceID == item.Id)
                               .Sum(o => o.CashAmount + o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);
                        }




                    }
                    totalOtherTransferIn = totalOtherTransferIn + totalOtherTransferInByServiceIDIn;

                 
                }



                cashierSession.totalCashierTransferOut = totalOtherTransferOut;
                cashierSession.totalCashierTransferIn = totalOtherTransferIn;

                //cashierSessionAmounts.ChannelingTotalPaidCardAmount = totalCashierCardAmount;
                return cashierSession;
            }
        }

        private Model.CashierSession PaymentDetailsCHE(int sessionId, List<int> InvoiceIds)
        {
            Model.CashierSession cashierSessionAmounts = new Model.CashierSession();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {

                List<int> CHEinvoiceList = dbContext.Invoices.Where(o => InvoiceIds.Contains(o.Id) && o.InvoiceType == InvoiceType.CHE && o.Status == 0).Select(r => r.Id).ToList();

                List<int> DrPaymentsServiceIdsList = dbContext.Invoices.Where(o => InvoiceIds.Contains(o.Id) && o.InvoiceType == InvoiceType.DOCTOR_PAYMENT && o.Status == 0).Select(r => r.ServiceID).ToList();



                //for doctor payments
                //List<int> doctorIdsOfNotInSystem = dbContext.Consultants.Where(o => o.isSystemDr == 0 && o.Status == 0).Select(r => r.Id).ToList();

                List<int> otherServiceIds = dbContext.OtherTransactions.Where(o =>  DrPaymentsServiceIdsList.Contains(o.Id) && o.Status == CommonStatus.Active && o.InvoiceType == InvoiceType.DOCTOR_PAYMENT).Select(r => r.Id).ToList();

                List<int> doctorPaymentServiceIdsList = dbContext.Invoices.Where(o => otherServiceIds.Contains(o.ServiceID) && o.InvoiceType == InvoiceType.DOCTOR_PAYMENT && o.Status == 0).Select(r => r.Id).ToList();



                decimal totalDoctorPaymentAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.OTHER_OUT && o.sessionID == sessionId && doctorPaymentServiceIdsList.Contains(o.InvoiceID))
                                                .Sum(o => o.CashAmount);

                decimal totalCashierCashAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.CASHIER &&  o.sessionID == sessionId && CHEinvoiceList.Contains(o.InvoiceID))
                                                .Sum(o => o.CashAmount);

                decimal totalCashierCardAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.CASHIER && o.sessionID == sessionId && CHEinvoiceList.Contains(o.InvoiceID))
                                                .Sum(o => o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal balanceAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.BALENCE && o.sessionID == sessionId && CHEinvoiceList.Contains(o.InvoiceID))
                                        .Sum(o => o.CashAmount);

                decimal refundAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.REFUND && o.sessionID == sessionId && CHEinvoiceList.Contains(o.InvoiceID))
                                        .Sum(o => o.CashAmount);

          
                cashierSessionAmounts.ChannelingTotalPaidCardAmount = totalCashierCardAmount;
                cashierSessionAmounts.ChannelingCashBalence = balanceAmount;
                cashierSessionAmounts.ChannelingTotalRefund = refundAmount;
                cashierSessionAmounts.ChannelingTotalDoctorPayment = totalDoctorPaymentAmount;
                cashierSessionAmounts.ChannelingTotalPaidAmount = totalCashierCashAmount + balanceAmount + refundAmount;
                cashierSessionAmounts.ChannelingTotalAmount = cashierSessionAmounts.ChannelingTotalPaidAmount + totalCashierCardAmount + totalDoctorPaymentAmount;


                return cashierSessionAmounts;
            }
        }
        private Model.CashierSession PaymentDetailsOPD(int sessionId, List<int> InvoiceIds)
        {
            Model.CashierSession cashierSessionAmounts = new Model.CashierSession();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {

                List<Invoice> invoiceList = dbContext.Invoices.Where(o => InvoiceIds.Contains(o.Id) && o.InvoiceType == InvoiceType.OPD && o.Status == 0).ToList();

                List<int> invoiceServiceIdList = invoiceList.Select(o => o.ServiceID).ToList();

                List<int> opdIds = dbContext.OPD.Where(o => invoiceServiceIdList.Contains(o.Id) && o.Status == CommonStatus.Active && o.Description == "OPD" && o.invoiceType == InvoiceType.OPD && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();

                List<int> invoiceIds = invoiceList.Where(o => opdIds.Contains(o.ServiceID) && o.InvoiceType == InvoiceType.OPD && o.Status == 0).Select(o => o.Id).ToList();

   

                decimal totalCashierCashAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.CASHIER && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                                .Sum(o => o.CashAmount);

                decimal totalCashierCardAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.CASHIER && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                                .Sum(o => o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal balanceAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.BALENCE && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                        .Sum(o => o.CashAmount);

                decimal refundAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.REFUND && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                        .Sum(o => o.CashAmount);


                cashierSessionAmounts.OPDTotalPaidCardAmount = totalCashierCardAmount;
                cashierSessionAmounts.OPDCashBalence = balanceAmount;
                cashierSessionAmounts.OPDTotalRefund = refundAmount;
                cashierSessionAmounts.OPDTotalPaidAmount = totalCashierCashAmount + balanceAmount + refundAmount;
                cashierSessionAmounts.OPDTotalAmount = cashierSessionAmounts.OPDTotalPaidAmount + totalCashierCardAmount;


                return cashierSessionAmounts;
            }
        }
        private Model.CashierSession PaymentDetailsXRAY(int sessionId, List<int> InvoiceIds)
        {
            Model.CashierSession cashierSessionAmounts = new Model.CashierSession();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {

                List<Invoice> invoiceList = dbContext.Invoices.Where(o => InvoiceIds.Contains(o.Id) && o.InvoiceType == InvoiceType.OPD && o.Status == 0).ToList();

                List<int> invoiceServiceIdList = invoiceList.Select(o => o.ServiceID).ToList();

                List<int> opdIds = dbContext.OPD.Where(o => invoiceServiceIdList.Contains(o.Id) && o.Status == CommonStatus.Active && o.Description == "X-RAY" && o.invoiceType == InvoiceType.OPD && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();

                List<int> invoiceIds = invoiceList.Where(o => opdIds.Contains(o.ServiceID) && o.InvoiceType == InvoiceType.OPD && o.Status == 0).Select(o => o.Id).ToList();



                decimal totalCashierCashAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.CASHIER && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                                .Sum(o => o.CashAmount);

                decimal totalCashierCardAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.CASHIER && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                                .Sum(o => o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal balanceAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.BALENCE && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                        .Sum(o => o.CashAmount);

                decimal refundAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.REFUND && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                        .Sum(o => o.CashAmount);


                cashierSessionAmounts.XRAYTotalPaidCardAmount = totalCashierCardAmount;
                cashierSessionAmounts.XRAYCashBalence = balanceAmount;
                cashierSessionAmounts.XRAYTotalRefund = refundAmount;
                cashierSessionAmounts.XRAYTotalPaidAmount = totalCashierCashAmount + balanceAmount + refundAmount;
                cashierSessionAmounts.XRAYTotalAmount = cashierSessionAmounts.XRAYTotalPaidAmount + totalCashierCardAmount;


                return cashierSessionAmounts;
            }
        }
        private Model.CashierSession PaymentDetailsOtherMS(int sessionId, List<int> InvoiceIds)
        {
            Model.CashierSession cashierSessionAmounts = new Model.CashierSession();

            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {

                List<Invoice> invoiceList = dbContext.Invoices.Where(o => InvoiceIds.Contains(o.Id) && o.InvoiceType == InvoiceType.OPD && o.Status == 0).ToList();

                List<int> invoiceServiceIdList = invoiceList.Select(o => o.ServiceID).ToList();

                List<int> opdIds = dbContext.OPD.Where(o => invoiceServiceIdList.Contains(o.Id) && o.Status == CommonStatus.Active && o.Description == "Other" && o.invoiceType == InvoiceType.OPD && o.paymentStatus == PaymentStatus.PAID).Select(r => r.Id).ToList();

                List<int> invoiceIds = invoiceList.Where(o => opdIds.Contains(o.ServiceID) && o.InvoiceType == InvoiceType.OPD && o.Status == 0).Select(o => o.Id).ToList();



                decimal totalCashierCashAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.CASHIER && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                                .Sum(o => o.CashAmount);

                decimal totalCashierCardAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.CASHIER && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                                .Sum(o => o.CreditAmount + o.DdebitAmount + o.ChequeAmount + o.GiftCardAmount);

                decimal balanceAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.BALENCE && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                        .Sum(o => o.CashAmount);

                decimal refundAmount = dbContext.Payments.Where(o => o.BillingType == BillingType.REFUND && o.sessionID == sessionId && invoiceIds.Contains(o.InvoiceID))
                                        .Sum(o => o.CashAmount);


                cashierSessionAmounts.OtherTotalPaidCardAmount = totalCashierCardAmount;
                cashierSessionAmounts.OtherCashBalence = balanceAmount;
                cashierSessionAmounts.OtherTotalRefund = refundAmount;
                cashierSessionAmounts.OtherTotalPaidAmount = totalCashierCashAmount + balanceAmount + refundAmount;
                cashierSessionAmounts.OtherTotalAmount = cashierSessionAmounts.OtherTotalPaidAmount + totalCashierCardAmount;


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
                    result.EndCardBalence = cashierSession.EndCardBalence;
                    result.cashierSessionStatus = cashierSession.cashierSessionStatus;
                    result.ModifiedUser = cashierSession.ModifiedUser;
                    result.ModifiedDate = cashierSession.ModifiedDate;



                    dbContext.SaveChanges();
                }
                return cashierSession;
            }
        }

        public List<CashierSessionDTO> GetAllCashierSessionSP()
        {
            using DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext();

            var cashierSessionDTOs = dbContext.Set<CashierSessionDTO>()
                .FromSqlRaw("EXEC GetAllCashierSessionDetails")
                .ToList();

            return cashierSessionDTOs;
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
                        Total = g.Sum(c => c.CashAmount+ c.CreditAmount + c.DdebitAmount + c.ChequeAmount + c.GiftCardAmount)
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
                if (result != null)
                {
                    result.col10Total = result.col10 * 5000;
                    result.col9Total = result.col9 * 1000;
                    result.col8Total = result.col8 * 500;
                    result.col7Total = result.col7 * 100;
                    result.col6Total = result.col6 * 50;
                    result.col5Total = result.col5 * 20;
                    result.col4Total = result.col4 * 10;
                    result.col3Total = result.col3 * 5;
                    result.col2Total = result.col2 * 2;
                    result.col1Total = result.col1 * 1;

                    result.totalCashAmountHandover = result.col10Total + result.col9Total + result.col8Total + result.col7Total + result.col6Total + result.col5Total + result.col4Total + result.col3Total + result.col2Total+ result.col2Total + result.col1Total;
                    result.totalAmountHandover = result.totalCashAmountHandover + result.EndCardBalence;
                }
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

        public Model.CashierSession UpdateCashierSessionStatus(int id,int userID)
        {


            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.CashierSession result = (from p in dbContext.CashierSessions where p.Id == id select p).SingleOrDefault();
                result.cashierSessionStatus = CashierSessionStatus.START;
                result.ModifiedDate = DateTime.Now;
                result.ModifiedUser = userID;
                dbContext.SaveChanges();
                return result;
            }
        }

    }
}
