using TechDesk.API.DTOs.Ticket;

namespace TechDesk.API.Services.Interfaces
{
    public interface ITicketService
    {
        Task<List<TicketResponseDto>> GetAllAsync();

        Task<TicketResponseDto?> GetByIdAsync(int id);

        Task<TicketResponseDto> CreateAsync(CreateTicketDto dto , int createdById);

        Task<bool> UpdateAsync(int id, UpdateTicketDto dto);

        Task<bool> DeleteAsync(int id);
        Task<bool> AssignEngineerAsync(int ticketId, AssignEngineerDto dto);
        Task<bool> UpdateStatusAsync(int ticketId, UpdateTicketStatusDto dto);
        Task<List<TicketResponseDto>> GetMyTicketsAsync(int userId);
        Task<List<TicketResponseDto>> GetMyAssignedTicketsAsync(int engineerId);
        Task<List<TicketResponseDto>> SearchAsync(TicketFilterDto dto);
    }
}