using Microsoft.AspNetCore.Mvc;
using MiBackendAPI.BusinessLogic;
using MiBackendAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.ComponentModel.DataAnnotations;
using MiBackendAPI.Requests;

namespace MiBackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // Obtener todos los usuarios
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<List<Usuario>>))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _usuarioService.GetUsuariosAsync();

            var response = new SuccessResponse<IEnumerable<Usuario>>(
                (int)HttpStatusCode.OK,
                "Usuarios obtenidos exitosamente.",
                usuarios
            );
            return Ok(response);
        }

        // Obtener un usuario por ID
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<Usuario>))]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
            if (usuario == null)
                return NotFound();

            var response = new SuccessResponse<Usuario>(
               (int)HttpStatusCode.OK,
               "Usuario obtenido exitosamente.",
               usuario
            );

            return Ok(response);
        }

        // Crear un nuevo usuario
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SuccessResponse<Usuario>))]
        public async Task<IActionResult> CreateUsuario([FromBody] Usuario usuario)
        {
            var newUsuario = await _usuarioService.CreateUsuarioAsync(usuario);

            var response = new SuccessResponse<Usuario>(
                (int)HttpStatusCode.Created,
                "Usuario creado exitosamente.",
                newUsuario
            );

            return CreatedAtAction(nameof(GetUsuario), new { id = newUsuario.Id }, response);
        }

        // Actualizar un usuario
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SuccessResponse<Usuario>))]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UpdateUsuario usuario)
        {
            var updatedUsuario = await _usuarioService.UpdateUsuarioAsync(id, usuario);
            if (updatedUsuario == null)
                return NotFound();

            var response = new SuccessResponse<Usuario>(
                (int)HttpStatusCode.OK,
                "Usuario actualizado exitosamente.",
                updatedUsuario
            );

            return Ok(response);
        }

        // Eliminar un usuario
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var result = await _usuarioService.DeleteUsuarioAsync(id);
            if (!result)
                return NotFound();

            var response = new SuccessResponse<object>(
                (int)HttpStatusCode.NoContent,
                "Usuario eliminado exitosamente."
            );
            return NoContent();
        }
    }
}
