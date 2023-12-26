using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalMgrSystem.Service.Room
{
    public class RoomService : IRoomService
    {

        public List<Model.Room> GetRooms()
        {
            List<Model.Room> objRoom = new List<Model.Room>();

            try
            {
                using (HospitalMgrSystem.DataAccess.HospitalDBContext dbContext = new HospitalMgrSystem.DataAccess.HospitalDBContext())
                {

                    objRoom = dbContext.Rooms.ToList();
                }
            }
            catch (Exception ex)
            { }
            return objRoom;
        }
    }
}
