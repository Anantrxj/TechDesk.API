using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechDesk.API.Services.Interfaces;

namespace TechDesk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = await _dashboardService.GetDashboardAsync();

            return Ok(dashboard);
        }
    }
}