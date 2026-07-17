using Microsoft.AspNetCore.Mvc;
using TechDesk.API.DTOs.User;
using TechDesk.API.Services;
using TechDesk.API.Services.Interfaces;

namespace TechDesk.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userservice;

        public UserController(IUserService userservice)
        {
            _userservice = userservice;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userservice.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userservice.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var user = await _userservice.CreateAsync(dto);

            return CreatedAtAction
                (nameof(GetById),
                new { id = user.Id },
                user);
        }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, UpdateUserDto dto)
            {
                bool updated = await _userservice.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound();

                return NoContent();
            }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _userservice.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

    }
}
