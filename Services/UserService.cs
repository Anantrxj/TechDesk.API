using Microsoft.EntityFrameworkCore;
using TechDesk.API.Data;
using TechDesk.API.DTOs;
using TechDesk.API.DTOs.User;
using TechDesk.API.Models;
using TechDesk.API.Services.Interfaces;

namespace TechDesk.API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        async Task<UserResponseDto> IUserService.CreateAsync(CreateUserDto dto)
        {
            bool emailexist = await _context.Users
                .AnyAsync(u => u.Email == dto.Email);

            if (emailexist)
            {
                throw new Exception("Email already exists.");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,
                PhoneNumber = dto.PhoneNumber,
                DepartmentId = dto.DepartmentId,
                Role = dto.Role
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var createduser = await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return new UserResponseDto
            {
                Id = createduser.Id,
                FullName = createduser.FullName,
                Email = createduser.Email,
                PhoneNumber = createduser.PhoneNumber,
                DepartmentName = createduser.Department.DepartmentName,
                DepartmentId = createduser.DepartmentId,
                Role = createduser.Role,
                IsActive = createduser.IsActive
            };

        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user == null )
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = await _context.Users
                .Include(u => u.Department)
                .ToListAsync();

            var response = users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role,
                IsActive = u.IsActive,
                DepartmentId = u.DepartmentId,
                DepartmentName = u.Department.DepartmentName
            }).ToList();

            return response;
        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Department)
                 .FirstOrDefaultAsync(u => u.Id == id);

            if(user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                IsActive = user.IsActive,
                DepartmentId = user.DepartmentId,
                DepartmentName = user.Department.DepartmentName
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if(user == null )
            { return false; }

            bool emailExists = await _context.Users
              .AnyAsync(u => u.Email == dto.Email && u.Id != id);

            if (emailExists)
            {
                throw new Exception("Email already exists.");
            }
            bool departmentExists = await _context.Departments
             .AnyAsync(d => d.Id == dto.DepartmentId);

            if (!departmentExists)
            {
                throw new Exception("Department not found.");
            }

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.DepartmentId = dto.DepartmentId;
            user.Role = dto.Role;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
