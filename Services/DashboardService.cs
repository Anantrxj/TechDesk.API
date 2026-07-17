using Microsoft.EntityFrameworkCore;
using TechDesk.API.Data;
using TechDesk.API.DTOs.Dashboard;
using TechDesk.API.Enums;
using TechDesk.API.Services.Interfaces;

namespace TechDesk.API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardResponseDto> GetDashboardAsync()
        {
            var totalUsers = await _context.Users.CountAsync();

            var totalEngineers = await _context.Users
                .CountAsync(u => u.Role == Role.Engineer);

            var totalDepartments = await _context.Departments.CountAsync();

            var totalCategories = await _context.Categories.CountAsync();

            var totalTickets = await _context.Tickets.CountAsync();

            var openTickets = await _context.Tickets
                .CountAsync(t => t.Status == TicketStatus.Open);

            var assignedTickets = await _context.Tickets
                .CountAsync(t => t.Status == TicketStatus.Assigned);

            var inProgressTickets = await _context.Tickets
                .CountAsync(t => t.Status == TicketStatus.InProgress);

            var resolvedTickets = await _context.Tickets
                .CountAsync(t => t.Status == TicketStatus.Resolved);

            var closedTickets = await _context.Tickets
                .CountAsync(t => t.Status == TicketStatus.Closed);

            return new DashboardResponseDto
            {
                TotalUsers = totalUsers,
                TotalEngineers = totalEngineers,
                TotalDepartments = totalDepartments,
                TotalCategories = totalCategories,
                TotalTickets = totalTickets,
                OpenTickets = openTickets,
                AssignedTickets = assignedTickets,
                InProgressTickets = inProgressTickets,
                ResolvedTickets = resolvedTickets,
                ClosedTickets = closedTickets
            };
        }
    }
}