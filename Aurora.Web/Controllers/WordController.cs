using Aurora.Web.Events;
using Aurora.Web.Factories;
using Aurora.Web.Models.RequestDtos;
using Aurora.Web.Models.ResonseDtos;
using Aurora.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Aurora.Web.Controllers
{

    [ApiController]
    [Route("/api/words")]
    public class WordController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomService _roomService;
        private readonly IEventDispatcherService _eventDispatcherService;
        private readonly IGlobalLeaderboardService _globalLeaderboardService;

        public WordController(ILogger<RoomController> logger, IRoomService roomService, IEventDispatcherService eventDispatcherService, IGlobalLeaderboardService globalLeaderboardService)
        {
            _logger = logger;
            _roomService = roomService;
            _eventDispatcherService = eventDispatcherService;
            _globalLeaderboardService = globalLeaderboardService;
        }

        /// <summary>
        /// Gets the words for the room
        /// </summary>
        /// <param name="roomKey"></param>
        /// <returns></returns>
        [HttpGet("/getwords/{roomKey}")]
        public async Task<IEnumerable<string>> GetWords(string roomKey, string? listType)
        {
            var wordFactory = new HardcodedWordFactory();

            return await wordFactory.GetWords(listType);
        }

        [HttpPost("submitWord")]
        public async Task<SubmitWordDto> SubmitWord([FromBody] SubmitWordRequest request)
        {
            var room = _roomService.GetRoomByCode(request.RoomCode.ToString());
            var player = room?.Players.Find(x => x.Id == request.PlayerKey);
            int? playerPosition = null;

            if (room == null || player == null) throw new BadHttpRequestException("Room or player not found");
            request.Word = request.Word.ToLower();

            var validWordInGame = room.Words.Contains(request.Word);

            if (validWordInGame && !player.WordsSubmited.Contains(request.Word))
            {
                player.WordsSubmited.Add(request.Word);
            }

            if (!request.WordWasSkipped)
            {
                player.WordsSubmited.Add(request.Word);
                player.Score += CalculateWordScore(request.Word);
            }

            // Player has completed the game
            bool playerFinished = room.Words.Count == player.WordsSubmited.Count;

            if (playerFinished)
            {
                player.Score += (int)(1000 - (DateTime.Now - room.StartTime).TotalSeconds);
            }

            room.Players.Remove(room.Players.First(x => x.Id == request.PlayerKey));
            room.Players.Add(player);
            room.Players.OrderByDescending(x => x.Score);
            playerPosition = room.Players.IndexOf(player) + 1;

            await _roomService.UpdateRoom(room);

            if (playerFinished)
            {
                await Task.WhenAll(
                    Task.Run(() => _roomService.UpdateRoom(room)),
                    _eventDispatcherService.DispatchEventAsync(new PlayerGameCompletedEvent() { EventMessage = new { PlayerName = player.Name, Position = playerPosition } }),
                    _globalLeaderboardService.AddEntry(player.Name, player.Score, player.Email)
                );
            }

            var lobbyUpdated = new PlayerScoreUpdatedEvent() { EventMessage = new { Players = room.Players, TotalWords = room.Words.Count() } };
            await _eventDispatcherService.DispatchEventAsync(lobbyUpdated);

            return new SubmitWordDto(true, playerPosition);
        }

        private int CalculateWordScore(string word)
        {
            decimal wordDifficulty = word switch
            {
                var w when HardcodedWordFactory.CultureWords.Contains(w) => 1.2M,
                var w when HardcodedWordFactory.DevEasy.Contains(w) => 1.4M,
                var w when HardcodedWordFactory.DevHard.Contains(w) => 1.6M,
                _ => 1.0M // Default difficulty
            };

            return (int)Math.Round((8 + word.Length) * wordDifficulty * 5);
        }
    }
}
