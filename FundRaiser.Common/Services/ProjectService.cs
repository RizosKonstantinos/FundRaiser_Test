using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundRaiser.Common.Data;
using FundRaiser.Common.Dto;
using FundRaiser.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace FundRaiser.Common.Services
{
    public interface IProjectService
    {
        Task<Project> Add(Project projectModel); 
        Task<Project> Update(int id, Project projectModel); 
        Task<bool> Delete(int id); //Delete
        Task<Project> GetProject(int id, bool baseInfo = true);
        Task<List<Project>> GetProjects(int pageCount, int pageSize, int? userId = null, string title = null, Category? category = null);
        Task<List<Project>> GetFundedProjects(int userId);
        Task<decimal> GetTotalFinancialAmount(int projectId);
    }
    
    public class ProjectService : IProjectService
    {
        private readonly FundRaiserDbContext _context;   
        
        public ProjectService(FundRaiserDbContext context)
        {
            _context = context;
        }

        public async Task<Project> Add(Project projectModel)
        {
            var project = new Project
            {
                //Map base info
                Title = projectModel.Title,
                UserId = projectModel.UserId,
                Description = projectModel.Description,
                Category = (Category)projectModel.Category,
                Goal = (decimal)projectModel.Goal,           
                StartDate = projectModel.StartDate,
                EndTime = (DateTime)projectModel.EndTime,
                IsComplited = projectModel.IsComplited,
                NumberOfBackers = projectModel.NumberOfBackers                 
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return project;
        }
        
        public async Task<Project> Update(int id, Project projectModel)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

            //Map base info
            project.Title = projectModel.Title ?? project.Title;
            project.Category = projectModel.Category;
            project.Description = projectModel.Description ?? project.Description;
            project.Goal = projectModel.Goal;
            project.EndTime = projectModel.EndTime;
                        
            
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<bool> Delete(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Project> GetProject(int id, bool baseInfo = true)
        {
            return baseInfo 
                ? await _context.Projects.FirstOrDefaultAsync(p => p.Id == id)
                : await _context.Projects
                    .Include(p => p.Updates)
                    .Include(p => p.Rewards)
                    .Include(p => p.Media)
                    .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<List<Project>> GetProjects(int pageCount, int pageSize, int? userId = null, string title = null, Category? category = null)
        {
            if (pageSize <= 0) pageSize = 10;
            if (pageCount <= 0) pageCount = 1;
            
            return await _context.Projects
                .Skip((pageCount - 1) * pageSize)
                .Take(pageCount)
                .Where(p => 
                    userId == null || p.UserId == userId
                    && title == null || p.Title.Contains(title)
                    && category == null || p.Category == category)
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }
        
        public async Task<List<Project>> GetFundedProjects(int userId)
        {
            return await _context.Funds
                .Include(f => f.Reward)
                .ThenInclude(r => r.Project)
                .Where(f => f.UserId == userId)
                .Select(f => f.Reward.Project)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalFinancialAmount(int projectId)
        {
            return await _context.Funds
                .Include(f => f.Reward)
                .Where(f => f.Reward.ProjectId == projectId)
                .SumAsync(r => r.Reward.RequiredAmount);
        }

        //public async Task<int> GetNumberOfBackers(int projectId)
        //{
        //    return await _context.Funds
        //        .Include(f=>f.Reward)
        //        .Where(f => f.Reward.ProjectId == projectId)
        //        .
                 
        //}
    }
}