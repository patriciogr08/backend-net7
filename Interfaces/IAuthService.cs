using MiBackendAPI.Models;

public interface IAuthService
{
    Task RegisterAsync(Usuario usuario);
    Task<string> Authenticate(string username, string password);
}