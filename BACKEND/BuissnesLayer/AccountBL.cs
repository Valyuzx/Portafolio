    using BACKEND.DTOs;
    using BACKEND.Helpers;
    using BACKEND.Models;
    using Microsoft.EntityFrameworkCore;


    namespace BACKEND.BuissnesLayer
    {
        public class AccountBL: IAccountBL
        {
            private readonly ApplicationDbContext _context;
            private readonly TokenService _tokenService;
            private readonly ILogger<AccountBL> _logger;
            public AccountBL(ApplicationDbContext context, TokenService tokenService, ILogger<AccountBL> logger)
            {
                _context = context;
                _tokenService = tokenService;
                _logger = logger;
            }
            public async Task<bool> RegisterAsync(RegisterRequestDTO registerDto)
            {
                string normalizedEmail = registerDto.Email.Trim().ToLowerInvariant();

                var exists = await _context.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail);
                if (exists) return false;

                var userId = Guid.NewGuid();

                string password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

                var nuevoUsuario = new User
                {
                    UserId = userId,
                    Email = normalizedEmail,
                    Password = password,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UserName = registerDto.UserName,
                };

                var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Name == registerDto.RoleName);
                if (rol == null) return false;
                var userRole = new UserRole { UserId = userId, RoleId = rol.RoleId };
                _context.Users.Add(nuevoUsuario);
                _context.UserRoles.Add(userRole);
                return await _context.SaveChangesAsync() > 0;
            }

            public async Task<bool> RegisterAdminAsync(RegisterRequestDTO registerDto)
            {
                string normalizedEmail = registerDto.Email.Trim().ToLowerInvariant();
                var exists = await _context.Users.AnyAsync(u => u.Email.ToLower() == normalizedEmail);

                if (exists) return false;

                var userId = Guid.NewGuid();

                string password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
                var nuevoUsuario = new User
                {
                    UserId = userId,
                    Email = normalizedEmail,
                    Password = password,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UserName = registerDto.UserName,
                };

                var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
                if (rol == null) return false;

                var userRole = new UserRole { UserId = userId, RoleId = rol.RoleId };
                _context.Users.Add(nuevoUsuario);
                _context.UserRoles.Add(userRole);   

                return await _context.SaveChangesAsync() > 0;  
            }

            public async Task<AuthResponseDTO?> LoginAsync(LoginRequestDTO loginDto)
            {
                string normalizedEmail = loginDto.Email.Trim().ToLowerInvariant();

                var user = await _context.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

                if (user == null || !user.IsActive) return null;

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
                if (!isPasswordValid) return null;

                user.LastLoginAt = DateTime.UtcNow;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                string token = _tokenService.GenerateJwtToken(user);

                return new AuthResponseDTO
                {
                    Token = token,
                    Email = user.Email,
                    Name = user.UserName
                };
            }
        }
    }
