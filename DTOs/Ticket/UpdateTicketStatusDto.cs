using System.ComponentModel.DataAnnotations;
using TechDesk.API.Enums;

namespace TechDesk.API.DTOs.Ticket
{
    public class UpdateTicketStatusDto
    {
        [Required]
        public TicketStatus Status { get; set; }
    }
}