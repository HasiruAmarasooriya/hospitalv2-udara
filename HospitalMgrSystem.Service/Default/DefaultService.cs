using HospitalMgrSystem.DataAccess;
using HospitalMgrSystem.Model;
using HospitalMgrSystem.Model.Enums;

namespace HospitalMgrSystem.Service.Default
{
    public class DefaultService : IDefaultService
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

        public Scan GetScanChannelingFee(int ID)
        {
            Model.Scan scanFee = new Model.Scan();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                scanFee = dbContext.ChannelingItems.First(o => o.Id == ID);

            }
            return scanFee;
        }


        public List<Scan> GetAllScanChannelingFee(int tag)
        {
            List<Model.Scan> scanFeeList = new List<Model.Scan>();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                scanFeeList = dbContext.ChannelingItems.Where(o => o.Tag1 == tag).ToList();

            }
            return scanFeeList;
        }

        public Drug GetExerciseBookFee()
        {
            Model.Drug exbFee = new Model.Drug();
            using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
            {
                exbFee = dbContext.Drugs.First(o => o.Id == 564);

            }
            return exbFee;
        }


    }
}
