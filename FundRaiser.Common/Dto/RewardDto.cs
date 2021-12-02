using FundRaiser.Common.Models;

namespace FundRaiser.Common.Dto
{
    public class RewardDto
    {
        public RewardDto() { }
        
        public RewardDto(Reward reward)
        {
            Id = reward.Id;
            ProjectId = reward.ProjectId;
            Title = reward.Title;
            Description = reward.Description;
            RequiredAmount = reward.RequiredAmount;
        }
        
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? RequiredAmount { get; set; }
    }
}