using TempestSushi.Application.DTOs;

namespace TempestSushi.Application.Services.Interfaces
{
    public interface IServiceCombo
    {
        Task<ICollection<ComboDTO>> ListAsync();
        Task<ComboDTO> FindByIdAsync(int id);

        Task<ComboFormDto> ObtenerParaCrearAsync();
        Task<ComboFormDto?> ObtenerParaEditarAsync(int id);
        Task CargarListasAsync(ComboFormDto dto);
        Task<bool> ExisteNombreAsync(string nombre, int? idExcluir = null);
        Task CrearAsync(ComboFormDto dto);
        Task<bool> ActualizarAsync(ComboFormDto dto);
    }
}