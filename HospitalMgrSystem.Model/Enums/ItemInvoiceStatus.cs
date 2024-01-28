using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Model.Enums
{
    public enum ItemInvoiceStatus
    {
      
        Add, // Invoiced but not paid
        Remove,// Refund after the payment
        BILLED, // paid for that item
        IN, // intra cashier-in transaction
        OUT // intra cashier-Out transaction
    }
}
