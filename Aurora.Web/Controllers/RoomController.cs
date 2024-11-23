using Aurora.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aurora.Web.Controllers
{
    [ApiController]
    [Route("/api/room")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomService _roomService;

        public RoomController(ILogger<RoomController> logger, IRoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        [HttpGet]
        public Room GetRoomByCode(string roomCode)
        {
            return _roomService.GetRoomByCode(roomCode);
        }

        [HttpPost]
        public async Task<string> CreateRoom(string roomCode, int numberOfWordsInGame)
        {
            var newRoom = await _roomService.CreateRoom(roomCode, numberOfWordsInGame);

            return newRoom.Code;
        }

        [HttpPost("start")]
        public bool StartGame(Guid roomKey)
        {
            // trigger game started event

            return true;
        }

        [HttpPost("join")]
        public Room JoinRoom(string roomCode, string playerName, string playerEmail)
        {
            var room = _roomService.JoinRoom(roomCode, playerName, playerEmail);

            // trigger player joined event
            return room;
        }
    }
}
