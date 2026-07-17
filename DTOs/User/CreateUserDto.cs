using System.ComponentModel.DataAnnotations;
using TechDesk.API.Enums;

namespace TechDesk.API.DTOs.User
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}