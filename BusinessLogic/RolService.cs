using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiBackendAPI.Models;
using MiBackendAPI.Data;

namespace MiBackendAPI.BusinessLogic
{
    public class RolService : IRolService
    {
        private readonly ApplicationDbContext _context;

        public RolService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> GetRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Rol> GetRolByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Rol> CreateRolAsync(Rol rol)
        {
            await _context.Roles.AddAsync(rol);
            await _context.SaveChangesAsync();
            return rol;
        }

        public async Task<Rol> UpdateRolAsync(int id, Rol rol)
        {
            var existingRol = await _context.Roles.FindAsync(id);
            if (existingRol == null) return null;

            existingRol.Nombre = rol.Nombre;
            _context.Roles.Update(existingRol);
            await _context.SaveChangesAsync();
            return existingRol;
        }

        public async Task<bool> DeleteRolAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return false;

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
