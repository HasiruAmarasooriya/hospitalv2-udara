using HospitalMgrSystem.Model;

namespace HospitalMgrSystem.Service.Default
{
    public interface IDefaultService
    {
        public Scan GetScanChannelingFee(int ID);

        public Discount getDiscount();
    }
}
