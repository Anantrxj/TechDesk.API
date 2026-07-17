using TechDesk.API.Enums;

namespace TechDesk.API.DTOs.Category
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Priority DefaultPriority { get; set; }
    }
}