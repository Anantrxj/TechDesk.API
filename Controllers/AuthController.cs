using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechDesk.API.DTOs.Auth;
using TechDesk.API.Services.Interfaces;

namespace TechDesk.API.Controllers
{
    [ApiController]

    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task <IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if(result == null)
            {
                return Unauthorized("Invalid credentials");
            }
            return Ok(result);

        }
    }
}
