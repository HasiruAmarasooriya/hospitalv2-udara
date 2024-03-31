using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.OPD;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System;
using HospitalMgrSystemUI.Models;
using HospitalMgrSystem.Model.Enums;
using HospitalMgrSystem.Service.Cashier;
using System.Threading.Tasks;
using HospitalMgrSystem.Service.Drugs;
using HospitalMgrSystem.Service.Investigation;
using HospitalMgrSystem.Service.Item;
using HospitalMgrSystem.Service.Channeling;
using System.Security.Cryptography;
using HospitalMgrSystem.Service.Consultant;
using HospitalMgrSystem.Service.Patients;
using Microsoft.Reporting.NETCore;
using System.Data;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using Azure.Core;
using static iTextSharp.text.pdf.AcroFields;
using HospitalMgrSystem.Service.CashierSession;
using HospitalMgrSystem.Service.User;
using HospitalMgrSystem.Service.NightShiftSession;

namespace HospitalMgrSystemUI.Controllers
{
    public class CashierController : Controller
    {
        [BindProperty]
        public CashierDto _CashierDto { get; set; }

        [BindProperty]
        public OPDDto _OPDDto { get; set; }

        [BindProperty]
        public OPD myOPD { get; set; }

        [BindProperty]
        public string SearchValue { get; set; }

        //PRINT RECEIPT
        private readonly ILogger<CashierController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private static List<Stream> m_streams;
        private static int m_currentPageIndex = 0;

        public CashierController(ILogger<CashierController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }


        public ActionResult Index(string PreID)
        {
            CashierDto cashierDto = new CashierDto();
            if (_CashierDto != null || PreID != null)
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
                        int userID = Convert.ToInt32(userIdCookie);

                        User user = GetUserById(userID);
                        if (user.userRole == UserRole.CASHIER || user.userRole == UserRole.ADMIN)
                        {
                            if (GetActiveCashierSession(userID).Count == 0)
                            {
                                return View();
                            }
                        }

                        // PreID = "OPD" + PreID;
                        if (PreID != null)
                        {
                            cashierDto.PreID = PreID;
                        }
                        if (_CashierDto != null && _CashierDto.PreID != null)
                        {
                            cashierDto.PreID = _CashierDto.PreID;
                        }
                        cashierDto = GetCashierAlldetails(cashierDto.PreID);
                        cashierDto.cashierRemoveBillingItemDtoList = GetCashierAllRemoveddetails(cashierDto.PreID);
                        cashierDto.userRole = user.userRole;
                        return View(cashierDto);
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }
                }

            }
            else
            {
                return View();
            }

        }

        public ActionResult PrintInvoice([FromBody] CashierDto cashierDtoPar)
        {
            CashierDto cashierDto = new CashierDto();
            using (var httpClient = new HttpClient())
            {

                try
                {
                    cashierDto.PreID = cashierDtoPar.PreID;

                    cashierDto = GetCashierAlldetails(cashierDto.PreID);
                    cashierDto.cashierRemoveBillingItemDtoList = GetCashierAllRemoveddetails(cashierDto.PreID);

                    return PartialView("_PartialViewInvoice", cashierDto);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult PrintBill([FromBody] CashierDto cashierDtoPar)
        {
            CashierDto cashierDto = new CashierDto();
            using (var httpClient = new HttpClient())
            {

                try
                {
                    cashierDto.PreID = cashierDtoPar.PreID;

                    cashierDto = GetCashierAlldetails(cashierDto.PreID);
                    cashierDto.cashierRemoveBillingItemDtoList = GetCashierAllRemoveddetails(cashierDto.PreID);

                    return PartialView("_PartialReceipt", cashierDto);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        private CashierDto GetCashierAlldetails(string preID)
        {
            try
            {
                CashierDto cashierDto = new CashierDto();
                List<BillingItemDto> billingItemDtoList = new List<BillingItemDto>();
                List<BillingItemDto> billedItemDtoList = new List<BillingItemDto>();

                decimal subtotal = 0;
                decimal discount = 0;
                decimal total = 0;

                string input = preID;
                string prefix = GetPrefix(input);
                int number = GetNumber(input);

                if (prefix == "OPD")
                {
                    OPD opd = new OPDService().GetAllOPDByID(number);
                    cashierDto.opd = opd;
                    cashierDto.consaltantName = opd.consultant != null && opd.consultant.Name != null ? opd.consultant.Name : string.Empty;

                    cashierDto.customerName = opd.patient != null ? opd.patient.FullName : string.Empty;
                    cashierDto.patientContactNo = opd.patient != null ? opd.patient.MobileNumber : string.Empty;
                    cashierDto.patientAge = opd.patient != null ? opd.patient.Age : 0;
                    cashierDto.patientSex = opd.patient != null ? (SexStatus)opd.patient.Sex : SexStatus.Non;

                    cashierDto.hospitalFee = opd.HospitalFee;
                    cashierDto.consaltantFee = opd.ConsultantFee;
                    cashierDto.defaultAmountOPD = opd.ConsultantFee + opd.HospitalFee;
                    cashierDto.invoiceType = InvoiceType.OPD;
                    cashierDto.customerID = opd.PatientID;
                    cashierDto.OPDDrugusList = GetOPDDrugus(number, ItemInvoiceStatus.Add);
                    cashierDto.OPDDrugusListInvoiced = GetOPDDrugus(number, ItemInvoiceStatus.BILLED);
                    cashierDto.OPDInvestigationList = GetOPDInvestigation(number, ItemInvoiceStatus.Add);
                    cashierDto.OPDItemList = GetOPDItems(number, ItemInvoiceStatus.Add);
                    cashierDto.invoice = new CashierService().GetInvoiceByServiceIDAndInvoiceType(number, InvoiceType.OPD);
                    if (cashierDto.invoice != null)
                    {
                        if (checkHospitalFeeRemoved(cashierDto.invoice.Id))
                        {
                            cashierDto.hospitalFee = 0;

                        }
                        if (checkConsultantFeeRemoved(cashierDto.invoice.Id))
                        {
                            cashierDto.consaltantFee = 0;
                        }
                    }
                    if (cashierDto.OPDDrugusList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDDrugusList)
                        {
                            decimal itemAmount = item.Qty * item.Price;
                            subtotal = subtotal + itemAmount;
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = item.Id,
                                billingItemsType = BillingItemsType.Drugs,
                                billingItemsTypeName = item.billingItemsType.ToString(),
                                billingItemName = item.Drug.DrugName,
                                qty = item.Qty,
                                price = item.Price,
                                discount = 0,
                                amount = item.Amount
                            });
                        }
                    }


                    if (cashierDto.OPDDrugusListInvoiced.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDDrugusListInvoiced)
                        {
                            decimal itemAmount = item.Qty * item.Price;
                            subtotal = subtotal + itemAmount;
                            billedItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = item.Id,
                                billingItemsType = BillingItemsType.Drugs,
                                billingItemsTypeName = item.billingItemsType.ToString(),
                                billingItemName = item.Drug.DrugName,
                                qty = item.Qty,
                                price = item.Price,
                                discount = 0,
                                amount = item.Amount
                            });
                        }
                    }
                    #region Not Applicable
                    if (cashierDto.OPDInvestigationList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDInvestigationList)
                        {
                            decimal itemAmount = item.Qty * item.Price;
                            subtotal = subtotal + itemAmount;
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = item.Id,
                                billingItemsType = BillingItemsType.Investigation,
                                billingItemsTypeName = "Investigation",
                                billingItemName = item.Investigation.InvestigationName,
                                qty = item.Qty,
                                price = item.Price,
                                discount = 0,
                                amount = item.Amount
                            });
                        }
                    }

                    if (cashierDto.OPDItemList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDItemList)
                        {
                            decimal itemAmount = item.Qty * item.Price;
                            subtotal = subtotal + itemAmount;
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = item.Id,
                                billingItemsType = BillingItemsType.Items,
                                billingItemsTypeName = "Item",
                                billingItemName = item.Item.ItemName,
                                qty = item.Qty,
                                price = item.Price,
                                discount = 0,
                                amount = item.Amount
                            });
                        }
                    }
                    #endregion

                    subtotal = subtotal + cashierDto.hospitalFee + cashierDto.consaltantFee;

                }

                if (prefix == "CHE")
                {
                    OPD opd = new OPDService().GetAllOPDByID(number);
                    cashierDto.opd = opd;
                    cashierDto.consaltantName = opd.consultant != null && opd.consultant.Name != null ? opd.consultant.Name : string.Empty;

                    cashierDto.customerName = opd.patient != null ? opd.patient.FullName : string.Empty;
                    cashierDto.patientContactNo = opd.patient != null ? opd.patient.MobileNumber : string.Empty;
                    cashierDto.patientAge = opd.patient != null ? opd.patient.Age : 0;
                    cashierDto.patientSex = opd.patient != null ? (SexStatus)opd.patient.Sex : SexStatus.Non;

                    cashierDto.hospitalFee = opd.HospitalFee;
                    cashierDto.consaltantFee = opd.ConsultantFee;
                    cashierDto.defaultAmountOPD = opd.ConsultantFee + opd.HospitalFee;
                    cashierDto.invoiceType = InvoiceType.CHE;
                    cashierDto.customerID = opd.PatientID;
                    cashierDto.OPDDrugusList = GetOPDDrugus(number, ItemInvoiceStatus.Add);
                    cashierDto.OPDDrugusListInvoiced = GetOPDDrugus(number, ItemInvoiceStatus.BILLED);
                    cashierDto.invoice = new CashierService().GetInvoiceByServiceIDAndInvoiceType(number, InvoiceType.CHE);
                    if (cashierDto.invoice != null)
                    {
                        if (checkHospitalFeeRemoved(cashierDto.invoice.Id))
                        {
                            cashierDto.hospitalFee = 0;

                        }
                        if (checkConsultantFeeRemoved(cashierDto.invoice.Id))
                        {
                            cashierDto.consaltantFee = 0;
                        }
                    }
                    if (cashierDto.OPDDrugusList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDDrugusList)
                        {
                            decimal itemAmount = item.Qty * item.Price;
                            subtotal = subtotal + itemAmount;
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = item.Id,
                                billingItemsType = BillingItemsType.Drugs,
                                billingItemsTypeName = item.billingItemsType.ToString(),
                                billingItemName = item.Drug.DrugName,
                                qty = item.Qty,
                                price = item.Price,
                                discount = 0,
                                amount = item.Amount
                            });
                        }
                    }

                    if (cashierDto.OPDDrugusListInvoiced.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDDrugusListInvoiced)
                        {
                            decimal itemAmount = item.Qty * item.Price;
                            subtotal = subtotal + itemAmount;
                            billedItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = item.Id,
                                billingItemsType = BillingItemsType.Drugs,
                                billingItemsTypeName = item.billingItemsType.ToString(),
                                billingItemName = item.Drug.DrugName,
                                qty = item.Qty,
                                price = item.Price,
                                discount = 0,
                                amount = item.Amount
                            });
                        }
                    }

                    subtotal = subtotal + cashierDto.hospitalFee + cashierDto.consaltantFee;

                }
                #region Not Applicable
                //if (prefix == "CHE")
                //{
                //    cashierDto.channel = GetChannelByID(number);
                //    cashierDto.consaltantName = cashierDto.channel != null && cashierDto.channel.ChannelingSchedule != null && cashierDto.channel.ChannelingSchedule.Consultant != null && cashierDto.channel.ChannelingSchedule.Consultant.Name != null ? cashierDto.channel.ChannelingSchedule.Consultant.Name : string.Empty;
                //    cashierDto.patientContactNo = cashierDto.channel != null && cashierDto.channel.Patient != null && cashierDto.channel.Patient.FullName != null ? cashierDto.channel.Patient.TelephoneNumber : string.Empty;
                //    cashierDto.customerName = cashierDto.channel != null && cashierDto.channel.Patient != null && cashierDto.channel.Patient.FullName != null ? cashierDto.channel.Patient.FullName : string.Empty;
                //    cashierDto.patientAge = cashierDto.channel != null && cashierDto.channel.Patient != null && cashierDto.channel.Patient.FullName != null ? cashierDto.channel.Patient.Age : 0;
                //    cashierDto.patientSex = cashierDto.channel != null && cashierDto.channel.Patient != null && cashierDto.channel.Patient.FullName != null ? (SexStatus)cashierDto.channel.Patient.Sex : SexStatus.Non;
                //    cashierDto.hospitalFee = cashierDto.channel.ChannelingSchedule.HospitalFee;
                //    cashierDto.consaltantFee = cashierDto.channel.ChannelingSchedule.ConsultantFee;
                //    subtotal = cashierDto.channel.ChannelingSchedule.HospitalFee + cashierDto.channel.ChannelingSchedule.ConsultantFee;
                //    cashierDto.invoiceType = InvoiceType.CHE;
                //    cashierDto.customerID = cashierDto.channel.Patient.Id;
                //    cashierDto.discount = discount;
                //    cashierDto.invoice = new CashierService().GetInvoiceByServiceIDAndInvoiceType(number, InvoiceType.CHE);

                //}

                #endregion

                if (cashierDto.invoice != null)
                {
                    cashierDto.invoiceID = cashierDto.invoice.Id;
                    // cashierDto.customerName=cashierDto.invoice.CustomerName;
                    cashierDto.paymentList = new CashierService().GetAllPaymentsByInvoiceID(cashierDto.invoice.Id);
                    foreach (var item in cashierDto.paymentList)
                    {
                        decimal subtot = item.CashAmount + item.ChequeAmount + item.CreditAmount + item.DdebitAmount + item.GiftCardAmount;
                        cashierDto.totalPaymentPaidAmount = cashierDto.totalPaymentPaidAmount + subtot;
                    }

                    cashierDto.InvoiceItemList = new CashierService().GetInvoiceItemByInvoicedID(cashierDto.invoice.Id);
                    bool hospitalFeeBilled = false;
                    bool consaltantFeeBilled = false;
                    foreach (var item in cashierDto.InvoiceItemList)
                    {
                        if (item.billingItemsType == BillingItemsType.Hospital)
                        {
                            hospitalFeeBilled = true;
                            if (!checkHospitalFeeRemoved(cashierDto.invoice.Id))
                            {

                                billedItemDtoList.Add(new BillingItemDto()
                                {
                                    BillingItemID = 2,
                                    billingItemsType = BillingItemsType.Hospital,
                                    billingItemsTypeName = "",
                                    billingItemName = "Hospital Fee",
                                    qty = 1,
                                    price = cashierDto.hospitalFee,
                                    discount = 0,
                                    amount = cashierDto.hospitalFee
                                });

                            }


                        }
                        if (item.billingItemsType == BillingItemsType.Consultant)
                        {
                            consaltantFeeBilled = true;
                            if (!checkConsultantFeeRemoved(cashierDto.invoice.Id))
                            {
                                billedItemDtoList.Add(new BillingItemDto()
                                {
                                    BillingItemID = 1,
                                    billingItemsType = BillingItemsType.Consultant,
                                    billingItemsTypeName = "",
                                    billingItemName = "Consultant Fee",
                                    qty = 1,
                                    price = cashierDto.consaltantFee,
                                    discount = 0,
                                    amount = cashierDto.consaltantFee
                                });
                            }

                        }
                    }

                    if (!consaltantFeeBilled)
                    {
                        billingItemDtoList.Add(new BillingItemDto()
                        {
                            BillingItemID = 1,
                            billingItemsType = BillingItemsType.Consultant,
                            billingItemsTypeName = "",
                            billingItemName = "Consultant Fee",
                            qty = 1,
                            price = cashierDto.consaltantFee,
                            discount = 0,
                            amount = cashierDto.consaltantFee
                        });
                    }


                    if (!hospitalFeeBilled)
                    {
                        billingItemDtoList.Add(new BillingItemDto()
                        {
                            BillingItemID = 2,
                            billingItemsType = BillingItemsType.Hospital,
                            billingItemsTypeName = "",
                            billingItemName = "Hospital Fee",
                            qty = 1,
                            price = cashierDto.hospitalFee,
                            discount = 0,
                            amount = cashierDto.hospitalFee
                        });
                    }


                }
                else
                {
                    billingItemDtoList.Add(new BillingItemDto()
                    {
                        BillingItemID = 1,
                        billingItemsType = BillingItemsType.Consultant,
                        billingItemsTypeName = "",
                        billingItemName = "Consultant Fee",
                        qty = 1,
                        price = cashierDto.consaltantFee,
                        discount = 0,
                        amount = cashierDto.consaltantFee
                    });

                    billingItemDtoList.Add(new BillingItemDto()
                    {
                        BillingItemID = 2,
                        billingItemsType = BillingItemsType.Hospital,
                        billingItemsTypeName = "",
                        billingItemName = "Hospital Fee",
                        qty = 1,
                        price = cashierDto.hospitalFee,
                        discount = 0,
                        amount = cashierDto.hospitalFee
                    });
                }


                cashierDto.PreID = input;
                cashierDto.sufID = number;
                cashierDto.preSubtotal = subtotal;
                cashierDto.subtotal = subtotal - cashierDto.totalPaymentPaidAmount;
                cashierDto.discount = discount;
                cashierDto.preTotal = cashierDto.preSubtotal - discount;
                cashierDto.total = cashierDto.subtotal - discount;
                cashierDto.cashierBillingItemDtoList = billingItemDtoList;
                cashierDto.cashierBilledItemDtoList = billedItemDtoList;
                return cashierDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private List<BillingItemDto> GetCashierAllRemoveddetails(string preID)
        {
            try
            {
                CashierDto cashierDto = new CashierDto();
                List<BillingItemDto> billingItemDtoList = new List<BillingItemDto>();

                decimal subtotal = 0;
                decimal discount = 0;
                decimal total = 0;

                string input = preID;
                string prefix = GetPrefix(input);
                int number = GetNumber(input);

                if (prefix == "OPD")
                {
                    OPD opd = new OPDService().GetAllOPDByID(number);
                    cashierDto.OPDDrugusList = GetOPDDrugus(number, ItemInvoiceStatus.Remove);
                    cashierDto.OPDInvestigationList = GetOPDInvestigation(number, ItemInvoiceStatus.Remove);
                    cashierDto.OPDItemList = GetOPDItems(number, ItemInvoiceStatus.Remove);
                    cashierDto.invoice = new CashierService().GetInvoiceByServiceIDAndInvoiceType(number, InvoiceType.OPD);
                    if (cashierDto.OPDDrugusList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDDrugusList)
                        {
                            decimal itemAmount = item.Qty * item.Price;
                            subtotal = subtotal + itemAmount;
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = item.Id,
                                billingItemsType = BillingItemsType.Drugs,
                                billingItemsTypeName = "Drug",
                                billingItemName = item.Drug.DrugName,
                                qty = item.Qty,
                                price = item.Price,
                                discount = 0,
                                amount = item.Amount
                            });
                        }
                    }
                    if (cashierDto.invoice != null)
                    {
                        int invoiceID = cashierDto.invoice.Id;
                        if (checkHospitalFeeRemoved(invoiceID))
                        {
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = 2,
                                billingItemsType = BillingItemsType.Hospital,
                                billingItemsTypeName = "Hospital",
                                billingItemName = "Hospital",
                                qty = 1,
                                price = opd.HospitalFee,
                                discount = 0,
                                amount = opd.HospitalFee
                            });
                        }

                        if (checkConsultantFeeRemoved(invoiceID))
                        {
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = 1,
                                billingItemsType = BillingItemsType.Consultant,
                                billingItemsTypeName = "Consultant Fee",
                                billingItemName = "Consultant Fee",
                                qty = 1,
                                price = opd.ConsultantFee,
                                discount = 0,
                                amount = opd.ConsultantFee
                            });
                        }
                    }



                    #region Not Applicable

                    if (cashierDto.OPDInvestigationList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDInvestigationList)
                        {
                            decimal itemAmount = item.Qty * item.Price;
                            subtotal = subtotal + itemAmount;
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = item.Id,
                                billingItemsType = BillingItemsType.Investigation,
                                billingItemsTypeName = "Investigation",
                                billingItemName = item.Investigation.InvestigationName,
                                qty = item.Qty,
                                price = item.Price,
                                discount = 0,
                                amount = item.Amount
                            });
                        }
                    }

                    if (cashierDto.OPDItemList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDItemList)
                        {
                            decimal itemAmount = item.Qty * item.Price;
                            subtotal = subtotal + itemAmount;
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = item.Id,
                                billingItemsType = BillingItemsType.Items,
                                billingItemsTypeName = "Item",
                                billingItemName = item.Item.ItemName,
                                qty = item.Qty,
                                price = item.Price,
                                discount = 0,
                                amount = item.Amount
                            });
                        }
                    }

                    #endregion

                }


                //if (prefix == "CHE")
                //{
                //    OPD opd = new OPDService().GetAllOPDByID(number);
                //    cashierDto.OPDDrugusList = GetOPDDrugus(number, ItemInvoiceStatus.Remove);
                //    cashierDto.invoice = new CashierService().GetInvoiceByServiceIDAndInvoiceType(number, InvoiceType.CHE);
                //    if (cashierDto.OPDDrugusList.Count > 0)
                //    {
                //        foreach (var item in cashierDto.OPDDrugusList)
                //        {
                //            decimal itemAmount = item.Qty * item.Price;
                //            subtotal = subtotal + itemAmount;
                //            billingItemDtoList.Add(new BillingItemDto()
                //            {
                //                BillingItemID = item.Id,
                //                billingItemsType = BillingItemsType.Drugs,
                //                billingItemsTypeName = "Drug",
                //                billingItemName = item.Drug.DrugName,
                //                qty = item.Qty,
                //                price = item.Price,
                //                discount = 0,
                //                amount = item.Amount
                //            });
                //        }
                //    }

                //}

                if (prefix == "CHE")
                {
                    OPD opd = new OPDService().GetAllOPDByID(number);
                    cashierDto.OPDDrugusList = GetOPDDrugus(number, ItemInvoiceStatus.Remove);
                    cashierDto.OPDInvestigationList = GetOPDInvestigation(number, ItemInvoiceStatus.Remove);
                    cashierDto.OPDItemList = GetOPDItems(number, ItemInvoiceStatus.Remove);
                    cashierDto.invoice = new CashierService().GetInvoiceByServiceIDAndInvoiceType(number, InvoiceType.CHE);
                    if (cashierDto.OPDDrugusList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDDrugusList)
                        {
                            decimal itemAmount = item.Qty * item.Price;
                            subtotal = subtotal + itemAmount;
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = item.Id,
                                billingItemsType = BillingItemsType.Drugs,
                                billingItemsTypeName = "Drug",
                                billingItemName = item.Drug.DrugName,
                                qty = item.Qty,
                                price = item.Price,
                                discount = 0,
                                amount = item.Amount
                            });
                        }
                    }
                    if (cashierDto.invoice != null)
                    {
                        int invoiceID = cashierDto.invoice.Id;
                        if (checkHospitalFeeRemoved(invoiceID))
                        {
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = 2,
                                billingItemsType = BillingItemsType.Hospital,
                                billingItemsTypeName = "Hospital",
                                billingItemName = "Hospital",
                                qty = 1,
                                price = opd.HospitalFee,
                                discount = 0,
                                amount = opd.HospitalFee
                            });
                        }

                        if (checkConsultantFeeRemoved(invoiceID))
                        {
                            billingItemDtoList.Add(new BillingItemDto()
                            {
                                BillingItemID = 1,
                                billingItemsType = BillingItemsType.Consultant,
                                billingItemsTypeName = "Consultant Fee",
                                billingItemName = "Consultant Fee",
                                qty = 1,
                                price = opd.ConsultantFee,
                                discount = 0,
                                amount = opd.ConsultantFee
                            });
                        }
                    }

                }

                #region Not Applicable

                //if (prefix == "CHE")
                //{
                //    cashierDto.OPDDrugusList = GetOPDDrugus(number, ItemInvoiceStatus.Add);
                //    cashierDto.OPDInvestigationList = GetOPDInvestigation(number, ItemInvoiceStatus.Add);
                //    cashierDto.OPDItemList = GetOPDItems(number, ItemInvoiceStatus.Add);

                //}

                //if (cashierDto.invoice != null)
                //{
                //    cashierDto.customerName = cashierDto.invoice.CustomerName;
                //    cashierDto.paymentList = new CashierService().GetAllPaymentsByInvoiceID(cashierDto.invoice.Id);
                //    foreach (var item in cashierDto.paymentList)
                //    {
                //        decimal subtot = item.CashAmount + item.ChequeAmount + item.CreditAmount + item.DdebitAmount + item.GiftCardAmount;
                //        cashierDto.totalPaymentPaidAmount = cashierDto.totalPaymentPaidAmount + subtot;
                //    }
                //}
                //cashierDto.PreID = input;
                //cashierDto.sufID = number;
                //cashierDto.subtotal = subtotal - cashierDto.totalPaymentPaidAmount;
                //cashierDto.discount = discount;
                //cashierDto.total = cashierDto.subtotal - discount;
                //
                #endregion

                return billingItemDtoList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private List<InvoiceItem> GetInvoiceItemsAlldetails(int id, InvoiceType prefix, int number)
        {
            try
            {
                CashierDto cashierDto = new CashierDto();
                List<InvoiceItem> billingItemDtoList = new List<InvoiceItem>();


                if (prefix == InvoiceType.OPD)
                {
                    OPD opd = new OPDService().GetAllOPDByID(number);
                    cashierDto.invoiceType = InvoiceType.OPD;
                    cashierDto.customerID = opd.PatientID;
                    cashierDto.OPDDrugusList = GetOPDDrugus(number, ItemInvoiceStatus.Add);
                    cashierDto.OPDInvestigationList = GetOPDInvestigation(number, ItemInvoiceStatus.Add);
                    cashierDto.OPDItemList = GetOPDItems(number, ItemInvoiceStatus.Add);
                    if (cashierDto.OPDDrugusList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDDrugusList)
                        {
                            billingItemDtoList.Add(new InvoiceItem()
                            {
                                InvoiceId = id,
                                ItemID = item.Id,
                                itemInvoiceStatus = ItemInvoiceStatus.BILLED,
                                billingItemsType = BillingItemsType.Drugs,
                                qty = item.Qty,
                                price = item.Price,
                                Discount = 0,
                                Total = item.Amount
                            });
                        }
                    }

                    billingItemDtoList.Add(new InvoiceItem()
                    {
                        InvoiceId = id,
                        ItemID = 2,
                        itemInvoiceStatus = ItemInvoiceStatus.BILLED,
                        billingItemsType = BillingItemsType.Hospital,
                        qty = 1,
                        price = opd.HospitalFee,
                        Discount = 0,
                        Total = opd.HospitalFee
                    });

                    #region Not Applicable

                    if (cashierDto.OPDInvestigationList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDInvestigationList)
                        {
                            billingItemDtoList.Add(new InvoiceItem()
                            {
                                InvoiceId = id,
                                ItemID = item.Id,
                                billingItemsType = BillingItemsType.Investigation,
                                qty = item.Qty,
                                price = item.Price,
                                Discount = 0,
                                Total = item.Amount
                            });
                        }
                    }

                    if (cashierDto.OPDItemList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDItemList)
                        {
                            billingItemDtoList.Add(new InvoiceItem()
                            {
                                InvoiceId = id,
                                ItemID = item.Id,
                                billingItemsType = BillingItemsType.Items,
                                qty = item.Qty,
                                price = item.Price,
                                Discount = 0,
                                Total = item.Amount
                            });
                        }
                    }
                    #endregion

                }

                if (prefix == InvoiceType.CHE)
                {
                    //cashierDto.OPDDrugusList = GetOPDDrugus(number, ItemInvoiceStatus.Add);
                    //cashierDto.OPDInvestigationList = GetOPDInvestigation(number, ItemInvoiceStatus.Add);
                    //cashierDto.OPDItemList = GetOPDItems(number, ItemInvoiceStatus.Add);

                    OPD opd = new OPDService().GetAllOPDByID(number);
                    cashierDto.invoiceType = InvoiceType.CHE;
                    cashierDto.customerID = opd.PatientID;
                    cashierDto.OPDDrugusList = GetOPDDrugus(number, ItemInvoiceStatus.Add);
                    if (cashierDto.OPDDrugusList.Count > 0)
                    {
                        foreach (var item in cashierDto.OPDDrugusList)
                        {
                            billingItemDtoList.Add(new InvoiceItem()
                            {
                                InvoiceId = id,
                                ItemID = item.Id,
                                itemInvoiceStatus = ItemInvoiceStatus.BILLED,
                                billingItemsType = BillingItemsType.Drugs,
                                qty = item.Qty,
                                price = item.Price,
                                Discount = 0,
                                Total = item.Amount
                            });
                        }
                    }

                    billingItemDtoList.Add(new InvoiceItem()
                    {
                        InvoiceId = id,
                        ItemID = 2,
                        itemInvoiceStatus = ItemInvoiceStatus.BILLED,
                        billingItemsType = BillingItemsType.Hospital,
                        qty = 1,
                        price = opd.HospitalFee,
                        Discount = 0,
                        Total = opd.HospitalFee
                    });

                    billingItemDtoList.Add(new InvoiceItem()
                    {
                        InvoiceId = id,
                        ItemID = 1,
                        itemInvoiceStatus = ItemInvoiceStatus.BILLED,
                        billingItemsType = BillingItemsType.Consultant,
                        qty = 1,
                        price = opd.ConsultantFee,
                        Discount = 0,
                        Total = opd.ConsultantFee
                    });


                }
                return billingItemDtoList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region Not Applicable
        private List<OPDItem> GetOPDItems(int id, ItemInvoiceStatus invoiceStatus)
        {
            List<OPDItem> opdItemlist = new List<OPDItem>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    opdItemlist = new OPDService().GetOPDItemsByInvoiceStatus(id, invoiceStatus);


                }
                catch (Exception ex) { }
            }
            return opdItemlist;
        }
        private List<OPDInvestigation> GetOPDInvestigation(int id, ItemInvoiceStatus invoiceStatus)
        {
            List<OPDInvestigation> opdInvestigationlist = new List<OPDInvestigation>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    opdInvestigationlist = new OPDService().GetOPDInvestigationByInvoiceStatus(id, invoiceStatus);

                }
                catch (Exception ex) { }
            }
            return opdInvestigationlist;
        }
        #endregion


        public bool checkHospitalFeeRemoved(int invoicedID)
        {
            bool isRemoved = false;
            InvoiceItem invoiceItem = new InvoiceItem();
            invoiceItem.InvoiceId = invoicedID;
            invoiceItem.billingItemsType = BillingItemsType.Hospital;
            invoiceItem.ItemID = 2;
            InvoiceItem invoiceItem2 = new InvoiceItem();
            invoiceItem2 = new CashierService().GetInvoiceItemByItemIdAndBillingItemTypeAndInvoiceIDAndInvoiceStatus(invoiceItem);
            if (invoiceItem2.itemInvoiceStatus == ItemInvoiceStatus.Remove)
            {
                isRemoved = true;
            }
            return isRemoved;
        }

        public bool checkConsultantFeeRemoved(int invoicedID)
        {
            bool isRemoved = false;
            InvoiceItem invoiceItem = new InvoiceItem();
            invoiceItem.InvoiceId = invoicedID;
            invoiceItem.billingItemsType = BillingItemsType.Consultant;
            invoiceItem.ItemID = 1;
            InvoiceItem invoiceItem2 = new InvoiceItem();
            invoiceItem2 = new CashierService().GetInvoiceItemByItemIdAndBillingItemTypeAndInvoiceIDAndInvoiceStatus(invoiceItem);
            if (invoiceItem2.itemInvoiceStatus == ItemInvoiceStatus.Remove)
            {
                isRemoved = true;
            }
            return isRemoved;
        }
        private List<OPDDrugus> GetOPDDrugus(int id, ItemInvoiceStatus invoiceStatus)
        {
            List<OPDDrugus> oPDDrugus = new List<OPDDrugus>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    oPDDrugus = new OPDService().GetOPDDrugusByInvoiceStatus(id, invoiceStatus);

                }
                catch (Exception ex) { }
            }
            return oPDDrugus;
        }

        public ActionResult AddSale()
        {
            using (var httpClient = new HttpClient())
            {
                var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];

                Invoice invoice = new Invoice();
                List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
                Payment payments = new Payment();

                if (_CashierDto.cash < _CashierDto.total)
                {
                    return RedirectToAction("Index", new { PreID = _CashierDto.PreID });
                }

                try
                {
                    if (_CashierDto.totalDueAmount <= 0)
                    {


                        bool isNightShift = false;
                        User user = new User();
                        user = GetUserById(Convert.ToInt32(userIdCookie));
                        if (user != null)
                        {
                            if (user.userRole == UserRole.OPDNURSE)
                            {
                                if ((HospitalMgrSystem.Model.Enums.InvoiceType)_CashierDto.invoiceType == InvoiceType.OPD)
                                {
                                    OPD opdForSession = new OPD();
                                    opdForSession = new OPDService().GetAllOPDByID(_CashierDto.sufID);
                                    if (opdForSession != null)
                                    {
                                        if (opdForSession.nightShiftSession.shift == Shift.NIGHT_SHIFT)
                                        {
                                            isNightShift = true;
                                        }
                                    }
                                }
                            }

                        }
                        invoice.Id = _CashierDto.invoiceID;
                        invoice.CustomerID = _CashierDto.customerID;
                        invoice.CustomerName = _CashierDto.customerName;
                        invoice.InvoiceType = (HospitalMgrSystem.Model.Enums.InvoiceType)_CashierDto.invoiceType;
                        invoice.paymentStatus = PaymentStatus.NOT_PAID;
                        invoice.Status = InvoiceStatus.New;
                        invoice.CreateUser = Convert.ToInt32(userIdCookie);
                        invoice.ModifiedUser = Convert.ToInt32(userIdCookie);
                        invoice.CreateDate = DateTime.Now;
                        invoice.ModifiedDate = DateTime.Now;
                        invoice.ServiceID = _CashierDto.sufID;
                        Invoice resInvoice = new CashierService().AddInvoice(invoice);
                        if (resInvoice != null && resInvoice.paymentStatus != PaymentStatus.PAID)
                        {
                            if (resInvoice.InvoiceType == InvoiceType.OPD)
                            {
                                _CashierDto.PreID = "OPD" + resInvoice.ServiceID;
                            }
                            if (resInvoice.InvoiceType == InvoiceType.ADM)
                            {
                                _CashierDto.PreID = "ADM" + resInvoice.ServiceID;
                            }
                            if (resInvoice.InvoiceType == InvoiceType.CHE)
                            {
                                _CashierDto.PreID = "CHE" + resInvoice.ServiceID;
                            }
                            List<CashierSession> CashierSessionList = new List<CashierSession>();
                            CashierSessionList = GetActiveCashierSession(Convert.ToInt32(userIdCookie));
                            // _CashierDto.invoice = new CashierService().AddInvoiceItems(invoiceItems);
                            payments.InvoiceID = resInvoice.Id;
                            payments.CashAmount = _CashierDto.cash;
                            payments.CreditAmount = _CashierDto.credit;
                            payments.DdebitAmount = _CashierDto.debit;
                            payments.ChequeAmount = _CashierDto.cheque;
                            payments.GiftCardAmount = _CashierDto.giftCard;
                            payments.CreateUser = Convert.ToInt32(userIdCookie);
                            payments.ModifiedUser = Convert.ToInt32(userIdCookie);
                            payments.CreateDate = DateTime.Now;
                            payments.ModifiedDate = DateTime.Now;
                            payments.CashierStatus = CashierStatus.CashierIn;
                            payments.BillingType = BillingType.CASHIER;
                            payments.sessionID = CashierSessionList[0].Id;
                            Payment resPayment = new CashierService().AddPayments(payments);
                            if (_CashierDto.totalDueAmount <= 0)
                            {
                                if (_CashierDto.totalDueAmount < 0)
                                {
                                    Payment paymentsOut = new Payment();
                                    paymentsOut.InvoiceID = resInvoice.Id;
                                    paymentsOut.CashAmount = _CashierDto.totalDueAmount;
                                    paymentsOut.CreditAmount = 0;
                                    paymentsOut.DdebitAmount = 0;
                                    paymentsOut.ChequeAmount = 0;
                                    paymentsOut.GiftCardAmount = 0;
                                    paymentsOut.CreateUser = Convert.ToInt32(userIdCookie);
                                    paymentsOut.ModifiedUser = Convert.ToInt32(userIdCookie);
                                    paymentsOut.CreateDate = DateTime.Now;
                                    paymentsOut.ModifiedDate = DateTime.Now;
                                    paymentsOut.CashierStatus = CashierStatus.CashierOut;
                                    paymentsOut.BillingType = BillingType.BALENCE;
                                    paymentsOut.sessionID = CashierSessionList[0].Id;
                                    Payment resPaymentOut = new CashierService().AddPayments(paymentsOut);
                                }
                                //update payment status on OPD
                                if (resInvoice.InvoiceType == InvoiceType.OPD)
                                {
                                    OPD updateOPD = new OPD();
                                    updateOPD.Id = _CashierDto.sufID;
                                    updateOPD.ModifiedUser = Convert.ToInt32(userIdCookie);
                                    if (isNightShift)
                                    {
                                        updateOPD.paymentStatus = PaymentStatus.OPD;
                                    }
                                    else
                                    {
                                        updateOPD.paymentStatus = PaymentStatus.PAID;
                                    }

                                    OPD upOpd = new OPDService().UpdatePaidStatus(updateOPD);
                                }

                                if (resInvoice.InvoiceType == InvoiceType.CHE)
                                {
                                    OPD updateOPD = new OPD();
                                    updateOPD.Id = _CashierDto.sufID;
                                    updateOPD.ModifiedUser = Convert.ToInt32(userIdCookie);
                                    updateOPD.paymentStatus = PaymentStatus.PAID;
                                    OPD upOpd = new OPDService().UpdatePaidStatus(updateOPD);
                                }
                                if (isNightShift)
                                {
                                    resInvoice.paymentStatus = PaymentStatus.OPD;
                                }
                                else
                                {
                                    resInvoice.paymentStatus = PaymentStatus.PAID;
                                }

                                Invoice upInvoice = new CashierService().UpdatePaidStatus(resInvoice);


                            }
                            else
                            {
                                resInvoice.paymentStatus = PaymentStatus.PARTIAL_PAID;
                                Invoice upInvoice = new CashierService().UpdatePaidStatus(resInvoice);

                            }
                        }
                        invoiceItems = GetInvoiceItemsAlldetails(resInvoice.Id, (HospitalMgrSystem.Model.Enums.InvoiceType)resInvoice.InvoiceType, _CashierDto.sufID);
                        // string partialViewHtml = await this.RenderViewAsync("_PartialViewInvoice", _CashierDto, true);
                        _CashierDto = GetCashierAlldetails(_CashierDto.PreID);
                        _CashierDto.InvoiceItemList = invoiceItems;

                        InvoiceItem hospitalItem = new InvoiceItem();
                        hospitalItem.itemInvoiceStatus = ItemInvoiceStatus.BILLED;
                        hospitalItem.billingItemsType = BillingItemsType.Hospital;
                        hospitalItem.ItemID = 2;
                        hospitalItem.Discount = 0;
                        hospitalItem.price = _CashierDto.hospitalFee;
                        hospitalItem.qty = 1;
                        hospitalItem.Total = _CashierDto.hospitalFee;
                        hospitalItem.InvoiceId = _CashierDto.invoiceID;
                        _CashierDto.InvoiceItemList.Add(hospitalItem);


                        InvoiceItem consaltantItem = new InvoiceItem();
                        consaltantItem.itemInvoiceStatus = ItemInvoiceStatus.BILLED;
                        consaltantItem.billingItemsType = BillingItemsType.Consultant;
                        consaltantItem.ItemID = 1;
                        consaltantItem.Discount = 0;
                        consaltantItem.price = _CashierDto.consaltantFee;
                        consaltantItem.qty = 1;
                        consaltantItem.Total = _CashierDto.consaltantFee;
                        consaltantItem.InvoiceId = _CashierDto.invoiceID;
                        _CashierDto.InvoiceItemList.Add(consaltantItem);

                        _CashierDto.cash = payments.CashAmount;
                        _CashierDto.credit = payments.CreditAmount;
                        _CashierDto.debit = payments.DdebitAmount;
                        _CashierDto.cheque = payments.ChequeAmount;
                        _CashierDto.giftCard = payments.GiftCardAmount;

                        Invoice InvoiceItem = new CashierService().AddInvoiceItems(invoiceItems, Convert.ToInt32(userIdCookie));
                        if (resInvoice.InvoiceType == InvoiceType.OPD)
                        {
                            new OPDService().UpdateOPDDrugInvoiceStatus(invoiceItems);
                        }
                        if (resInvoice.InvoiceType == InvoiceType.CHE)
                        {
                            new OPDService().UpdateOPDDrugInvoiceStatus(invoiceItems);
                        }


                        //PrintRecept();
                        //var pdfContent = new ViewAsPdf("_PartialViewInvoice", _CashierDto)
                        //{
                        //    // Set PDF options
                        //    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                        //    FileName = "Invoice.pdf" 
                        //};

                        CashierDto cashierDtoToPrint = new CashierDto();
                        cashierDtoToPrint.PreID = _CashierDto.PreID;

                        cashierDtoToPrint = GetCashierAlldetails(cashierDtoToPrint.PreID);
                        cashierDtoToPrint.cashierRemoveBillingItemDtoList = GetCashierAllRemoveddetails(cashierDtoToPrint.PreID);

                        return RedirectToAction("Index", new { PreID = cashierDtoToPrint.PreID });
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }
        public IActionResult RemoveItem(int Id, InvoiceType InvoiceType, BillingItemsType BillingItemsType, string PreID, int InvoiceID)
        {
            using (var httpClient = new HttpClient())
            {
                CashierDto cashierDto = new CashierDto();
                try
                {
                    var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];

                    if (InvoiceType == InvoiceType.OPD || InvoiceType == InvoiceType.CHE)
                    {
                        _CashierDto.PreID = PreID;
                        InvoiceItem removeInvoiceItem = new InvoiceItem();
                        removeInvoiceItem.InvoiceId = InvoiceID;
                        removeInvoiceItem.billingItemsType = BillingItemsType;
                        removeInvoiceItem.ItemID = Id;
                        removeInvoiceItem.ModifiedUser = Convert.ToInt32(userIdCookie);
                        removeInvoiceItem.ModifiedDate = DateTime.Now;

                        if (BillingItemsType == BillingItemsType.Drugs)
                        {
                            OPDService oPDService = new OPDService();
                            oPDService.RemoveOPDDrugus(Id, Convert.ToInt32(userIdCookie));
                            CashierDto cashierData = GetCashierAlldetails(PreID);
                            oPDService.UpdatePaymentStatus(Id, cashierData.subtotal, InvoiceType);
                        }

                        if (BillingItemsType == BillingItemsType.Hospital)
                        {
                            OPDService oPDService = new OPDService();
                            CashierDto cashierData = GetCashierAlldetails(PreID);
                            oPDService.UpdatePaymentStatusForHospitalAndConsaltantFee(cashierData.invoiceID, cashierData.opd.Id);
                        }
                        if (BillingItemsType == BillingItemsType.Consultant)
                        {
                            OPDService oPDService = new OPDService();
                            CashierDto cashierData = GetCashierAlldetails(PreID);
                            oPDService.UpdatePaymentStatusForHospitalAndConsaltantFee(cashierData.invoiceID, cashierData.opd.Id);
                        }
                        #region Not Applicable
                        if (BillingItemsType == BillingItemsType.Investigation)
                        {
                            new OPDService().RemoveOPDInvestigation(Id);
                        }
                        if (BillingItemsType == BillingItemsType.Items)
                        {
                            new OPDService().RemoveOPDItems(Id);
                        }
                        #endregion

                        new CashierService().RemoveInvoiceItems(removeInvoiceItem);
                    }
                    return RedirectToAction("Index", new { PreID = PreID });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }
        public IActionResult AddItem(int Id, HospitalMgrSystem.Model.Enums.InvoiceType InvoiceType, HospitalMgrSystem.Model.Enums.BillingItemsType BillingItemsType, string PreID)
        {
            using (var httpClient = new HttpClient())
            {
                CashierDto cashierDto = new CashierDto();
                try
                {
                    _CashierDto.PreID = PreID;
                    if (InvoiceType == HospitalMgrSystem.Model.Enums.InvoiceType.OPD)
                    {
                        if (BillingItemsType == HospitalMgrSystem.Model.Enums.BillingItemsType.Drugs)
                        {
                            new OPDService().AddOPDDrugus(Id);

                        }
                        if (BillingItemsType == HospitalMgrSystem.Model.Enums.BillingItemsType.Investigation)
                        {
                            new OPDService().AddOPDInvestigation(Id);
                        }
                        if (BillingItemsType == HospitalMgrSystem.Model.Enums.BillingItemsType.Items)
                        {
                            new OPDService().AddOPDItems(Id);
                        }

                    }
                    return RedirectToAction("Index", new { PreID = PreID });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }


        public IActionResult Refund(int Id, decimal Refund, string userPassword)
        {
            using (var httpClient = new HttpClient())
            {

                var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
                CashierDto cashierDto = new CashierDto();

                User userValidate = new User();
                userValidate.Id = Convert.ToInt32(userIdCookie);
                userValidate.Password = userPassword;
                bool isUserValid = new UserService().ValidateUserById(userValidate);


                if (!isUserValid)
                {
                    return RedirectToAction("Index", new { PreID = _CashierDto.PreID });
                }

                try
                {
                    List<CashierSession> CashierSessionList = new List<CashierSession>();
                    CashierSessionList = GetActiveCashierSession(Convert.ToInt32(userIdCookie));
                    Invoice resInvoice = new CashierService().GetInvoiceByInvoiceID(Id);
                    if (resInvoice != null && resInvoice.paymentStatus != PaymentStatus.PAID)
                    {
                        if (resInvoice.InvoiceType == InvoiceType.OPD)
                        {
                            _CashierDto.PreID = "OPD" + resInvoice.ServiceID;
                        }
                        if (resInvoice.InvoiceType == InvoiceType.ADM)
                        {
                            _CashierDto.PreID = "ADM" + resInvoice.ServiceID;
                        }
                        if (resInvoice.InvoiceType == InvoiceType.CHE)
                        {
                            _CashierDto.PreID = "CHE" + resInvoice.ServiceID;
                        }


                        if (Refund < 0)
                        {
                            Payment paymentsOut = new Payment();
                            paymentsOut.InvoiceID = resInvoice.Id;
                            paymentsOut.CashAmount = Refund;
                            paymentsOut.CreditAmount = 0;
                            paymentsOut.DdebitAmount = 0;
                            paymentsOut.ChequeAmount = 0;
                            paymentsOut.GiftCardAmount = 0;
                            paymentsOut.CreateUser = Convert.ToInt32(userIdCookie);
                            paymentsOut.ModifiedUser = Convert.ToInt32(userIdCookie);
                            paymentsOut.CreateDate = DateTime.Now;
                            paymentsOut.ModifiedDate = DateTime.Now;
                            paymentsOut.CashierStatus = CashierStatus.CashierOut;
                            paymentsOut.BillingType = BillingType.REFUND;
                            paymentsOut.sessionID = CashierSessionList[0].Id;
                            Payment resPaymentOut = new CashierService().AddPayments(paymentsOut);
                        }

                        if (resInvoice.InvoiceType == InvoiceType.OPD)
                        {
                            OPD updateOPD = new OPD();
                            updateOPD.Id = resInvoice.ServiceID;
                            updateOPD.ModifiedUser = Convert.ToInt32(userIdCookie);
                            updateOPD.paymentStatus = PaymentStatus.PAID;
                            OPD upOpd = new OPDService().UpdatePaidStatus(updateOPD);
                        }

                        if (resInvoice.InvoiceType == InvoiceType.CHE)
                        {
                            OPD updateOPD = new OPD();
                            updateOPD.Id = resInvoice.ServiceID;
                            updateOPD.ModifiedUser = Convert.ToInt32(userIdCookie);
                            updateOPD.paymentStatus = PaymentStatus.PAID;
                            OPD upOpd = new OPDService().UpdatePaidStatus(updateOPD);
                        }

                        resInvoice.paymentStatus = PaymentStatus.PAID;
                        Invoice upInvoice = new CashierService().UpdatePaidStatus(resInvoice);
                    }



                    //update payment status on OPD

                    return RedirectToAction("Index", new { PreID = _CashierDto.PreID });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public IActionResult MarkAsPaid(int Id)
        {
            using (var httpClient = new HttpClient())
            {
                var userIdCookie = HttpContext.Request.Cookies["UserIdCookie"];
                CashierDto cashierDto = new CashierDto();
                try
                {
                    List<CashierSession> CashierSessionList = new List<CashierSession>();
                    CashierSessionList = GetActiveCashierSession(Convert.ToInt32(userIdCookie));
                    Invoice resInvoice = new CashierService().GetInvoiceByInvoiceID(Id);
                    if (resInvoice != null && resInvoice.paymentStatus == PaymentStatus.OPD)
                    {
                        if (resInvoice.InvoiceType == InvoiceType.OPD)
                        {
                            _CashierDto.PreID = "OPD" + resInvoice.ServiceID;
                        }
                        if (CashierSessionList.Count > 0)
                        {
                            new CashierService().UpdateSessionIDOnPayments(resInvoice.Id, CashierSessionList[0].Id, Convert.ToInt32(userIdCookie));

                            if (resInvoice.InvoiceType == InvoiceType.OPD)
                            {
                                OPD updateOPD = new OPD();
                                updateOPD.Id = resInvoice.ServiceID;
                                updateOPD.ModifiedUser = Convert.ToInt32(userIdCookie);
                                updateOPD.paymentStatus = PaymentStatus.PAID;
                                OPD upOpd = new OPDService().UpdatePaidStatus(updateOPD);
                            }

                            resInvoice.paymentStatus = PaymentStatus.PAID;
                            Invoice upInvoice = new CashierService().UpdatePaidStatus(resInvoice);

                        }

                    }



                    //update payment status on OPD

                    return RedirectToAction("Index", new { PreID = _CashierDto.PreID });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        #region Drugs Management 
        //drugs popup openning
        public ActionResult AddDrugusToOPD(int Id)
        {
            OPDDto oPDDto = new OPDDto();
            oPDDto.Drugs = DrugsSearch();
            oPDDto.opdId = Id;
            oPDDto.OPDDrugusList = GetOPDDrugus(Id);

            return PartialView("_PartialAddDrugus", oPDDto);
        }

        public ActionResult AddOPDsToOPD(int Id)
        {
            OPDDto oPDDto = new OPDDto();
            oPDDto.consultantList = LoadConsultants();
            oPDDto.patientsList = LoadPatients();
            oPDDto.patientsList = LoadPatients();
            if (Id > 0)
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {

                        oPDDto.opd = new OPDService().GetAllOPDByID(Id);
                        return PartialView("_PartialAddOPDRegistration", oPDDto);
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            else
            {
                return PartialView("_PartialAddOPDRegistration", oPDDto);
            }

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

        private List<Patient> LoadPatients()
        {
            List<Patient> patients = new List<Patient>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    patients = new PatientService().GetAllPatientByStatus();

                }
                catch (Exception ex) { }
            }
            return patients;
        }
        private List<OPDTbDto> LoadOPD()
        {
            List<OPD> opd = new List<OPD>();
            List<OPDTbDto> oPDTbDto = new List<OPDTbDto>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    opd = new OPDService().GetAllOPDByStatus();
                    var result = opd;

                    foreach (var item in result)
                    {
                        oPDTbDto.Add(new OPDTbDto()
                        {
                            Id = item.Id,
                            roomName = item.room.Name,
                            consaltantName = item.consultant.Name,
                            FullName = item.patient.FullName,
                            MobileNumber = item.patient.MobileNumber,
                            DateTime = item.DateTime,
                            Sex = (HospitalMgrSystem.Model.Enums.SexStatus)item.patient.Sex,
                            Status = item.Status
                        });
                    }

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return oPDTbDto;
        }

        public List<Drug> DrugsSearch()
        {
            List<Drug> drugs = new List<Drug>();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    drugs = new DrugsService().GetAllDrugsByStatus();

                }
                catch (Exception ex) { }
            }
            return drugs;
        }

        private List<OPDDrugus> GetOPDDrugus(int id)
        {
            List<OPDDrugus> oPDDrugus = new List<OPDDrugus>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    oPDDrugus = new OPDService().GetOPDDrugus(id);

                }
                catch (Exception ex) { }
            }
            return oPDDrugus;
        }

        public IActionResult AddNewDrugs()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    _OPDDto.opdDrugus.opdId = _OPDDto.opdId;
                    _OPDDto.opdDrugus.Amount = _OPDDto.opdDrugus.Qty * _OPDDto.opdDrugus.Price;
                    _OPDDto.opdDrugus = new OPDService().CreateOPDDrugus(_OPDDto.opdDrugus);
                    string PreID = "OPD" + _OPDDto.opdId;
                    return RedirectToAction("Index", new { PreID = PreID });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public IActionResult DeleteOPDDrug(int Id, int OPDID)
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    new OPDService().DeleteOPDDrugus(Id);
                    string PreID = "OPD" + OPDID;
                    return RedirectToAction("Index", new { PreID = PreID });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }
        #endregion

        #region Investigation Management 
        public ActionResult AddInvestigationToOPD(int Id)
        {
            OPDDto opdDto = new OPDDto();
            opdDto.Investigations = GetInvestigations();
            opdDto.opdId = Id;
            opdDto.OPDInvestigationList = GetOPDInvestigation(Id);

            return PartialView("_PartialInvestigation", opdDto);
        }

        public List<Investigation> GetInvestigations()
        {
            List<Investigation> Investigationlist = new List<Investigation>();
            using (var httpClient = new HttpClient())
            {
                try
                {

                    Investigationlist = new InvestigationService().GetAllInvestigationByStatus();

                }
                catch (Exception ex) { }
            }
            return Investigationlist;
        }
        private List<OPDInvestigation> GetOPDInvestigation(int id)
        {
            List<OPDInvestigation> opdInvestigationlist = new List<OPDInvestigation>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    opdInvestigationlist = new OPDService().GetOPDInvestigation(id);

                }
                catch (Exception ex) { }
            }
            return opdInvestigationlist;
        }

        public IActionResult AddNewInvestigation()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    _OPDDto.opdInvestigation.opdId = _OPDDto.opdId;
                    _OPDDto.opdInvestigation.Amount = _OPDDto.opdInvestigation.Qty * _OPDDto.opdInvestigation.Price;
                    _OPDDto.opdInvestigation = new OPDService().CreateOPDInvestigation(_OPDDto.opdInvestigation);
                    string PreID = "OPD" + _OPDDto.opdId;
                    return RedirectToAction("Index", new { PreID = PreID });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public IActionResult DeleteOPDInvestigation(int Id, int OPDID)
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    new OPDService().DeleteOPDInvestigation(Id);
                    string PreID = "OPD" + OPDID;
                    return RedirectToAction("Index", new { PreID = PreID });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }
        #endregion

        #region Items Management 

        public ActionResult AddItemsToOPD(int Id)
        {
            OPDDto opdDto = new OPDDto();
            opdDto.Items = SearchItems();
            opdDto.opdId = Id;
            opdDto.OPDItemList = GetOPDItems(Id);

            return PartialView("_PartialAddItems", opdDto);
        }

        public List<HospitalMgrSystem.Model.Item> SearchItems()
        {
            List<HospitalMgrSystem.Model.Item> items = new List<HospitalMgrSystem.Model.Item>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    items = new ItemService().GetAllItemByStatus();
                }
                catch (Exception ex) { }
            }
            return items;
        }


        private List<OPDItem> GetOPDItems(int id)
        {
            List<OPDItem> opdItemlist = new List<OPDItem>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    opdItemlist = new OPDService().GetOPDItems(id);


                }
                catch (Exception ex) { }
            }
            return opdItemlist;
        }

        public IActionResult AddNewItems()
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    _OPDDto.opdItem.opdId = _OPDDto.opdId;
                    _OPDDto.opdItem.Amount = _OPDDto.opdItem.Qty * _OPDDto.opdItem.Price;
                    _OPDDto.opdItem = new OPDService().CreateOPDItems(_OPDDto.opdItem);
                    string PreID = "OPD" + _OPDDto.opdId;
                    return RedirectToAction("Index", new { PreID = PreID });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public IActionResult DeleteOPDItems(int Id, int OPDID)
        {
            using (var httpClient = new HttpClient())
            {

                try
                {
                    new OPDService().DeleteOPDItems(Id);
                    string PreID = "OPD" + OPDID;
                    return RedirectToAction("Index", new { PreID = PreID });
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }
        }

        private IActionResult print()
        {


            using (var httpClient = new HttpClient())
            {
                try
                {
                    new OPDService().printme("1");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index");
                }
            }

        }

        public ActionResult OpenQR(int Id)
        {
            OPDDto opdDto = new OPDDto();
            opdDto.Items = SearchItems();
            opdDto.opdId = Id;
            opdDto.OPDItemList = GetOPDItems(Id);

            return PartialView("_PartialQR", opdDto);
        }
        #endregion

        static int GetNumber(string input)
        {
            int index = 0;
            while (index < input.Length && char.IsLetter(input[index]))
            {
                index++;
            }

            if (index < input.Length && int.TryParse(input.Substring(index), out int number))
            {
                return number;
            }

            // If the numeric part cannot be parsed, return a default value (e.g., 0).
            return 0;
        }
        static string GetPrefix(string input)
        {
            int index = 0;
            while (index < input.Length && char.IsLetter(input[index]))
            {
                index++;
            }

            return input.Substring(0, index);
        }

        public Channeling GetChannelByID(int ID)
        {
            Channeling channeling = new Channeling();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    channeling = new ChannelingService().GetChannelByID(ID);

                }
                catch (Exception ex) { }
            }
            return channeling;
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

        private User GetUserById(int id)
        {
            User user = new User();

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

        private List<NightShiftSession> GetActiveShiftSession()
        {
            List<NightShiftSession> NightShiftSessionList = new List<NightShiftSession>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    NightShiftSessionList = new NightShiftSessionService().GetACtiveNtShiftSessions();

                }
                catch (Exception ex) { }
            }
            return NightShiftSessionList;
        }



        //PRINT RECEIPT

        public void PrintRecept()
        {
            var dt = new DataTable();
            using var report = new LocalReport();
            dt = GetItemList();

            var dt2 = new DataTable();
            dt2 = GetDetailsReceipt();

            report.DataSources.Add(new ReportDataSource("DataSet1", dt));
            report.DataSources.Add(new ReportDataSource("DataSet2", dt2));
            //report.ReportPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\rdlcDetailsReport.rdlc";
            report.ReportPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\rdlcReport.rdlc";
            PrintToPrinter(report);
        }

        public static void Export(LocalReport report, bool print = true)
        {
            string deviceInfo =
             @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>3.5in</PageWidth>
                <PageHeight>10in</PageHeight>
                <MarginTop>0in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                Print();
            }
        }

        public static void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }

        public static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        public static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        public static void DisposePrint()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }

        public DataTable GetItemList()
        {
            var dt = new DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("ItemQty");
            dt.Columns.Add("ItemPrice");

            DataRow row;
            row = dt.NewRow();
            row["ItemID"] = "No";
            row["ItemName"] = "Name";
            row["ItemQty"] = "Qty";
            row["ItemPrice"] = "Amount";
            dt.Rows.Add(row);

            int i = 0;
            decimal total = 0;
            foreach (var item in _CashierDto.cashierBillingItemDtoList)
            {
                i++;

                row = dt.NewRow();
                row["ItemID"] = i;
                row["ItemName"] = item.billingItemName + " (" + item.billingItemsTypeName + ")";
                row["ItemQty"] = (int)Math.Floor(item.qty);
                row["ItemPrice"] = item.price * (int)Math.Floor(item.qty);
                dt.Rows.Add(row);
                decimal amount = item.price * (int)Math.Floor(item.qty);
                total = total + amount;
            }

            DataRow rowLast;
            rowLast = dt.NewRow();
            rowLast["ItemID"] = "";
            rowLast["ItemName"] = "Total";
            rowLast["ItemQty"] = "";
            rowLast["ItemPrice"] = total;
            dt.Rows.Add(rowLast);

            decimal paidamount = _CashierDto.cash + _CashierDto.credit + _CashierDto.debit + _CashierDto.cheque + _CashierDto.giftCard;
            DataRow rowCash;
            rowCash = dt.NewRow();
            rowCash["ItemID"] = "";
            rowCash["ItemName"] = "Cash";
            rowCash["ItemQty"] = "";
            rowCash["ItemPrice"] = paidamount;
            dt.Rows.Add(rowCash);

            decimal balance = total - paidamount;
            DataRow rowBalance;
            rowBalance = dt.NewRow();
            rowBalance["ItemID"] = "";
            rowBalance["ItemName"] = "Balance";
            rowBalance["ItemQty"] = "";
            rowBalance["ItemPrice"] = balance;
            dt.Rows.Add(rowBalance);

            //DataRow row;
            //for (int i = 1; i < 10; i++)
            //{
            //    row = dt.NewRow();
            //    row["ItemID"] = i;
            //    row["ItemName"] = "Content template is filled with easy-to-use placeholders for text " + i.ToString();
            //    row["ItemQty"] = 1;
            //    row["ItemPrice"] = 100 * i;

            //    dt.Rows.Add(row);
            //}
            return dt;
        }

        public DataTable GetDetailsReceipt()
        {
            var dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("drName");
            dt.Columns.Add("billNo");
            dt.Columns.Add("date");

            DataRow row;

            row = dt.NewRow();
            row["name"] = "Name: " + _CashierDto.customerName;
            row["drName"] = "Doctor: " + _CashierDto.customerName;
            row["billNo"] = "Bill No: INV23" + _CashierDto.invoiceID;
            row["date"] = "Date :" + DateTime.Now;

            dt.Rows.Add(row);

            return dt;
        }
        public static void PrintToPrinter(LocalReport report)
        {
            Export(report);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
