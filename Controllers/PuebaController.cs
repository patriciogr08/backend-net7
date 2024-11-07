using Microsoft.AspNetCore.Mvc;

namespace MiBackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PuebaController : ControllerBase
    {
        [HttpGet("saludo")]
        public IActionResult ObtenerSaludo()
        {
            return Ok("¡Hola desde el backend en .NET!");
        }
    }
}
