using Microsoft.AspNetCore.Mvc;
using MiBackendAPI.Models;
using MiBackendAPI.BusinessLogic;

namespace MiBackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authServices)
        {
            _authService = authServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Usuario usuario)
        {
            if (usuario == null)
                return BadRequest("Datos de usuario inválidos.");

            try
            {
                await _authService.RegisterAsync(usuario);
                return Ok("Usuario registrado exitosamente.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al registrar el usuario: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Credenciales inválidas.");
            }

            try
            {
                var token = await _authService.Authenticate(loginRequest.Username, loginRequest.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }
        }
    }
}
