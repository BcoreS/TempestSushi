using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUsuario
    {
        Task<Usuario?> FindByCorreoAsync(string correo);

        Task<bool> ExisteCorreoAsync(string correo);

        Task<RolUsuario?> FindRolByNombreAsync(string nombre);

        Task<Usuario> CrearAsync(Usuario usuario);

        Task<List<Usuario>> ListAsync();

        Task<Usuario?> FindByIdAsync(int idUsuario);

        Task<List<RolUsuario>> ListRolesActivosAsync();

        Task GuardarCambiosAsync();
    }
}