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

        public async Task<Combo> FindByIdAsync(int id)
        {
            var entity = await _context.Set<Combo>()
                .Include(c => c.IdCategoriaNavigation)
                .Include(c => c.ComboProductos)
                    .ThenInclude(cp => cp.IdProductoNavigation)
                .FirstOrDefaultAsync(c => c.IdCombo == id);

            return entity;
        }

        public async Task<ICollection<Combo>> ListAsync()
        {
            var collection = await _context.Set<Combo>()
                .Include(c => c.IdCategoriaNavigation)
                .Include(c => c.ComboProductos)
                    .ThenInclude(cp => cp.IdProductoNavigation)
                .ToListAsync();

            return collection;
        }
    }
}