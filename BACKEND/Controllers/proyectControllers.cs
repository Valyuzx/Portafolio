
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectosController : ControllerBase
    {
        public ProyectosController()
        {
        }

        // GET: api/proyectos
        [HttpGet]
        [AllowAnonymous] // Cualquiera puede ver tus proyectos
        public async Task<IActionResult> GetProyectos()
        {
            return Ok(new { mensaje = "Lista de proyectos" });
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProyecto(int id)
        {
            return Ok(new { mensaje = $"Detalle del proyecto {id}" });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProyecto([FromBody] object proyectoDto)
        {
            return CreatedAtAction(nameof(GetProyecto), new { id = 1 }, proyectoDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProyecto(int id, [FromBody] object proyectoDto)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProyecto(int id)
        {
            return NoContent();
        }
    }
}