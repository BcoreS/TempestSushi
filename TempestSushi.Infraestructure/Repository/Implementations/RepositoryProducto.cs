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
    }
}