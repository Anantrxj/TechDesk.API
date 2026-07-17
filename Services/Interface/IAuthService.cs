using TechDesk.API.DTOs.Auth;

namespace TechDesk.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto dto);
    }
}