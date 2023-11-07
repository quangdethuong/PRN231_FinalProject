using Microsoft.EntityFrameworkCore;
using Project.Services.RewardAPI.Data;
using Project.Services.RewardAPI.Message;
using Project.Services.RewardAPI.Models;

namespace Project.Services.RewardAPI.Services
{
    public class RewardsService : IRewardsService
    {
        private DbContextOptions<AppDbContext> _dbOptions;
        public RewardsService(DbContextOptions<AppDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }
        public async Task UpdateRewards(RewardsMessage rewardsMessage)
        {
            try
            {
                Reward rewards = new()
                {
                    OrderId = rewardsMessage.OrderId,
                    RewardsActivity = rewardsMessage.RewardsActivity,
                    UserId = rewardsMessage.UserId,
                    RewardsDate = DateTime.Now
                };
                await using var _db = new AppDbContext(_dbOptions);
                await _db.Rewards.AddAsync(rewards);
                await _db.SaveChangesAsync();
            } catch (Exception ex)
            {

            }
        }
    }
}
