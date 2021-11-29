using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FundRaiser.Common.Models;
using FundRaiser.Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FundRaiser.Mvc.Models;

namespace FundRaiser.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IProjectService projectService)
        {
            _logger = logger;
            _userService = userService;
            _projectService = projectService;
        }
        
        public IActionResult Index()
        {
            
            return View();
        }
        
        [HttpGet("getProjects")]
        public async Task<ActionResult<List<Project>>> Indexrt(int pageCount = 1, int pageSize = 10)
        {
            var projectsFromDb = await _projectService.GetProjects(pageCount, pageSize);
            
            return Ok(projectsFromDb);
        }
        
        [HttpGet("getUserProjects")]
        public async Task<IActionResult> UserProjects(int pageCount, int pageSize, int userId)
        {
            var projectsFromDb = await _projectService.GetProjects(pageCount, pageSize, userId);
            
            return Ok(projectsFromDb);
        }
        
        
        [HttpGet("getUserFundedProjects")]
        public async Task<IActionResult> UserFundedProjects(int userId)
        {
            var projectsFromDb = await _projectService.GetFundedProjects(userId);
            
            return Ok(projectsFromDb);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}