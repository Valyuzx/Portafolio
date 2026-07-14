using BACKEND.BuissnesLayer;
using BACKEND.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BACKEND.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : ControllerBase
    {
        private readonly IAccountBL _accountBL;

        public AuthController(IAccountBL accountBL)
        {
            _accountBL = accountBL;
        }

        /// <summary>Inicia sesión y retorna los tokens de acceso.</summary>
        /// <response code="200">Login exitoso.</response>
        /// <response code="401">Credenciales incorrectas.</response>
        [HttpPost("v1/accounts/login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            // UnauthorizedException → GlobalExceptionHandler → 401 ProblemDetails
            var resultado = await _accountBL.LoginAsync(loginDTO);
            return Ok(resultado);
        }

        /// <summary>Registra un nuevo usuario con rol User.</summary>
        /// <response code="201">Usuario registrado exitosamente.</response>
        /// <response code="409">El correo ya está registrado.</response>
        [HttpPost("v1/accounts/register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            // ConflictException / NotFoundException → GlobalExceptionHandler
            await _accountBL.RegisterAsync(registerDTO);
            return StatusCode(StatusCodes.Status201Created,
                new { mensaje = "Usuario registrado exitosamente." });
        }

        /// <summary>Registra un nuevo usuario con rol Admin.</summary>
        /// <response code="201">Admin registrado exitosamente.</response>
        /// <response code="409">El correo ya está registrado.</response>
        [HttpPost("v1/accounts/registerAdmin")]
        [Authorize(Policy = "AdminPolicy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestDTO registerDTO)
        {
            await _accountBL.RegisterAdminAsync(registerDTO);
            return StatusCode(StatusCodes.Status201Created,
                new { mensaje = "Admin registrado exitosamente." });
        }

        /// <summary>Renueva el access token usando un refresh token válido.</summary>
        /// <response code="200">Tokens renovados.</response>
        /// <response code="401">Refresh token inválido o expirado.</response>
        [HttpPost("v1/accounts/refresh")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var ipAddress = GetClientIpAddress();
            var resultado = await _accountBL.RefreshTokenAsync(request.RefreshToken, ipAddress);
            return Ok(resultado);
        }

        /// <summary>Cierra la sesión del usuario actual.</summary>
        /// <response code="204">Sesión cerrada correctamente.</response>
        /// <response code="401">Token no encontrado o inválido.</response>
        [HttpPost("v1/accounts/logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDTO? request = null)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var authHeader  = HttpContext.Request.Headers.Authorization.ToString();
            var accessToken = authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
                ? authHeader["Bearer ".Length..].Trim()
                : string.Empty;

            if (string.IsNullOrEmpty(accessToken))
                return BadRequest(new { mensaje = "Token no encontrado en el header." });

            await _accountBL.LogoutAsync(accessToken, request?.RefreshToken, userId);
            return NoContent();
        }

        private string? GetClientIpAddress() =>
            HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}
