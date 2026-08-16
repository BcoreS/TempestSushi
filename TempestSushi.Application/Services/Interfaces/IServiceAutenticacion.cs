using TempestSushi.Application.DTOs;

namespace TempestSushi.Application.Services.Interfaces
{
    public interface IServiceAutenticacion
    {
        Task<UsuarioAutenticadoDTO?> AutenticarAsync(
            string correo,
            string password);
    }
}