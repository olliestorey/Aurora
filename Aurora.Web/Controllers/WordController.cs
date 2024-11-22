using Aurora.Web.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Aurora.Web.Controllers
{

    [ApiController]
    [Route("/api/words")]
    public class WordController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;

        public WordController(ILogger<RoomController> logger)
        {
            _logger = logger;
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
        public bool SubmitWord(Guid roomKey, Guid playerKey, string word)
        {
            // validate word exists in game

            // validate that player has not already submitted this word

            // update player score in game state

            // trgger player score update event 

            return true;
        }
    }
}
