using Microsoft.EntityFrameworkCore;
using TechDesk.API.Data;
using TechDesk.API.DTOs.Ticket;
using TechDesk.API.Enums;
using TechDesk.API.Models;
using TechDesk.API.Services.Interfaces;

namespace TechDesk.API.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext _context;
        public TicketService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TicketResponseDto> CreateAsync(CreateTicketDto dto,int createdById)
        {
            bool categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId);

            if (!categoryExists)
                throw new Exception("Category not found.");

            bool userExists = await _context.Users
                .AnyAsync(u => u.Id == createdById);

            if (!userExists)
                throw new Exception("User not found.");

            var ticket = new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                Priority = dto.priority,
                CreatedById = createdById,
                Status = TicketStatus.Open,
                AssignedEngineerId = null,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Tickets.AddAsync(ticket);

            await _context.SaveChangesAsync();

            var createdTicket = await _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                    .ThenInclude(u => u.Department)
                .Include(t => t.AssignedEngineer)
                .FirstOrDefaultAsync(t => t.Id == ticket.Id);

            return new TicketResponseDto
            {
                Id = createdTicket!.Id,
                Title = createdTicket.Title,
                Description = createdTicket.Description,
                CategoryName = createdTicket.Category.CategoryName,
                Priority = createdTicket.Priority,
                Status = createdTicket.Status,
                CreatedBy = createdTicket.CreatedBy.FullName,
                CreatedByDepartmentName = createdTicket.CreatedBy.Department.DepartmentName,
                AssignedEngineer = createdTicket.AssignedEngineer?.FullName,
                CreatedAt = createdTicket.CreatedAt,
                UpdatedAt = createdTicket.UpdatedAt,
                ResolvedAt = createdTicket.ResolvedAt
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
                return false;

            _context.Tickets.Remove(ticket);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<TicketResponseDto>> GetAllAsync()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                    .ThenInclude(u => u.Department)
                .Include(t => t.AssignedEngineer)
                .ToListAsync();

            var response = tickets.Select(ticket => new TicketResponseDto
            {
                Id = ticket.Id,

                Title = ticket.Title,

                Description = ticket.Description,

                CategoryName = ticket.Category.CategoryName,
                CategoryId =ticket.CategoryId,

                Priority = ticket.Priority,

                Status = ticket.Status,

                CreatedBy = ticket.CreatedBy.FullName,
                CreatedByDepartmentName = ticket.CreatedBy.Department.DepartmentName,

                AssignedEngineer = ticket.AssignedEngineer?.FullName,

                CreatedAt = ticket.CreatedAt,

                UpdatedAt = ticket.UpdatedAt,

                ResolvedAt = ticket.ResolvedAt
            }).ToList();
            return response;
        }

        public async Task<TicketResponseDto?> GetByIdAsync(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                    .ThenInclude(u => u.Department)
                .Include(t => t.AssignedEngineer)
                .FirstOrDefaultAsync(t => t.Id == id);

            if(ticket == null)
            {
                return null;
            }
            return new TicketResponseDto
            {
                Id = ticket.Id,

                Title = ticket.Title,

                Description = ticket.Description,

                CategoryName = ticket.Category.CategoryName,

                Priority = ticket.Priority,

                Status = ticket.Status,

                CreatedBy = ticket.CreatedBy.FullName,
                CreatedByDepartmentName = ticket.CreatedBy.Department.DepartmentName,

                AssignedEngineer = ticket.AssignedEngineer?.FullName,

                CreatedAt = ticket.CreatedAt,

                UpdatedAt = ticket.UpdatedAt,

                ResolvedAt = ticket.ResolvedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateTicketDto dto)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == id);

            if(ticket == null)
            {
                return false;
            }

            bool categoryExists = await _context.Categories
                 .AnyAsync(c => c.Id == dto.CategoryId);

            if (!categoryExists)
            {
                throw new Exception("Category not found.");
            }

            ticket.Title = dto.Title;

            ticket.Description = dto.Description;

            ticket.CategoryId = dto.CategoryId;
            

            ticket.Priority = dto.Priority;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignEngineerAsync(int ticketId, AssignEngineerDto dto)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null)
                return false;

            var engineer = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Id == dto.EngineerId &&
                    u.Role == Role.Engineer);

            if (engineer == null)
                throw new Exception("Engineer not found.");

            ticket.AssignedEngineerId = dto.EngineerId;

            ticket.Status = TicketStatus.Assigned;

            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateStatusAsync(int ticketId, UpdateTicketStatusDto dto)
        {
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null)
                return false;

            ticket.Status = dto.Status;

            ticket.UpdatedAt = DateTime.UtcNow;

            if (dto.Status == TicketStatus.Resolved)
            {
                ticket.ResolvedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<TicketResponseDto>> GetMyTicketsAsync(int userId)
        {
            var tickets = await _context.Tickets
                .Where(t => t.CreatedById == userId)
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                    .ThenInclude(u => u.Department)
                .Include(t => t.AssignedEngineer)
                .ToListAsync();

            var response = tickets.Select(ticket => new TicketResponseDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                CategoryName = ticket.Category.CategoryName,
                Priority = ticket.Priority,
                Status = ticket.Status,
                CreatedBy = ticket.CreatedBy.FullName,
                CreatedByDepartmentName = ticket.CreatedBy.Department.DepartmentName,
                AssignedEngineer = ticket.AssignedEngineer?.FullName,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                ResolvedAt = ticket.ResolvedAt
            }).ToList();

            return response;
        }

        public async Task<List<TicketResponseDto>> GetMyAssignedTicketsAsync(int engineerId)
        {
            var tickets = await _context.Tickets
                .Where(t => t.AssignedEngineerId == engineerId)
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                    .ThenInclude(u => u.Department)
                .Include(t => t.AssignedEngineer)
                .ToListAsync();

            var response = tickets.Select(ticket => new TicketResponseDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                CategoryName = ticket.Category.CategoryName,
                Priority = ticket.Priority,
                Status = ticket.Status,
                CreatedBy = ticket.CreatedBy.FullName,
                CreatedByDepartmentName = ticket.CreatedBy.Department.DepartmentName,
                AssignedEngineer = ticket.AssignedEngineer?.FullName,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                ResolvedAt = ticket.ResolvedAt
            }).ToList();

            return response;
        }

        public async Task<List<TicketResponseDto>> SearchAsync(TicketFilterDto dto)
        {
            var query = _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.CreatedBy)
                    .ThenInclude(u => u.Department)
                .Include(t => t.AssignedEngineer)
                .AsQueryable();

            if (dto.Status.HasValue)
            {
                query = query.Where(t => t.Status == dto.Status.Value);
            }

            if (dto.Priority.HasValue)
            {
                query = query.Where(t => t.Priority == dto.Priority.Value);
            }

            if (dto.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == dto.CategoryId.Value);
            }

            var tickets = await query.ToListAsync();

            var response = tickets.Select(ticket => new TicketResponseDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                CategoryName = ticket.Category.CategoryName,
                Priority = ticket.Priority,
                Status = ticket.Status,
                CreatedBy = ticket.CreatedBy.FullName,
                CreatedByDepartmentName = ticket.CreatedBy.Department.DepartmentName,
                AssignedEngineer = ticket.AssignedEngineer?.FullName,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                ResolvedAt = ticket.ResolvedAt
            }).ToList();

            return response;
        }
    }
}
