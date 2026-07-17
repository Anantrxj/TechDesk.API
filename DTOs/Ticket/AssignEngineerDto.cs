using System.ComponentModel.DataAnnotations;

namespace TechDesk.API.DTOs.Ticket
{
    public class AssignEngineerDto
    {
        [Required]
        public int EngineerId { get; set; }
    }
}