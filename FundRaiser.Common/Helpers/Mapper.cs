using System.Linq;
using FundRaiser.Common.Dto;
using FundRaiser.Common.Models;

namespace FundRaiser.Common.Helpers
{
    public static class Mapper
    {
        public static Project DtoToProject(ProjectDto dto)
        {
            var project = new Project();
            
            //Base info
            project.Id = dto.Id;
            project.Title = dto.Title;
            project.UserId = dto.UserId;
            
            //Navigation info
            if (dto.Updates != null)
                project.Updates = dto.Updates.Select(dto => new Update()
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    PostDate = dto.PostDate,
                    ProjectId = dto.ProjectId
                }).ToList();
            
            if (dto.Rewards != null)
                project.Rewards = dto.Rewards.Select(dto => new Reward()
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    ProjectId = dto.ProjectId,
                    Description = dto.Description,
                    RequiredAmount = dto.RequiredAmount
                }).ToList();

            return project;
        }
    }
}