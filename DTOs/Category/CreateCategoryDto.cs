using System.ComponentModel.DataAnnotations;
using TechDesk.API.Enums;

namespace TechDesk.API.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public Priority DefaultPriority { get; set; }
    }
}