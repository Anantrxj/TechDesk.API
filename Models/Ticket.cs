using TechDesk.API.Enums;

namespace TechDesk.API.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public Priority Priority { get; set; }

        public TicketStatus Status { get; set; } = TicketStatus.Open;

        public int CreatedById { get; set; }

        public int? AssignedEngineerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }
        public Category Category { get; set; } = null!;

        public User CreatedBy { get; set; } = null!;

        public User? AssignedEngineer { get; set; }
    }
}