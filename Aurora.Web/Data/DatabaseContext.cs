using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace Aurora.Web.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=app.db");
            }
        }

        public DbSet<LeaderboardEntry> LeaderboardEntry { get; set; }
    }

    public class LeaderboardEntry
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Score { get; set; }
        public string? Email { get; set; }
    }
}
