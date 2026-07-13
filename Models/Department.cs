namespace TechDesk.API.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string DepartmentName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}