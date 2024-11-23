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

        [HttpPost]
        public async Task<SubmitWordDto> SubmitWord(string roomCode, Guid playerKey, string word)
        {
            var room = _roomService.GetRoomByCode(roomCode.ToString());
            if (room == null) return new SubmitWordDto(false, null);

            var isWordExist = room.Words.Contains(word.ToLower());

            if (isWordExist && room.Players.Find(x => x.Id == playerKey)?.WordsSubmited.Contains(word) == false)
            {
                var lobbyUpdated = new PlayerScoreUpdatedEvent() { EventMessage = new { Players = room.Players } };

                room.Players.Find(x => x.Id == playerKey).WordsSubmited.Add(word);
                room.Players.Find(x => x.Id == playerKey).Score += score / room.Words.Count;

                await _roomService.UpdateRoom(room);

                if (room.Words.Count == room.Players.Find(x => x.Id == playerKey).WordsSubmited.Count)
                {
                    // Calculate player's position
                    var players = room.Players.OrderByDescending(x => x.Score).ToList();
                    var playerPosition = players.FindIndex(x => x.Id == playerKey) + 1;
                    var positionPoints = 21 - (playerPosition * 1);
                    room.Players.Find(x => x.Id == playerKey).Score += positionPoints;

                    var playerFinished = new PlayerGameCompletedEvent() { EventMessage = new { PlayerName = room.Players.Find(x => x.Id == playerKey).Name } };

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
}
