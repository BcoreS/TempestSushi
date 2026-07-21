using System.Collections.Generic;
using System.Threading.Tasks;
using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryProducto
    {
        Task<Producto?> FindByIdAsync(int id);
        Task<ICollection<Producto>> ListAsync();
        Task<ICollection<Categorium>> ListCategoriasActivasAsync();
        Task<ICollection<Ingrediente>> ListIngredientesActivosAsync();
        Task<ICollection<Ingrediente>> ListIngredientesByIdsAsync(List<int> ids);
        Task<bool> ExisteNombreAsync(string nombre, int? idExcluir = null);
        Task AddAsync(Producto producto);
        Task UpdateAsync(Producto producto);
    }
}