using Microsoft.EntityFrameworkCore;
using Project.Services.RewardAPI.Models;

namespace Project.Services.RewardAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Reward> Rewards { get; set; }
    }
}
