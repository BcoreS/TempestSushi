using TempestSushi.Application.DTOs;

public interface IServiceProcesoPreparacion
{
    Task<ICollection<ProcesoPreparacionListDTO>> ListAsync();

    Task<ProcesoPreparacionDetalleDTO?>
        FindByProductoIdAsync(int idProducto);

    Task<ICollection<ProcesoPreparacionMantenimientoDTO>>
        ListMantenimientoAsync();

    Task<ProcesoPreparacionFormDTO> PrepararCrearAsync();

    Task PrepararFormularioAsync(
        ProcesoPreparacionFormDTO dto);

    Task CrearAsync(
        ProcesoPreparacionFormDTO dto);

    Task<ProcesoPreparacionFormDTO?>
        ObtenerParaEditarAsync(int idProducto);

    Task ActualizarAsync(
        ProcesoPreparacionFormDTO dto);

    Task EliminarAsync(int idProducto);

}