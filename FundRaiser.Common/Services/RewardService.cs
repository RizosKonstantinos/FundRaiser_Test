using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundRaiser.Common.Data;
using FundRaiser.Common.Dto;
using FundRaiser.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace FundRaiser.Common.Services
{
    public interface IRewardService
    {
        Task<Reward> Create(Reward rewardModel); // Create
        Task<Reward> Update(int id, Reward rewardModel); // Update
        Task<bool> Delete(int id);
        Task<List<Reward>> GetRewards(int projectId);
        Task<List<Reward>> GetBackerRewards(int userId, int projectId);
    }

    public class RewardService : IRewardService
    {
        private readonly FundRaiserDbContext _context;   
        
        public RewardService(FundRaiserDbContext context)
        {
            _context = context;
        }
        
        public async Task<Reward> Create(Reward rewardModel)
        {
            var reward = new Reward()
            {
                Title = rewardModel.Title,
                Description = rewardModel.Description,
                RequiredAmount = (decimal)rewardModel.RequiredAmount, 
            };

            await _context.Rewards.AddAsync(reward);
            await _context.SaveChangesAsync();

            return reward;
        }

       
        public async Task<Reward> Update(int id, Reward rewardModel)
        {
            var reward = await _context.Rewards.FirstOrDefaultAsync(p => p.Id == id);

            reward.Title = rewardModel.Title ?? reward.Title;
            reward.Description = rewardModel.Description ?? reward.Description;
            reward.RequiredAmount = rewardModel.RequiredAmount;

            await _context.Rewards.AddAsync(reward);
            await _context.SaveChangesAsync();

            return reward;
        }

        public async Task<bool> Delete(int id)
        {
            var reward = await _context.Rewards.FirstOrDefaultAsync(r=>r.Id == id);

            if (reward == null) return false;

            _context.Rewards.Remove(reward);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Reward>> GetRewards(int projectId)
        {
            return await _context.Rewards
                .Where(r => r.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<Reward>> GetBackerRewards(int userId, int projectId)
        {
            return await _context.Funds
                .Include(f => f.Reward)
                .Where(f => f.UserId == userId && f.Reward.ProjectId == projectId)
                .Select(f => f.Reward)
                .ToListAsync();
        }
    }
}