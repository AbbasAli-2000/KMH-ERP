using KMH_ERP.Application.DTOs;

namespace KMH_ERP.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(UserDto, string token)> LoginAsync(UserLoginDto userLoginDto);
        Task LogoutAsync(int userId, string? ipAddress = null);
    }
}
