using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] object loginDto) 
        {
            bool credencialesValidas = true; 

            if (!credencialesValidas)
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });
            }

            string tokenSimulado = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";

            return Ok(new { Token = tokenSimulado });
        }
    }
}
