using HospitalMgrSystem.DataAccess;
using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Default
{
    public class DefaultService
    {
        public Decimal GetDefailtHospitalPrice()
        {
            Model.HospitalFee hospitalFee = new Model.HospitalFee();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                hospitalFee = dbContext.HospitalFee.First(o => o.IsDefault == DefaultStatus.IS_DEFAULT);

            }
            return hospitalFee.Price;
        }

        public Decimal GetDefailtConsaltantPrice()
        {
            Model.OPDConsultantFee consultantFee = new Model.OPDConsultantFee();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                consultantFee = dbContext.OPDConsultantFee.First(o => o.IsDefault == DefaultStatus.IS_DEFAULT);

            }
            return consultantFee.Price;
        }

        public Shift GetDefailtShiftStatus()
        {
            NightShift nightShift = new NightShift();
            using (HospitalDBContext dbContext = new HospitalDBContext())
            {
                nightShift = dbContext.NightShifts.First(o => o.Id == 1);

            }
            return nightShift.IsNightShift;
        }
    }
}
