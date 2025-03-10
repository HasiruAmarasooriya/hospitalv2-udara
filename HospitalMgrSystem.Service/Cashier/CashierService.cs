using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace HospitalMgrSystem.Service.Cashier
{
    public class CashierService : ICashierService
    {
	    public Invoice? GetInvoiceDataByServiceId(int id)
	    {
		    using var dbContext = new DataAccess.HospitalDBContext();

		    var result = (from p in dbContext.Invoices where p.ServiceID == id select p).SingleOrDefault();

		    return result;
	    }

	    public Invoice? GetInvoiceDataByServiceIdAndWithoutOtherIncome(int id)
	    {
		    using var dbContext = new DataAccess.HospitalDBContext();

		    var result = (from p in dbContext.Invoices where p.ServiceID == id && (p.InvoiceType == InvoiceType.OPD || p.InvoiceType == InvoiceType.CHE|| p.InvoiceType ==InvoiceType.ADM) select p).SingleOrDefault();

		    return result;
	    }

	    public int IncrementThePrintCountUsingServiceId(int serviceId)
	    {
		    using var dbContext = new DataAccess.HospitalDBContext();

		    var result = (from p in dbContext.Invoices where p.ServiceID == serviceId && (p.InvoiceType == InvoiceType.OPD || p.InvoiceType == InvoiceType.CHE || p.InvoiceType == InvoiceType.ADM) select p).SingleOrDefault();

		    if (result == null) return 999;

		    result.PrintCount++;
		    dbContext.SaveChanges();

		    return (from p in dbContext.Invoices where p.ServiceID == serviceId && (p.InvoiceType == InvoiceType.OPD || p.InvoiceType == InvoiceType.CHE || p.InvoiceType == InvoiceType.ADM) select p).SingleOrDefault()!.PrintCount;
	    }

		public Invoice AddInvoice(Invoice invoice)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
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

        public InvoiceItem AddSingleInvoiceItem(InvoiceItem invoiceItem)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                try
                {
                    if (invoiceItem.Id == 0)
                    {
                        dbContext.InvoiceItems.Add(invoiceItem);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        HospitalMgrSystem.Model.InvoiceItem result = (from p in dbContext.InvoiceItems where p.Id == invoiceItem.Id && p.InvoiceId ==invoiceItem.InvoiceId select p).SingleOrDefault();
                        result.price = invoiceItem.price; // Update only the "Price" column
                        result.ModifiedDate = invoiceItem.ModifiedDate; // Optional, set the ModifiedDate if needed
                        result.ModifiedUser = invoiceItem.ModifiedUser; // Optional, set the ModifiedDate if needed
                        result.qty = invoiceItem.qty; // Optional, set the ModifiedDate if needed
                        result.Discount = invoiceItem.Discount; // Optional, set the ModifiedDate if needed
                        result.Total = invoiceItem.Total; // Optional, set the ModifiedDate if needed
                        result.itemInvoiceStatus = invoiceItem.itemInvoiceStatus; // Optional, set the ModifiedDate if needed
                        // This will affect when the discount is applied
                        result.DiscountPercentage = invoiceItem.DiscountPercentage;
                        result.PrevPrice = invoiceItem.PrevPrice;
                        dbContext.SaveChanges();
                    }

                    
                    

                    return dbContext.InvoiceItems.Find(invoiceItem.Id);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }


        public Invoice AddInvoiceItems(List<InvoiceItem> invoiceItems, int userID)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
            {
                var invoice = new Invoice();
                try
                {

                    invoice.Id = invoiceItems[0].InvoiceId;
                    foreach (var item in invoiceItems)
                    {
                        item.InvoiceId = invoice.Id;
                        item.CreateDate = DateTime.Now;
                        item.ModifiedDate = DateTime.Now;
                        item.CreateUser = userID;
                        item.ModifiedUser = userID;

                        var result = (from p in dbContext.InvoiceItems where p.InvoiceId == invoice.Id && p.billingItemsType == item.billingItemsType && p.ItemID == item.ItemID select p).SingleOrDefault();
                        if (result != null)
                        {
                            result.price = item.price; // Update only the "Price" column
                            result.ModifiedDate = item.ModifiedDate;
                            result.ModifiedUser = item.ModifiedUser;
                            result.qty = item.qty; // Optional, set the ModifiedDate if needed
                            result.Discount = item.Discount; // Optional, set the ModifiedDate if needed
                            result.Total = item.Total; // Optional, set the ModifiedDate if needed
                            result.itemInvoiceStatus = item.itemInvoiceStatus; // Optional, set the ModifiedDate if needed

                            // This will affect when the discount is applied
                            result.DiscountPercentage = item.DiscountPercentage;
                            result.PrevPrice = item.PrevPrice;

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
                        result.ModifiedUser = invoiceItems.ModifiedUser;
                        result.ModifiedDate = invoiceItems.ModifiedDate;
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

        public Payment AddPayments(Payment payment)
        {
            using (DataAccess.HospitalDBContext dbContext = new DataAccess.HospitalDBContext())
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
                Model.Invoice result = (from p in dbContext.Invoices where p.ServiceID == serviceID && p.InvoiceType == invoiceType select p).SingleOrDefault();
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
        public bool UpdateSessionIDOnPayments(int invoiceId, int NewSessionID, int ModifiedUser)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                try
                {
                    if (invoiceId != null)
                    {
                        List<Model.Payment> mtList = new List<Model.Payment>();

                        mtList = dbContext.Payments.Where(c => c.InvoiceID == invoiceId).ToList<Model.Payment>();
                        foreach (var item in mtList)
                        {

                            HospitalMgrSystem.Model.Payment result = (from p in dbContext.Payments where p.Id == item.Id select p).SingleOrDefault();
                            if (result != null)
                            {
                                result.sessionID = NewSessionID;
                                result.ModifiedDate = DateTime.Now;
                                result.ModifiedUser = ModifiedUser;
                                dbContext.SaveChanges();
                            }

                        }
                    }

                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }


            }
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

        public Invoice GetInvoiceByInvoiceID(int invoiceID)
        {

            Invoice invoice = new Invoice();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                HospitalMgrSystem.Model.Invoice result = (from p in dbContext.Invoices where p.Id == invoiceID select p).SingleOrDefault();
                invoice = result;

            }
            return invoice;
        }


        public Model.InvoiceItem GetInvoiceItemByItemIdAndBillingItemTypeAndInvoiceIDAndInvoiceStatus(Model.InvoiceItem invoiceItems)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {

                InvoiceItem invoiceItem = new InvoiceItem();
                try
                {

                    HospitalMgrSystem.Model.InvoiceItem result = (from p in dbContext.InvoiceItems where p.InvoiceId == invoiceItems.InvoiceId && p.billingItemsType == invoiceItems.billingItemsType && p.ItemID == invoiceItems.ItemID select p).SingleOrDefault();
                    if (result != null)
                    {
                        invoiceItem = result;

                    }

                    return invoiceItem;

                }
                catch (Exception ex)
                {
                    return null;
                }



            }
        }
        public Model.InvoiceItem GetADMInvoiceItemByItemId(Model.InvoiceItem invoiceItems)
        {
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {

                InvoiceItem invoiceItem = new InvoiceItem();
                try
                {

                    HospitalMgrSystem.Model.InvoiceItem result = (from p in dbContext.InvoiceItems where p.InvoiceId == invoiceItems.InvoiceId && p.billingItemsType == invoiceItems.billingItemsType && p.ItemID == invoiceItems.ItemID select p).SingleOrDefault();
                    if (result != null)
                    {
                        invoiceItem = result;

                    }

                    return invoiceItem;

                }
                catch (Exception ex)
                {
                    return null;
                }



            }
        }
        public decimal getDiscountedPrice(int itemId)
        {
            using var dbContext = new DataAccess.HospitalDBContext();
            var result = (from p in dbContext.InvoiceItems where p.ItemID == itemId select p).SingleOrDefault();
            
            return result != null ? result.Discount : 0;
        }

		public bool CheckIsDiscountAvailableForThatItem(int itemId)
		{
			using var dbContext = new DataAccess.HospitalDBContext();

			// Assume OPDDrugus has a related entity you want to include, e.g., "Category"
			var result = dbContext.OPDDrugus
				.Include(p => p.Drug)  // Replace 'Category' with the actual navigation property
				.SingleOrDefault(p => p.Id == itemId);

			return result != null && result.Drug!.IsDiscountAvailable;
		}

	}
}
