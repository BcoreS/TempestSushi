using System.Collections.Generic;
using System.Threading.Tasks;
using TempestSushi.Application.DTOs;

namespace TempestSushi.Application.Services.Interfaces
{
    public interface IProductoService
    {
        Task<List<ProductoDto>> ObtenerListadoAsync();
        Task<ProductoDetalleDto?> ObtenerDetalleAsync(int id);

        Task<ProductoFormDto> ObtenerParaCrearAsync();
        Task<ProductoFormDto?> ObtenerParaEditarAsync(int id);
        Task CargarListasAsync(ProductoFormDto dto);
        Task<bool> ExisteNombreAsync(string nombre, int? idExcluir = null);
        Task CrearAsync(ProductoFormDto dto, string? imagenUrl);
        Task<bool> ActualizarAsync(ProductoFormDto dto, string? imagenUrl);
    }
}