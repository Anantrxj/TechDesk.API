using Microsoft.EntityFrameworkCore;
using TechDesk.API.Data;
using TechDesk.API.DTOs.Department;
using TechDesk.API.Models;
using TechDesk.API.Services.Interfaces;

namespace TechDesk.API.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<DepartmentResponseDto> CreateAsync(CreateDepartmentDto dto)
        {
            bool exists = await _context.Departments
                .AnyAsync(d => d.DepartmentName == dto.DepartmentName);

            if (exists)
                throw new Exception("Department already exists.");

            var department = new Department
            {
                DepartmentName = dto.DepartmentName,
                Description = dto.Description
            };

            await _context.Departments.AddAsync(department);

            await _context.SaveChangesAsync();

            return new DepartmentResponseDto
            {
                Id = department.Id,
                DepartmentName = department.DepartmentName,
                Description = department.Description
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return false;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return true;
        }

        
        public async Task<List<DepartmentResponseDto>> GetAllAsync()
        {
            var departments = await _context.Departments.ToListAsync();

            var response = departments.Select(d => new DepartmentResponseDto
            {
                Id = d.Id,
                DepartmentName = d.DepartmentName,
                Description = d.Description
            }).ToList();

            return response;
        }
        

        public async Task<DepartmentResponseDto?> GetByIdAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                return null;
            }
            return new DepartmentResponseDto
            {
                Id = department.Id,
                DepartmentName = department.DepartmentName,
                Description = department.Description
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateDepartmentDto dto)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return false;

            bool exists = await _context.Departments.AnyAsync(d =>
                d.DepartmentName == dto.DepartmentName &&
                d.Id != id);

            if (exists)
                throw new Exception("Department already exists.");

            department.DepartmentName = dto.DepartmentName;
            department.Description = dto.Description;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}