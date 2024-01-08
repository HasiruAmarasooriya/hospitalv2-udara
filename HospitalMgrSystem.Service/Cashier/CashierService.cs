using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Cashier
{
    public class CashierService : ICashierService
    {
        public Model.Invoice AddInvoice(Model.Invoice invoice)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (invoice.Id == 0)
                    {
                        dbContext.Invoices.Add(invoice);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        HospitalMgrSystem.Model.Invoice result = (from p in dbContext.Invoices where p.Id == invoice.Id select p).SingleOrDefault();
                        result.paymentStatus = invoice.paymentStatus; // Update only the "Price" column                   
                        result.ModifiedDate = DateTime.Now; // Optional, set the ModifiedDate if needed
                    }
                    return dbContext.Invoices.Find(invoice.Id);
                }
                catch (Exception ex)
                {
                    return null;
                }


            }
        }
        public Model.Invoice UpdatePaidStatus(Model.Invoice invoice)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (invoice.Id != 0)
                    {
                        HospitalMgrSystem.Model.Invoice result = (from p in dbContext.Invoices where p.Id == invoice.Id select p).SingleOrDefault();
                        result.paymentStatus = invoice.paymentStatus; // Update only the "Price" column
                        result.ModifiedDate = DateTime.Now; // Optional, set the ModifiedDate if needed

                        dbContext.SaveChanges();
                    }
                    return dbContext.Invoices.Find(invoice.Id);
                }
                catch (Exception ex)
                {
                    return null;
                }


            }
        }
        public Model.Invoice AddInvoiceItems(List<Model.InvoiceItem> invoiceItems)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Invoice invoice = new Invoice();
                try
                    {
                       
                        invoice.Id = invoiceItems[0].InvoiceId;
                        foreach (var item in invoiceItems)
                        {
                            item.InvoiceId = invoice.Id;
                            HospitalMgrSystem.Model.InvoiceItem result = (from p in dbContext.InvoiceItems where p.InvoiceId == invoice.Id  && p.billingItemsType== item.billingItemsType && p.ItemID == item.ItemID select p).SingleOrDefault();
                            if (result != null)
                            {
                                result.price = item.price; // Update only the "Price" column

                                result.qty = item.qty; // Optional, set the ModifiedDate if needed
                                result.Discount = item.Discount; // Optional, set the ModifiedDate if needed
                                result.Total = item.Total; // Optional, set the ModifiedDate if needed
                                result.itemInvoiceStatus = item.itemInvoiceStatus; // Optional, set the ModifiedDate if needed
                                dbContext.SaveChanges();
                            }                           
                            else
                            {
                                dbContext.InvoiceItems.Add(item);
                                dbContext.SaveChanges();
                            }
                        }
                  
                    return dbContext.Invoices.Find(invoice.Id);

                }
                catch (Exception ex)
                {
                    return null;
                }



            }
        }

        public Model.Invoice RemoveInvoiceItems(Model.InvoiceItem invoiceItems)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Invoice invoice = new Invoice();
                try
                {

                    invoice.Id = invoiceItems.InvoiceId;
                    HospitalMgrSystem.Model.InvoiceItem result = (from p in dbContext.InvoiceItems where p.InvoiceId == invoiceItems.InvoiceId && p.billingItemsType == invoiceItems.billingItemsType && p.ItemID == invoiceItems.ItemID select p).SingleOrDefault();
                    if (result != null)
                    {
        
                        result.itemInvoiceStatus = ItemInvoiceStatus.Remove; // Optional, set the ModifiedDate if needed
                        dbContext.SaveChanges();
                    }

                    return dbContext.Invoices.Find(invoice.Id);

                }
                catch (Exception ex)
                {
                    return null;
                }



            }
        }

        public Model.Payment AddPayments(Payment payment)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (payment.Id == 0)
                    {
                        dbContext.Payments.Add(payment);
                        dbContext.SaveChanges();

                    }
                    return dbContext.Payments.Find(payment.Id);
                }
                catch (Exception ex)
                {
                    return null;
                }

                
            }           
        }

        public Model.Invoice GetInvoiceByServiceIDAndInvoiceType(int serviceID, InvoiceType invoiceType)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                Model.Invoice result = (from p in dbContext.Invoices where p.ServiceID == serviceID && p.InvoiceType==invoiceType select p).SingleOrDefault();
                return result;
            }

        }

        public List<Model.Payment> GetAllPaymentsByInvoiceID(int invoiceID)
        {

            List<Model.Payment> mtList = new List<Model.Payment>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.Payments.Where(c => c.InvoiceID == invoiceID).ToList<Model.Payment>();

            }
            return mtList;
        }


        public List<Model.InvoiceItem> GetInvoiceItemByInvoicedID(int invoiceID)
        {

            List<Model.InvoiceItem> mtList = new List<Model.InvoiceItem>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                mtList = dbContext.InvoiceItems.Where(c => c.InvoiceId == invoiceID).ToList<Model.InvoiceItem>();

            }
            return mtList;
        }
    }
}
