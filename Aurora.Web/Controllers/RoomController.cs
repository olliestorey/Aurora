using Aurora.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace Aurora.Web.Controllers
{
    [ApiController]
    [Route("/api/room")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;

        public RoomController(ILogger<RoomController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public string CreateRoom(string roomCode, int numberOfWordsInGame)
        {
            // number of words must be > 5

            // generates words and associates them with the room

            return "GUID_FOR_ROOM_KEY";
        }

        [HttpPost]
        public bool StartGame(Guid roomKey)
        {
            // trigger game started event

            return true;
        }

        [HttpPost]
        public string JoinRoom(string roomCode, string playerName, string playerEmail)
        {
            // validate name and email against naughty words

            // trigger player joined event
            return "GUID_FOR_ROOM_KEY";
        }
    }
}
