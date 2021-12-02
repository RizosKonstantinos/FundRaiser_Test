using System;
using System.Collections.Generic;
using System.Linq;
using FundRaiser.Common.Models;

namespace FundRaiser.Common.Dto
{
    public class ProjectDto
    {
        public ProjectDto() { }
        
        public ProjectDto(Project project)
        {
            Id = project.Id;
            UserId = project.UserId;
            Title = project.Title;
            Description = project.Description;
            Category = project.Category;
            Goal = project.Goal;
            CurrentAmount = project.CurrentAmount;
            IsComplited = project.IsComplited;
            StartDate = project.StartDate;
            EndTime = project.EndTime;
            NumberOfBackers = project.NumberOfBackers;
            
            //Check Rewards collection 
            if (project.Rewards != null)
                Rewards = project.Rewards.Select(reward => new RewardDto(reward));
            
            //Check Updates collection 
            if (project.Updates != null)
                Updates = project.Updates.Select(update => new UpdateDto(update));
            
            //Check Media collection 
            if (project.Media != null)
                Media = project.Media.Select(update => new MediaDto(update));
        }
        
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public decimal? Goal { get; set; }
        public decimal? CurrentAmount { get; set; }
        public bool IsComplited { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndTime { get; set; }
        public int NumberOfBackers { get; set; }
        public IEnumerable<RewardDto> Rewards { get; set; }
        public IEnumerable<UpdateDto> Updates { get; set; }
        public IEnumerable<MediaDto> Media { get; set; }

    }
}