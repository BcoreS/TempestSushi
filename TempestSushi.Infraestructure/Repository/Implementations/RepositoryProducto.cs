using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Infraestructure.Repository.Implementations
{
    public class RepositoryProducto : IRepositoryProducto
    {
        private readonly TempestSushiDbContext _context;

        public RepositoryProducto(TempestSushiDbContext context)
        {
            _context = context;
        }

        public async Task<Producto?> FindByIdAsync(int id)
        {
            return await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.ImagenProductos)
                .Include(p => p.IdIngredientes)
                .FirstOrDefaultAsync(p => p.IdProducto == id);
        }

        public async Task<ICollection<Producto>> ListAsync()
        {
            return await _context.Productos
                .Where(p => p.Activo == true)
                .Include(p => p.IdCategoriaNavigation)
                .Include(p => p.ImagenProductos)
                .OrderBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<ICollection<Categorium>> ListCategoriasActivasAsync()
        {
            return await _context.Categoria
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<ICollection<Ingrediente>> ListIngredientesActivosAsync()
        {
            return await _context.Ingredientes
                .Where(i => i.Activo)
                .OrderBy(i => i.Nombre)
                .ToListAsync();
        }

        public async Task<ICollection<Ingrediente>> ListIngredientesByIdsAsync(List<int> ids)
        {
            return await _context.Ingredientes
                .Where(i => ids.Contains(i.IdIngrediente))
                .ToListAsync();
        }

        public async Task<bool> ExisteNombreAsync(string nombre, int? idExcluir = null)
        {
            return await _context.Productos.AnyAsync(p =>
                p.Nombre.ToLower() == nombre.ToLower() && (idExcluir == null || p.IdProducto != idExcluir));
        }

        public async Task AddAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producto producto)
        {
            // La entidad ya viene rastreada por el contexto (se obtuvo con FindByIdAsync)
            await _context.SaveChangesAsync();
        }
    }
}