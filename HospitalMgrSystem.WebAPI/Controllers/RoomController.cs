using HospitalMgrSystem.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMgrSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly HospitalMgrSystem.Service.Room.IRoomService _roomService;

        public RoomController(HospitalMgrSystem.Service.Room.IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("GetRooms")]
        public ActionResult<Room> UserLoginDetails()
        {
            var objRomms = _roomService.GetRooms();
            if (objRomms == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(objRomms);
            }
        }
    }
}
