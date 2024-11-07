using MiBackendAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRolService
{
    Task<IEnumerable<Rol>> GetRolesAsync();
    Task<Rol> GetRolByIdAsync(int id);
    Task<Rol> CreateRolAsync(Rol rol);
    Task<Rol> UpdateRolAsync(int id, Rol rol);
    Task<bool> DeleteRolAsync(int id);
}
