using TechDesk.API.Enums;

namespace TechDesk.API.DTOs.Ticket
{
    public class TicketResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        public Priority Priority { get; set; }

        public TicketStatus Status { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedByDepartmentName { get; set; } = string.Empty;

        public string? AssignedEngineer { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }
    }
}