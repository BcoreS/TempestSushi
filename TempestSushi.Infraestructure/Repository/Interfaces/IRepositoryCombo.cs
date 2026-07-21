using System.Collections.Generic;
using System.Threading.Tasks;
using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCombo
    {
        Task<Combo> FindByIdAsync(int id);
        Task<ICollection<Combo>> ListAsync();
        Task<ICollection<Producto>> ListProductosActivosAsync();
        Task<int> ObtenerIdCategoriaComboAsync();
        Task<bool> ExisteNombreAsync(string nombre, int? idExcluir = null);
        Task AddAsync(Combo combo);
        Task UpdateAsync(Combo combo, List<int> productosSeleccionados);
    }
}