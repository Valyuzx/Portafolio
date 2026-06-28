using BACKEND.DTOs;

namespace BACKEND.BuissnesLayer
{
    public interface IAccountBL
    {
        Task<bool> RegisterAsync(RegisterRequestDTO registerDto);
        Task<bool> RegisterAdminAsync(RegisterRequestDTO registerDto);
        Task<AuthResponseDTO?> LoginAsync(LoginRequestDTO loginDto);
    }
}
