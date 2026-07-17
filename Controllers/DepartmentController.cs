using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechDesk.API.DTOs.Department;
using TechDesk.API.Services.Interfaces;

namespace TechDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(departments);

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);

            if (department == null)
                return NotFound();

            return Ok(department);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateDepartmentDto dto)
        {
            var department = await _departmentService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = department.Id },
                department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
        int id,
        UpdateDepartmentDto dto)
        {
            var updated = await _departmentService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _departmentService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
