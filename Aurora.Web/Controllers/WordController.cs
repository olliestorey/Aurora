using Aurora.Web.Events;
using Aurora.Web.Factories;
using Aurora.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aurora.Web.Controllers
{

    [ApiController]
    [Route("/api/words")]
    public class WordController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomService _roomService;
        private readonly IEventDispatcherService _eventDispatcherService;
        private readonly int score = 100;

        public WordController(ILogger<RoomController> logger, IRoomService roomService, IEventDispatcherService eventDispatcherService)
        {
            _logger = logger;
            _roomService = roomService;
            _eventDispatcherService = eventDispatcherService;
        }

        /// <summary>
        /// Gets the words for the room
        /// </summary>
        /// <param name="roomKey"></param>
        /// <returns></returns>
        [HttpGet("/getwords/{roomKey}")]
        public async Task<IEnumerable<string>> GetWords(string roomKey)
        {
            var wordFactory = new HardcodedWordFactory();

            return await wordFactory.GetWords();
        }

        [HttpPost("submitWord")]
        public async Task<SubmitWordDto> SubmitWord([FromBody] SubmitWordRequest request)
        {
            var room = _roomService.GetRoomByCode(request.RoomCode.ToString());
            if (room == null) return new SubmitWordDto(false, null);

            var isWordExist = room.Words.Contains(request.Word.ToLower());

            if (isWordExist && room.Players.Find(x => x.Id == request.PlayerKey)?.WordsSubmited.Contains(request.Word) == false)
            {
                var lobbyUpdated = new PlayerScoreUpdatedEvent() { EventMessage = new { Players = room.Players } };

                room.Players.Find(x => x.Id == request.PlayerKey).WordsSubmited.Add(request.Word);
                room.Players.Find(x => x.Id == request.PlayerKey).Score += score / room.Words.Count;

                await _roomService.UpdateRoom(room);

                if (room.Words.Count == room.Players.Find(x => x.Id == request.PlayerKey).WordsSubmited.Count)
                {
                    // Calculate player's position
                    var players = room.Players.OrderByDescending(x => x.Score).ToList();
                    var playerPosition = players.FindIndex(x => x.Id == request.PlayerKey) + 1;
                    var positionPoints = 21 - (playerPosition * 1);
                    room.Players.Find(x => x.Id == request.PlayerKey).Score += positionPoints;

                    var playerFinished = new PlayerGameCompletedEvent() { EventMessage = new { PlayerName = room.Players.Find(x => x.Id == request.PlayerKey).Name, Position = playerPosition } };

                    await Task.WhenAll(
                        Task.Run(() => _roomService.UpdateRoom(room)),
                        _eventDispatcherService.DispatchEventAsync(lobbyUpdated),
                        _eventDispatcherService.DispatchEventAsync(playerFinished)
                    );

                    return new SubmitWordDto(true, playerPosition);
                }

                await _eventDispatcherService.DispatchEventAsync(lobbyUpdated);

                return new SubmitWordDto(true, null);
            }

            return new SubmitWordDto(false, null);
        }
    }

    public class SubmitWordDto
    {
        public bool Result { get; set; }
        public int? Position { get; set; }

        public SubmitWordDto(bool result, int? position)
        {
            Result = result;
            Position = position;
        }
    }

    public class SubmitWordRequest
    {
        public string RoomCode { get; set; }
        public Guid PlayerKey { get; set; }
        public string Word { get; set; }
    }
}
