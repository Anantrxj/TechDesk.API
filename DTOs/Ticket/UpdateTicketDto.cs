using System.ComponentModel.DataAnnotations;
using TechDesk.API.Enums;

namespace TechDesk.API.DTOs.Ticket
{
    public class UpdateTicketDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public Priority Priority { get; set; }
    }
}