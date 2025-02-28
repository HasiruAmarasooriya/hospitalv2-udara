using HospitalMgrSystem.DataAccess;

namespace HospitalMgrSystem.Service.Room
{
    public class RoomService : IRoomService
    {
        public List<Model.Room> GetRooms()
        {
            var objRoom = new List<Model.Room>();

            try
            {
                using var dbContext = new HospitalDBContext();
                objRoom = dbContext.Rooms.ToList();
            }
            catch (Exception ex)
            {
                // ignored
            }

            return objRoom;
        }
    }
}