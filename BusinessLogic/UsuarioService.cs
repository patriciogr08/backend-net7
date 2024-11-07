using Microsoft.EntityFrameworkCore;
using MiBackendAPI.Data;
using MiBackendAPI.Models;
using MiBackendAPI.Requests;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;

namespace MiBackendAPI.BusinessLogic
{
    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }

        // Método para crear un nuevo usuario
        public async Task<Usuario> CreateUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> UpdateUsuarioAsync(int id, UpdateUsuario usuario)
        {
            var existingUsuario = await _context.Usuarios.FindAsync(id);
            if (existingUsuario == null) return null;

            existingUsuario.Nombre = usuario.Nombre;
            existingUsuario.Email = usuario.Email;

            var existingRol = await _context.Roles.FindAsync(usuario.IdRol);
            if (existingRol is null) throw new ValidationException("No existe el rol."); ;

            existingUsuario.IdRol = usuario.IdRol;

            await _context.SaveChangesAsync();
            return existingUsuario;
        }

        // Método para eliminar un usuario
        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
