using MiBackendAPI.Data;
using MiBackendAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiBackendAPI.BusinessLogic
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task RegisterAsync(Usuario usuario)
        {
            // Verificar si el nombre de usuario ya existe
            var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.User == usuario.User);
            if (existingUser != null)
            {
                throw new Exception("El nombre de usuario ya está en uso.");
            }

            // Asegúrate de hashear la contraseña antes de guardarla
            usuario.Password = HashPassword(usuario.Password);

            // Agregar el usuario a la base de datos
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }


        public async Task<string> Authenticate(string username, string password)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.User == username);
            if (user == null || !VerifyPassword(password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            // Generar el token JWT
            var token = GenerateJwtToken(user);
            return token;
        }

        // Método para generar el token JWT
        public string GenerateJwtToken(Usuario user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.User),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var notBefore = now;
            var expires = now.AddDays(1);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                NotBefore = notBefore,
                SigningCredentials = creds,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedPassword);
        }

    }
}
