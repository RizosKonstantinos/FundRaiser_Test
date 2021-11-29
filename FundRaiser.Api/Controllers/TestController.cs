using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundRaiser.Common.Dto;
using FundRaiser.Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FundRaiser.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;


        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger, IUserService userService, IProjectService projectService)
        {
            _logger = logger;
            _userService = userService;
            _projectService = projectService;
        }
        
        [HttpGet("getProject")]
        public async Task<ProjectDto> GetProjects(int projectId)
        {
            var projectFromDb = await _projectService.GetProject(projectId, false);
            
            return new ProjectDto(projectFromDb);
        }
        
        [HttpGet("getProjects")]
        public async Task<IEnumerable<ProjectDto>> GetProjects(int pageCount = 1, int pageSize = 10)
        {
            var projectsFromDb = await _projectService.GetProjects(pageCount, pageSize);
            
            return projectsFromDb.Select(p => new ProjectDto(p));
        }
        
        [HttpGet("getUserProjects")]
        public async Task<IEnumerable<ProjectDto>> UserProjects(int pageCount, int pageSize, int userId)
        {
            var projectsFromDb = await _projectService.GetProjects(pageCount, pageSize, userId);

            return projectsFromDb.Select(p => new ProjectDto(p));
        }
        
        
        [HttpGet("getUserFundedProjects")]
        public async Task<IEnumerable<ProjectDto>> UserFundedProjects(int userId)
        {
            var projectsFromDb = await _projectService.GetFundedProjects(userId);

           return projectsFromDb.Select(p => new ProjectDto(p));
        }
        
        [HttpGet("getProjectFinancialProgress")]
        public async Task<decimal> GetProjectFinancialProgress(int projectId)
        {
            var progress = await _projectService.GetTotalFinancialAmount(projectId);

            return progress;
        }
    }
}