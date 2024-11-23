using Aurora.Web.Data;
using Aurora.Web.Events;
using Aurora.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Aurora.Web.Controllers
{
    [ApiController]
    [Route("/api/room")]
    public class RoomController : ControllerBase
    {
        private readonly IEventDispatcherService _eventDispatcherService;
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomService _roomService;

        public RoomController(ILogger<RoomController> logger, IRoomService roomService, IEventDispatcherService eventDispatcherService)
        {
            _logger = logger;
            _roomService = roomService;
            _eventDispatcherService = eventDispatcherService;
        }

        [HttpGet]
        public Room? GetRoomByCode(string roomCode)
        {
            return _roomService.GetRoomByCode(roomCode);
        }

        [HttpPost("create")]
        public async Task<string> CreateRoom([FromBody] CreateRoomRequest request)
        {
            var newRoom = await _roomService.CreateRoom(request.RoomCode, request.NumberOfWordsInGame);

            return newRoom.Code;
        }

        [HttpPost("start")]
        public async Task<bool> StartGame(string roomCode)
        {
            var x = new GameStartedEvent() { EventMessage = new { RoomCode = roomCode } };
            await _eventDispatcherService.DispatchEventAsync(x);

            return true;
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinRoom([FromBody]JoinRoomRequest joinRoomRequest)
        {
            try
            {
                string[] names = System.IO.File.ReadAllLines("data/naughtywords.txt").Select(x => x.Trim()).ToArray();

                if (names.Contains(joinRoomRequest.PlayerName))
                    return BadRequest("Please choose an appropriate name");
            }
            catch (Exception)
            {
                _logger.LogError("Could not read naughty words file");
            }

            if (!IsValidEmail(joinRoomRequest.PlayerEmail))
                return BadRequest("Invalid email address");

            var room = _roomService.JoinRoom(joinRoomRequest.RoomCode, joinRoomRequest.PlayerName, joinRoomRequest.PlayerEmail);

            var x = new PlayerJoinedGameEvent() { EventMessage = new { PlayerName = joinRoomRequest.PlayerName } };
            await _eventDispatcherService.DispatchEventAsync(x);

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
    public class CreateRoomRequest
    {
        public string RoomCode { get; set; }
        public int NumberOfWordsInGame { get; set; }
    }
    public class JoinRoomRequest
    {
        public string RoomCode { get; set; }

        public string PlayerName { get; set; }
        public string PlayerEmail { get;set; }
    }
}
