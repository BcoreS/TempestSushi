using Microsoft.EntityFrameworkCore;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;
namespace TempestSushi.Infraestructure.Repository.Implementations
{
    public class RepositoryCombo : IRepositoryCombo
    {
        private readonly TempestSushiDbContext _context;
        public RepositoryCombo(TempestSushiDbContext context)
        {
            _context = context;
        }
        public async Task<Combo?> FindByIdAsync(int id)
        {
            return await _context.Set<Combo>()
                .Include(c => c.IdCategoriaNavigation)
                .Include(c => c.ComboProductos)
                    .ThenInclude(cp => cp.IdProductoNavigation)
                .FirstOrDefaultAsync(c => c.IdCombo == id);
        }
        public async Task<ICollection<Combo>> ListAsync()
        {
            return await _context.Set<Combo>()
                .Include(c => c.IdCategoriaNavigation)
                .Include(c => c.ComboProductos)
                    .ThenInclude(cp => cp.IdProductoNavigation)
                .ToListAsync();
        }
        public async Task<ICollection<Producto>> ListProductosActivosAsync()
        {
            return await _context.Productos
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }
        public async Task<int> ObtenerIdCategoriaComboAsync()
        {
            var categoria = await _context.Categoria.FirstOrDefaultAsync(c => c.Nombre == "Combos");
            return categoria?.IdCategoria ?? (await _context.Categoria.Select(c => c.IdCategoria).FirstAsync());
        }
        public async Task<bool> ExisteNombreAsync(string nombre, int? idExcluir = null)
        {
            return await _context.Combos.AnyAsync(c =>
                c.Nombre.ToLower() == nombre.ToLower() && (idExcluir == null || c.IdCombo != idExcluir));
        }
        public async Task AddAsync(Combo combo)
        {
            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Combo combo, List<int> productosSeleccionados)
        {
            // Productos actuales en la base de datos para este combo
            var actuales = await _context.Set<ComboProducto>()
                .Where(cp => cp.IdCombo == combo.IdCombo)
                .ToListAsync();

            // Eliminar los que ya no están seleccionados
            var aEliminar = actuales
                .Where(cp => !productosSeleccionados.Contains(cp.IdProducto))
                .ToList();
            if (aEliminar.Any())
                _context.Set<ComboProducto>().RemoveRange(aEliminar);

            // Agregar los nuevos que no existían antes
            var idsActuales = actuales.Select(cp => cp.IdProducto).ToList();
            var aAgregar = productosSeleccionados
                .Where(id => !idsActuales.Contains(id))
                .Select(id => new ComboProducto { IdCombo = combo.IdCombo, IdProducto = id, Cantidad = 1 });

            await _context.Set<ComboProducto>().AddRangeAsync(aAgregar);

            await _context.SaveChangesAsync();
        }
    }
}