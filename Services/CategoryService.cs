using Microsoft.EntityFrameworkCore;
using TechDesk.API.Data;
using TechDesk.API.DTOs.Category;
using TechDesk.API.Models;
using TechDesk.API.Services.Interfaces;

namespace TechDesk.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
        {
            bool exists = await _context.Categories
                .AnyAsync(c => c.CategoryName == dto.CategoryName);

            if (exists)
                throw new Exception("Category already exists.");

            var category = new Category
            {
                CategoryName = dto.CategoryName,
                Description = dto.Description,
                DefaultPriority = dto.DefaultPriority
            };

            await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();

            return new CategoryResponseDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description,
                DefaultPriority = category.DefaultPriority
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return false;

            _context.Categories.Remove(category);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _context.Categories.ToListAsync();

            return categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                Description = c.Description,
                DefaultPriority = c.DefaultPriority
            }).ToList();
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            return new CategoryResponseDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName,
                Description = category.Description,
                DefaultPriority = category.DefaultPriority
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return false;

            bool exists = await _context.Categories
                .AnyAsync(c => c.CategoryName == dto.CategoryName && c.Id != id);

            if (exists)
                throw new Exception("Category already exists.");

            category.CategoryName = dto.CategoryName;
            category.Description = dto.Description;
            category.DefaultPriority = dto.DefaultPriority;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
