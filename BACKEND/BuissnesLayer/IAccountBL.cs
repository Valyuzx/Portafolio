using BACKEND.DTOs;
namespace BACKEND.BuissnesLayer
{
    public interface IAccountBL
    {
        Task RegisterAsync(RegisterRequestDTO registerDto);
        Task RegisterAdminAsync(RegisterRequestDTO registerDto);
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginDto);
        Task<RefreshTokenResponseDTO> RefreshTokenAsync(string refreshToken, string? ipAddress = null);
        Task LogoutAsync(string accessToken, string? refreshToken, Guid userId);
    }
}
