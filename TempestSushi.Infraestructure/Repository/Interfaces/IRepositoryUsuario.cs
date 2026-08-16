using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryUsuario
    {
        Task<Usuario?> FindByCorreoAsync(string correo);
    }
}