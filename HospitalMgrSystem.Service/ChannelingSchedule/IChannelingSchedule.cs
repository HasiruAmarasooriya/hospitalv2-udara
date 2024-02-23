using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.ChannelingSchedule
{
    public interface IChannelingSchedule
    {
        public HospitalMgrSystem.Model.ChannelingSchedule CreateChannelingSchedule(HospitalMgrSystem.Model.ChannelingSchedule channelingSchedule);
        public List<Model.ChannelingSchedule> SheduleGetByStatus();
        public Model.ChannelingSchedule SheduleGetByConsultantIdandDate(int id, string date);
        public List<Model.ChannelingSchedule> SheduleGetByConsultantId(int id);

        public Model.ChannelingSchedule SheduleGetById(int id);
        public HospitalMgrSystem.Model.ChannelingSchedule DeleteChannelingShedule(int id);
        public Model.Scan GetChannelingItemById(int id);
    }
}
