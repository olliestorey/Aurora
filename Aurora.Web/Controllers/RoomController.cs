using Aurora.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
        public Room? GetRoomByCode(string roomCode)
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
        public IActionResult JoinRoom(string roomCode, string playerName, string playerEmail)
        {
            string[] names = System.IO.File.ReadAllLines("naughtywords.txt");
            if (names.Contains(playerName))
            {
                return BadRequest("Please choose an appropriate name");
            }

            if (!IsValidEmail(playerEmail))
            {
                return BadRequest("Invalid email address");
            }

            var room = _roomService.JoinRoom(roomCode, playerName, playerEmail);

            // trigger player joined event
            return Ok(room);
        }

        [HttpDelete()]
        public bool DeleteRoom(string roomCode)
        {
            return _roomService.DeleteRoom(roomCode);
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
