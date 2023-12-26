using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Channeling
{
    public interface IChannelingService
    {
        public HospitalMgrSystem.Model.Channeling CreateChanneling(HospitalMgrSystem.Model.Channeling channeling);
        public List<Model.Channeling> GetAllChannelingByStatus();
        //public HospitalMgrSystem.Model.Channeling ChannelingGetById(int Id);

        public List<Model.Channeling> ChannelingGetBySheduleId(int id);
        public HospitalMgrSystem.Model.Channeling DeleteChanneling(int id);
    }
}
