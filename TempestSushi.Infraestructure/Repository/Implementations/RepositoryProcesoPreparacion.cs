using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace TempestSushi.Infraestructure.Repository.Implementations
{
    public class RepositoryProcesoPreparacion : IRepositoryProcesoPreparacion
    {
        private readonly TempestSushiDbContext _context;

        public RepositoryProcesoPreparacion(TempestSushiDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ProcesoPreparacion>> FindByProductoIdAsync(int idProducto)
        {
            return await _context.Set<ProcesoPreparacion>()
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.IdEstacionCocinaNavigation)
                .Where(p => p.IdProducto == idProducto)
                .OrderBy(p => p.NumeroPaso)
                .ToListAsync();
        }

        public async Task<ICollection<ProcesoPreparacion>> ListAsync()
        {
            return await _context.Set<ProcesoPreparacion>()
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.IdEstacionCocinaNavigation)
                .ToListAsync();
        }
    
    }
}
