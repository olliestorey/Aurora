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
        private readonly int score = 100;

        public WordController(ILogger<RoomController> logger, IRoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        /// <summary>
        /// Gets the words for the room
        /// </summary>
        /// <param name="roomKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<string>> GetWords(Guid roomKey)
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
                    await _roomService.UpdateRoom(room);

                    return new SubmitWordDto(true, playerPosition);
                }

                return new SubmitWordDto(true, null);
            }

            // trgger player score update event 

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
