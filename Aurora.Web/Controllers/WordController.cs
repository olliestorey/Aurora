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
        public async Task<bool> SubmitWord(string roomCode, Guid playerKey, string word)
        {
            var room = _roomService.GetRoomByCode(roomCode.ToString());
            var isWordExist = room.Words.Contains(word.ToLower());

            if (isWordExist && room.Players.Find(x => x.Id == playerKey)?.WordsSubmited.Contains(word) == false)
            {
                room.Players.Find(x => x.Id == playerKey).WordsSubmited.Add(word);
                room.Players.Find(x => x.Id == playerKey).Score += 25;

                await _roomService.UpdateRoom(room);

                return true;
            }

            // trgger player score update event 

            return false;
        }
    }
}
