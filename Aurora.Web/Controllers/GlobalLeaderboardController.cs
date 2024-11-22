using Microsoft.AspNetCore.Mvc;

namespace Aurora.Web.Controllers
{
    [ApiController]
    [Route("/api/globalleaderboard")]
    public class GlobalLeaderboardController : ControllerBase
    {

        private readonly ILogger<GlobalLeaderboardController> _logger;

        public GlobalLeaderboardController(ILogger<GlobalLeaderboardController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Leaderboard Get()
        {
            return ReturnHardcodedLeaderboard();
        }

        private Leaderboard ReturnHardcodedLeaderboard()
        {
            return new Leaderboard
            {
                Entries = new List<LeaderboardEntry>
                {
                    new LeaderboardEntry { Name = "John", Score = 100, Position = 1 },
                    new LeaderboardEntry { Name = "Jane", Score = 90, Position = 2 },
                    new LeaderboardEntry { Name = "Alice", Score = 80, Position = 3 },
                    new LeaderboardEntry { Name = "Bob", Score = 70, Position = 4 },
                    new LeaderboardEntry { Name = "Eve", Score = 60, Position = 5 },
                }
            };
        }
    }
    public class Leaderboard
    {
        public List<LeaderboardEntry> Entries { get; set; }
    }

    public class LeaderboardEntry
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int Position { get; set; }
    }
}
