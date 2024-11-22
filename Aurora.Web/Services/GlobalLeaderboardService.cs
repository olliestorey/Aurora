using Aurora.Web.Data;

namespace Aurora.Web.Services
{
    public class GlobalLeaderboardService : IGlobalLeaderboardService
    {
        private DatabaseContext _dbContext;
        public GlobalLeaderboardService(DatabaseContext dbContext)
        {
                _dbContext = dbContext;
        }

        public async Task<(bool, string)> AddEntry(string name, int score, string email)
        {
            var LeaderboardEntry = new LeaderboardEntry
            {
                Name = name,
                Score = score,
                Email = email
            };

            try
            {
                await _dbContext.LeaderboardEntry.AddAsync(LeaderboardEntry);
                await _dbContext.SaveChangesAsync();

                return (true, "Entry added to global leaderboard successfully");
            }
            catch (Exception e)
            {
                return (false, "Failed to add entry to global leaderboard "+ e.Message);
            }
        }
    }

    interface IGlobalLeaderboardService
    {
        public Task<(bool, string)> AddEntry(string name, int score, string email);
    }
}
