using Project.Services.RewardAPI.Message;

namespace Project.Services.RewardAPI.Services
{
    public interface IRewardsService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}
