using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.Specialist;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Channeling;
using HospitalMgrSystemUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using HospitalMgrSystem.Service.ChannelingSchedule;
using HospitalMgrSystem.Service.OPD;
using HospitalMgrSystem.Service.SMS;
using iTextSharp.text.pdf.parser.clipper;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.CashierSession;
using HospitalMgrSystem.Service.OtherTransactions;
using HospitalMgrSystem.Service.Cashier;

namespace HospitalMgrSystemUI.Controllers
{
    public class ChannelingSheduleController : Controller
    {
        private IConfiguration _configuration;

        [BindProperty]
        public ChannelingSheduleDto viewChannelingSchedule { get; set; }

        [BindProperty]
        public ChannelingSchedule myChannelingShedule { get; set; }

        public ChannelingSheduleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            ChannelingSheduleDto channelingSheduleDto = new ChannelingSheduleDto()
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Specialists = new ChannelingService().GetAllSpecialists(),
                channellingScheduleStatus = ChannellingScheduleStatus.ALL,
            };

            using (var httpClient = new HttpClient())
            {
                try
                {
                    channelingSheduleDto.ChannelingScheduleList = new ChannelingScheduleService().SheduleGetByStatus();
                    return View(channelingSheduleDto);

                }
                catch (Exception ex)
                {
                    return View();

                }
            }

        }

        public IActionResult FilterForm()
        {
            ChannelingSheduleDto channelingSheduleDto = new ChannelingSheduleDto()
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Specialists = new ChannelingService().GetAllSpecialists(),
                // channellingScheduleStatus = ChannellingScheduleStatus.ALL,
            };

            try
            {
                List<ChannelingSchedule> resultSet = new List<ChannelingSchedule>();

                // When the user selects only the date
                if (viewChannelingSchedule.SpecialistId == -2 && viewChannelingSchedule.channellingScheduleStatus == ChannellingScheduleStatus.ALL)
                {
                    resultSet = new ChannelingScheduleService().GetAllChannelingScheduleByDateTime(viewChannelingSchedule.StartTime, viewChannelingSchedule.EndTime);
                }

                // When the user select the speciality
                else if (viewChannelingSchedule.SpecialistId != -2 && viewChannelingSchedule.channellingScheduleStatus == ChannellingScheduleStatus.ALL)
                {
                    resultSet = new ChannelingScheduleService().GetAllChannelingScheduleByDateTimeWithSpeciality(viewChannelingSchedule.StartTime, viewChannelingSchedule.EndTime, viewChannelingSchedule.SpecialistId);
                }

                // When the user select the status
                else if (viewChannelingSchedule.SpecialistId == -2 && viewChannelingSchedule.channellingScheduleStatus != ChannellingScheduleStatus.ALL)
                {
                    resultSet = new ChannelingScheduleService().GetAllChannelingScheduleByDateTimeWithStatus(viewChannelingSchedule.StartTime, viewChannelingSchedule.EndTime, viewChannelingSchedule.channellingScheduleStatus);
                }

                // When the user select both the speciality and the status
                else if (viewChannelingSchedule.SpecialistId != -2 && viewChannelingSchedule.channellingScheduleStatus != ChannellingScheduleStatus.ALL)
                {
                    resultSet = new ChannelingScheduleService().GetAllChannelingScheduleByDateTimeWithSpecialityAndStatus(viewChannelingSchedule.StartTime, viewChannelingSchedule.EndTime, viewChannelingSchedule.channellingScheduleStatus, viewChannelingSchedule.SpecialistId);
                }

                channelingSheduleDto.ChannelingScheduleList = resultSet;

                return View("Index", channelingSheduleDto);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult CreateChannelShedule(int Id)
        {
            ChannelingSheduleDto channelingSheduleDto = new ChannelingSheduleDto();
            channelingSheduleDto.Specialists = LoadSpecialists();
            channelingSheduleDto.Consultants = LoadConsultants();
            if (Id > 0)
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {

                        channelingSheduleDto.ChannelingSchedule = LoadChannelingSheduleByID(Id);
                        return PartialView("_PartialAddChannelingShedule", channelingSheduleDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }

            }

            return PartialView("_PartialAddChannelingShedule", channelingSheduleDto);



        }

        private List<Specialist> LoadSpecialists()
        {
            try
            {
                List<Specialist> specialists = new SpecialistsService().GetSpecialist();
                return specialists;
            }
            catch (System.Exception ex)
            { }
            return null;
        }

        private List<Consultant> LoadConsultants()
        {
            List<Consultant> consultants = new List<Consultant>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    consultants = new ConsultantService().GetAllConsultantByStatus();

                }
                catch (Exception ex) { }
            }
            return consultants;
        }


        private List<ChannelingSchedule> LoadChannelingShedule()
        {
            List<ChannelingSchedule> channelingSchedule = new List<ChannelingSchedule>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    channelingSchedule = new ChannelingScheduleService().SheduleGetByStatus();

                }
                catch (Exception ex) { }
            }
            return channelingSchedule;
        }

        private ChannelingSchedule LoadChannelingSheduleByID(int id)
        {
            ChannelingSchedule channelingSchedule = new ChannelingSchedule();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    channelingSchedule = new ChannelingScheduleService().SheduleGetById(id);

                }
                catch (Exception ex) { }
            }
            return channelingSchedule;
        }

        public async Task<IActionResult> AddNewChannelingSheduleAsync()
        {
            var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];

            using (var httpClient = new HttpClient())
            {
                try
                {
                    if (viewChannelingSchedule.ChannelingSchedule.scheduleStatus != ChannellingScheduleStatus.NOT_ACTIVE && viewChannelingSchedule.ChannelingSchedule.scheduleStatus != ChannellingScheduleStatus.ACTIVE)
                    {
                        ChannelingSMS channelingSMS = new ChannelingSMS();

                        channelingSMS.channeling = new OPDService().GetAllOPDBySchedularID(viewChannelingSchedule.ChannelingSchedule.Id);
                        channelingSMS.channelingSchedule = LoadChannelingSheduleByID(viewChannelingSchedule.ChannelingSchedule.Id);
                        channelingSMS.ChannellingScheduleStatus = viewChannelingSchedule.ChannelingSchedule.scheduleStatus;

                        // Add temp mobile number to last record
                        channelingSMS.channeling[channelingSMS.channeling.Count - 1].patient.MobileNumber = "0702869830";

                        SMSService sMSService = new SMSService();
                        //await sMSService.SendSMSToken(channelingSMS);

                    }

                    if (viewChannelingSchedule.ChannelingSchedule.scheduleStatus == ChannellingScheduleStatus.SESSION_END)
                    {
                        List<CashierSession> mtList = new List<CashierSession>();
                        ChannelingSchedule channelingScheduleData = new ChannelingScheduleService().SheduleGetById(viewChannelingSchedule.ChannelingSchedule.Id);
                        mtList = GetActiveCashierSession(Convert.ToInt32(userIdCookie));

                        if (mtList.Count > 0)
                        {
                            OtherTransactions otherTransactions = new OtherTransactions();

                            otherTransactions.SessionID = mtList[0].Id;
                            otherTransactions.ConvenerID = Convert.ToInt32(userIdCookie);
                            otherTransactions.InvoiceType = InvoiceType.DOCTOR_PAYMENT;
                            otherTransactions.Amount = channelingScheduleData.totalDoctorFeeAmount;
                            otherTransactions.Description = "DOCTOR_PAYMENT";
                            otherTransactions.ApprovedByID = Convert.ToInt32(userIdCookie);
                            otherTransactions.Status = CommonStatus.Active;
                            otherTransactions.otherTransactionsStatus = OtherTransactionsStatus.Cashier_Out;
                            otherTransactions.CreateUser = Convert.ToInt32(userIdCookie);
                            otherTransactions.ModifiedUser = Convert.ToInt32(userIdCookie);
                            otherTransactions.CreateDate = DateTime.Now;
                            otherTransactions.ModifiedDate = DateTime.Now;
                            otherTransactions.BeneficiaryID = channelingScheduleData.ConsultantId;
                            otherTransactions.SchedularId = viewChannelingSchedule.ChannelingSchedule.Id;

                            OtherTransactions savedOtherTransaction = new OtherTransactionsService().CreateOtherTransactions(otherTransactions);

                            Invoice invoice = new Invoice();

                            invoice.CustomerID = channelingScheduleData.ConsultantId;
                            invoice.CustomerName = channelingScheduleData.Consultant.Name;
                            invoice.InvoiceType = InvoiceType.DOCTOR_PAYMENT;
                            invoice.paymentStatus = PaymentStatus.PAID;
                            invoice.Status = InvoiceStatus.New;
                            invoice.CreateUser = Convert.ToInt32(userIdCookie);
                            invoice.ModifiedUser = Convert.ToInt32(userIdCookie);
                            invoice.CreateDate = DateTime.Now;
                            invoice.ModifiedDate = DateTime.Now;
                            invoice.ServiceID = savedOtherTransaction.Id;


                            Invoice savedInvoice = new CashierService().AddInvoice(invoice);

                            InvoiceItem invoiceItem = new InvoiceItem();

                            invoiceItem.InvoiceId = savedInvoice.Id;
                            invoiceItem.ItemID = 2;
                            invoiceItem.billingItemsType = BillingItemsType.OTHER;
                            invoiceItem.price = channelingScheduleData.totalDoctorFeeAmount;
                            invoiceItem.qty = 1;
                            invoiceItem.Discount = 0;
                            invoiceItem.Total = invoiceItem.price * invoiceItem.qty;
                            invoiceItem.CreateDate = DateTime.Now;
                            invoiceItem.CreateUser = Convert.ToInt32(userIdCookie);
                            invoiceItem.ModifiedDate = DateTime.Now;
                            invoiceItem.ModifiedUser = Convert.ToInt32(userIdCookie);
                            invoiceItem.Status = CommonStatus.Active;
                            invoiceItem.itemInvoiceStatus = ItemInvoiceStatus.BILLED;

                            InvoiceItem savedInvoiceItem = new CashierService().AddSingleInvoiceItem(invoiceItem);

                            Payment payment = new Payment();

                            payment.InvoiceID = savedInvoice.Id;
                            payment.CashAmount = 0 - invoiceItem.Total;
                            payment.CreditAmount = 0;
                            payment.DdebitAmount = 0;
                            payment.ChequeAmount = 0;
                            payment.GiftCardAmount = 0;
                            payment.CreateUser = Convert.ToInt32(userIdCookie);
                            payment.ModifiedUser = Convert.ToInt32(userIdCookie);
                            payment.CreateDate = DateTime.Now;
                            payment.ModifiedDate = DateTime.Now;
                            payment.CashierStatus = CashierStatus.CashierOut;
                            payment.BillingType = BillingType.OTHER_OUT;
                            payment.sessionID = mtList[0].Id;

                            Payment savedPayment = new CashierService().AddPayments(payment);
                            
                        }

                    }

                    viewChannelingSchedule.ChannelingSchedule.Consultant = null;
                    ChannelingSchedule channelingSchedule = new ChannelingScheduleService().CreateChannelingSchedule(viewChannelingSchedule.ChannelingSchedule);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }

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

        public IActionResult SheduleGetByConsultantIdandDate()
        {
            ChannelingSchedule channelingSchedule = new ChannelingSchedule();
            using (var httpClient = new HttpClient())
            {

                try
                {
                    channelingSchedule = new ChannelingScheduleService().CreateChannelingSchedule(viewChannelingSchedule.ChannelingSchedule);
                    string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    httpClient.BaseAddress = new Uri(APIUrl + "SheduleGetByConsultantIdandDate/");
                    var postObj = httpClient.PostAsJsonAsync<ChannelingSchedule>("DeleteDrug?", myChannelingShedule);
                    postObj.Wait();
                    var res = postObj.Result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public IActionResult DeleteChannelingShedule()
        {
            ChannelingSchedule channelingSchedule = new ChannelingSchedule();
            using (var httpClient = new HttpClient())
            {

                try
                {
                    channelingSchedule = new ChannelingScheduleService().DeleteChannelingShedule(myChannelingShedule.Id);
                    //string APIUrl = _configuration.GetValue<string>("MainAPI:APIURL");
                    //httpClient.BaseAddress = new Uri(APIUrl + "ChannelingShedule/");
                    //var postObj = httpClient.PostAsJsonAsync<ChannelingSchedule>("DeleteChannelingShedule?", myChannelingShedule);
                    //postObj.Wait();
                    //var res = postObj.Result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }
    }
}
