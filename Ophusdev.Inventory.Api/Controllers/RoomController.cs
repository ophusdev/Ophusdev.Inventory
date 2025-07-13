using Inventory.Business.Abstraction;
using Inventory.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RoomController : ControllerBase
    {
        private readonly IBusiness _business;
        private readonly ILogger<RoomController> _logger;

        public RoomController(IBusiness business, ILogger<RoomController> logger)
        {
            _business = business;
            _logger = logger;
        }

        [HttpPost(Name = "CreateRoom")]
        public async Task<ActionResult> CreateRoom(RoomDto roomDto)
        {
            await _business.CreateRoomAsync(roomDto);

            return Ok("Room created");
        }

        [HttpGet(Name = "ReadRoom")]
        public async Task<ActionResult> ReadRoom(int idRoom)
        {
            RoomDto room = await _business.ReadRoomAsync(idRoom);

            return new JsonResult(room);
        }

        [HttpGet(Name = "ReadAllRooms")]
        public async Task<ActionResult> ReadAllRooms()
        {
            List<RoomDto> rooms = await _business.ReadAllRoomsAsync();

            return new JsonResult(rooms);
        }
    }
}
