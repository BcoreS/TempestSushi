using TempestSushi.Infraestructure.Models;

public interface IRepositoryProcesoPreparacion
{
    Task<ICollection<ProcesoPreparacion>> FindByProductoIdAsync(
        int idProducto);

    Task<ICollection<ProcesoPreparacion>> ListAsync();

    Task CreateForProductoAsync(
        int idProducto,
        IEnumerable<ProcesoPreparacion> pasos);

    Task UpdateForProductoAsync(
        int idProducto,
        IEnumerable<ProcesoPreparacion> pasos);

    Task DeleteByProductoIdAsync(int idProducto);

    Task<bool> ExistsForProductoAsync(int idProducto);
}