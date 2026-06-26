using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactoController : ControllerBase
    {
        public ContactoController()
        {
        }

        [HttpPost]
        [AllowAnonymous] 
        public async Task<IActionResult> EnviarMensaje([FromBody] object contactoDto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(new { mensaje = "Mensaje enviado correctamente. Te contactaré pronto." });
        }
    }
}
