using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Default
{
    public interface IDefaultService
    {
        public Model.Scan GetScanChannelingFee(int ID);
    }
}
