using BACKEND.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BACKEND.BuissnesLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

        [HttpPost("v1/accounts/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resultado = await _accountBL.LoginAsync(loginDTO);
            if (resultado == null)
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas." });
            }

            return Ok(resultado);
        }

        [HttpPost("v1/accounts/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool registrado = await _accountBL.RegisterAsync(registerDTO);
            if (!registrado)
            {
                return Conflict(new { mensaje = "El correo electrónico ya se encuentra registrado." });
            }

            return StatusCode(StatusCodes.Status201Created, new { mensaje = "Usuario registrado exitosamente." });
        }

        [HttpPost("v1/accounts/registerAdmin")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequestDTO registerDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool registrado = await _accountBL.RegisterAdminAsync(registerDTO);
            if (!registrado)
            {
                return Conflict(new { mensaje = "El correo electrónico ya se encuentra registrado." });
            }

            return StatusCode(StatusCodes.Status201Created, new { mensaje = "Usuario registrado exitosamente." });
        }

    }
}
