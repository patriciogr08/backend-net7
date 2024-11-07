using MiBackendAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MiBackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _rolService.GetRolesAsync();
            var response = new SuccessResponse<IEnumerable<Rol>>(
                    (int)HttpStatusCode.OK,
                    "Roles obtenidos exitosamente.",
                    roles
             );
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRol(int id)
        {
            var rol = await _rolService.GetRolByIdAsync(id);
            if (rol == null) return NotFound();

            var response = new SuccessResponse<Rol>(
                 (int)HttpStatusCode.OK,
                 "Rol obtenido exitosamente.",
                 rol
             );
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRol([FromBody] Rol rol)
        {
            var newRol = await _rolService.CreateRolAsync(rol);
            var response = new SuccessResponse<Rol>(
                (int)HttpStatusCode.Created,
                "Rol creado exitosamente.",
                newRol
            );

            return CreatedAtAction(nameof(GetRol), new { id = newRol.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] Rol rol)
        {
            var updatedRol = await _rolService.UpdateRolAsync(id, rol);
            if (updatedRol == null) return NotFound();
            
            var response = new SuccessResponse<Rol>(
                (int)HttpStatusCode.OK,
                "Rol actualizado exitosamente.",
                updatedRol
            );
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var result = await _rolService.DeleteRolAsync(id);
            if (!result) return NotFound();

            var response = new SuccessResponse<object>(
                (int)HttpStatusCode.NoContent,
                "Rol eliminado exitosamente."
            );
            return NoContent();
        }
    }
}
