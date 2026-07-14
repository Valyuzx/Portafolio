using BACKEND.DTOs;
using BACKEND.Exceptions;
using BACKEND.Helpers;
using BACKEND.Models;
using Microsoft.EntityFrameworkCore;

namespace BACKEND.BuissnesLayer
{
    public class AccountBL : IAccountBL
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly BlackListService _blackListService;
        private readonly ILogger<AccountBL> _logger;

        public AccountBL( ApplicationDbContext context, TokenService tokenService,
        ILogger<AccountBL> logger, RefreshTokenService refreshTokenService,BlackListService blackListService)
        {
            _context = context;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
            _blackListService = blackListService;
            _logger = logger;
        }

        public async Task RegisterAsync(RegisterRequestDTO registerDto)
        {
            string normalizedEmail = registerDto.Email.Trim().ToLowerInvariant();

            if (await _context.Users.AnyAsync(u => u.Email == normalizedEmail))
                throw new ConflictException("El correo electrónico ya se encuentra registrado.");

            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User")
                ?? throw new NotFoundException("No existe el rol.");

            var userId = Guid.NewGuid();
            var nuevoUsuario = new User
            {
                UserId    = userId,
                Email     = normalizedEmail,
                Password  = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, workFactor: 10),
                IsActive  = true,
                CreatedAt = DateTime.UtcNow,
                UserName  = registerDto.UserName,
            };

            _context.Users.Add(nuevoUsuario);
            _context.UserRoles.Add(new UserRole { UserId = userId, RoleId = rol.RoleId });
            await _context.SaveChangesAsync();

            _logger.LogInformation("Usuario registrado. Email: {Email}", normalizedEmail);
        }

        public async Task RegisterAdminAsync(RegisterRequestDTO registerDto)
        {
            string normalizedEmail = registerDto.Email.Trim().ToLowerInvariant();

            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail))
            {
                _logger.LogWarning("Intento de registro duplicado. Email: {Email}", normalizedEmail);
                throw new ConflictException("El correo electrónico ya se encuentra registrado.");
            }

            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin")
                ?? throw new NotFoundException("El rol 'Admin' no está configurado en el sistema. Contacta al administrador.");

            var userId = Guid.NewGuid();
            var nuevoUsuario = new User
            {
                UserId    = userId,
                Email     = normalizedEmail,
                Password  = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                IsActive  = true,
                CreatedAt = DateTime.UtcNow,
                UserName  = registerDto.UserName,
            };

            _context.Users.Add(nuevoUsuario);
            _context.UserRoles.Add(new UserRole { UserId = userId, RoleId = rol.RoleId });
            await _context.SaveChangesAsync();

            _logger.LogInformation("Admin registrado. Email: {Email}", normalizedEmail);
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginDto)
        {
            string normalizedEmail = loginDto.Email.Trim().ToLowerInvariant();

            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Login fallido. Usuario no encontrado o inactivo. Email: {Email}", normalizedEmail);
                throw new UnauthorizedException("Credenciales incorrectas.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                _logger.LogWarning("Login fallido. Contraseña inválida. Email: {Email}", normalizedEmail);
                throw new UnauthorizedException("Credenciales incorrectas.");
            }

            user.LastLoginAt = DateTime.UtcNow;

            string accessToken = _tokenService.GenerateJwtToken(user);
        
            var refreshTokenEntity = await _refreshTokenService.CreateRefreshTokenAsync(user.UserId);
            var expiresAt          = _tokenService.GetTokenExpiration(accessToken)
                                     ?? DateTime.UtcNow.AddMinutes(60);

            _logger.LogInformation("Login exitoso. Email: {Email} | UserId: {UserId}", user.Email, user.UserId);

            return new AuthResponseDTO
            {
                Token        = accessToken,
                RefreshToken = refreshTokenEntity.Token,  
                ExpiresAt    = expiresAt
            };
        }

        public async Task<RefreshTokenResponseDTO> RefreshTokenAsync(string refreshToken, string? ipAddress = null)
        {
            var newRefreshToken = await _refreshTokenService.RotateRefreshTokenAsync(refreshToken, ipAddress);

            if (newRefreshToken == null)
            {
                _logger.LogWarning("Intento de refresh con token inválido o revocado.");
                throw new UnauthorizedException("Refresh token inválido o expirado.");
            }

            string newAccessToken = _tokenService.GenerateJwtToken(newRefreshToken.User);
            var expiresAt = _tokenService.GetTokenExpiration(newAccessToken) ?? DateTime.UtcNow.AddMinutes(15);

            return new RefreshTokenResponseDTO
            {
                AccessToken  = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                ExpiryDate   = expiresAt
            };
        }

        public async Task LogoutAsync(string accessToken, string? refreshToken, Guid userId)
        {
            var jti        = _tokenService.GetJtiFromToken(accessToken);
            var expiration = _tokenService.GetTokenExpiration(accessToken);

            if (jti != null && expiration != null)
                await _blackListService.AddToBlackListAsync(jti, accessToken, userId, expiration.Value);

            await _refreshTokenService.RevokeAllUserTokensAsync(userId);

            _logger.LogInformation("Logout exitoso. UserId: {UserId}", userId);
        }
    }
}
