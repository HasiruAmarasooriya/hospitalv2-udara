using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.ClaimBill
{
    public interface IClaimBill
    {
        public Model.ClaimBill CreateClaimBill(Model.ClaimBill claimBill);
        public List<Model.ClaimBill> GetClaimBillByPatientId(int id);
        public Model.ClaimBill GetClaimBillById(int id);
        public Model.ClaimBill DeleteClaimBill(int id);
    }
}
