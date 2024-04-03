using HospitalMgrSystem.DataAccess;
using HospitalMgrSystem.Model;
using HospitalMgrSystem.Service.ChannelingSchedule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.ClaimBill
{
    public class ClaimBillService : IClaimBill
    {
        public Model.ClaimBill CreateClaimBill(Model.ClaimBill claimBill)
        {
            HospitalDBContext dBContext = new HospitalDBContext();

            try
            {
                if (claimBill != null && claimBill.Id == 0)
                {
                    dBContext.ClaimBills.Add(claimBill);
                    dBContext.SaveChanges();
                } else
                {
                    Model.ClaimBill claimBillToUpdate = dBContext.ClaimBills.Where(x => x.Id == claimBill.Id).SingleOrDefault();

                    claimBillToUpdate.PatientID = claimBill.PatientID;
                    claimBillToUpdate.ConsultantId = claimBill.ConsultantId;
                    claimBillToUpdate.RefNo = claimBill.RefNo;
                    claimBillToUpdate.SubTotal = claimBill.SubTotal;
                    claimBillToUpdate.Discount = claimBill.Discount;
                    claimBillToUpdate.TotalAmount = claimBill.TotalAmount;
                    claimBillToUpdate.CashAmount = claimBill.CashAmount;
                    claimBillToUpdate.Balance = claimBill.Balance;
                    claimBillToUpdate.CreateDate = claimBill.CreateDate;
                    claimBillToUpdate.ModifiedDate = DateTime.Now;

                    dBContext.SaveChanges();
                }

                return dBContext.ClaimBills
                    .Include(x => x.Patient)
                    .Include(x => x.Consultant)
                    .Where(x => x.Id == claimBill.Id)
                    .SingleOrDefault();
                    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Model.ClaimBill DeleteClaimBill(int id)
        {
            throw new NotImplementedException();
        }

        public Model.ClaimBill GetClaimBillById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Model.ClaimBill> GetClaimBillByPatientId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
