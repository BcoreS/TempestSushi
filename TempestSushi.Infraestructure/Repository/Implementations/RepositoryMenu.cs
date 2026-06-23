using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Infraestructure.Repository.Implementations
{
    public class RepositoryMenu : IRepositoryMenu
    {
        private readonly TempestSushiDbContext _context;

        public RepositoryMenu(TempestSushiDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Menu>> ListConDetalleAsync()
        {
            return await _context.Menus
                .Where(m => m.Activo == true)
                .Include(m => m.MenuProductos)
                    .ThenInclude(mp => mp.IdProductoNavigation)
                        .ThenInclude(p => p.IdCategoriaNavigation)
                .Include(m => m.MenuCombos)
                    .ThenInclude(mc => mc.IdComboNavigation)
                        .ThenInclude(c => c.IdCategoriaNavigation)
                .ToListAsync();
        }
    }
}