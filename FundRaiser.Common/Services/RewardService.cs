using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundRaiser.Common.Data;
using FundRaiser.Common.Dto;
using FundRaiser.Common.Models;

namespace FundRaiser.Common.Services
{
    public interface IRewardService
    {
        Task<IEnumerable<Reward>> Add(List<RewardDto> dtos);
    }

    public class RewardService : IRewardService
    {
        private readonly FundRaiserDbContext _context;   
        
        public RewardService(FundRaiserDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Reward>> Add(List<RewardDto> dtos)
        {
            var rewards = dtos.Select(dto => new Reward
            {
                Description = dto.Description,
                ProjectId = dto.ProjectId,
                Title = dto.Title,
                RequiredAmount = dto.RequiredAmount
            });

            await _context.AddRangeAsync(rewards);
            await _context.SaveChangesAsync();

            return rewards;
        }
    }
}