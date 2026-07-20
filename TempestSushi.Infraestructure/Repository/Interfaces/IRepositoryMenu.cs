using TempestSushi.Infraestructure.Models;

public interface IRepositoryMenu
{
    Task<ICollection<Menu>> ListConDetalleAsync();

    Task<Menu?> FindByIdAsync(int id);

    Task<Menu> CreateAsync(Menu menu);

    Task DeleteAsync(int id);

    Task UpdateWithRelationsAsync(
        Menu menu,
        IEnumerable<int> productosSeleccionados,
        IEnumerable<int> combosSeleccionados);
}