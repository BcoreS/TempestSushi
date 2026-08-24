using TempestSushi.Application.DTOs;

namespace TempestSushi.Application.Services.Interfaces
{
    public interface IServiceUsuario
    {
        Task<List<UsuarioListadoDTO>> ObtenerListadoAsync();

        Task<List<RolUsuarioDTO>> ObtenerRolesActivosAsync();

        Task<bool> CrearAsync(
            UsuarioCrearDTO dto);

        Task<UsuarioEditarDTO?> ObtenerParaEditarAsync(
            int idUsuario);

        Task<bool> EditarAsync(
            UsuarioEditarDTO dto);

        Task<bool> CambiarEstadoAsync(
            int idUsuario);
    }
}