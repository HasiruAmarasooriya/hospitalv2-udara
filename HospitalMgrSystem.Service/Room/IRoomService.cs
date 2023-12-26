using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Room
{
    public interface IRoomService
    {
        public List<Model.Room> GetRooms();
    }
}
