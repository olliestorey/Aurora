using Aurora.Web.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Aurora.Web.Controllers
{
    [ApiController]
    [Route("/api/globalleaderboard")]
    public class GlobalLeaderboardController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly ILogger<GlobalLeaderboardController> _logger;

        public GlobalLeaderboardController(ILogger<GlobalLeaderboardController> logger, DatabaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public Leaderboard Get()
        {
            var leaderboardData = new Leaderboard(_dbContext.LeaderboardEntry.ToList());

            return leaderboardData;
        }

    }
    public class Leaderboard
    {
        public List<Entry> Entries { get; set; } = new List<Entry>();

        public Leaderboard(IEnumerable<LeaderboardEntry> ldbEntry)
        {
            Entries = ldbEntry.Select(x => new Entry(x)).OrderBy(x => x.Score).Select((e, i) => { e.Position = i; return e; }).ToList() ?? new List<Entry>();
        }
    }

    public class Entry
    {
        public Entry(LeaderboardEntry ldbE)
        {
            Name = ldbE.Name ?? "NO_NAME";
            Score = ldbE.Score;
        }

        public string Name { get; set; }
        public int Score { get; set; }
        public int Position { get; set; }
    }
}
