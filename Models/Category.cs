using TechDesk.API.Enums;

namespace TechDesk.API.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Priority DefaultPriority { get; set; }
    }
}