using TechDesk.API.DTOs.Dashboard;

namespace TechDesk.API.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardResponseDto> GetDashboardAsync();
    }
}