using TechDesk.API.Enums;

namespace TechDesk.API.DTOs.Ticket
{
    public class TicketFilterDto
    {
        public TicketStatus? Status { get; set; }

        public Priority? Priority { get; set; }

        public int? CategoryId { get; set; }
    }
}