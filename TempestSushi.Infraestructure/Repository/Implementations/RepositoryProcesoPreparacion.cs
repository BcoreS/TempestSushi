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

        public async Task<ProcesoPreparacion> FindByIdAsync(int id)
        {
            var entity = await _context.Set<ProcesoPreparacion>()
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.IdEstacionCocinaNavigation)
                .FirstOrDefaultAsync(p => p.IdProcesoPreparacion == id);
            return entity;

            //Revisar si es null en la logica
        }

        public async Task<ICollection<ProcesoPreparacion>> ListAsync()
        {
            var collection = await _context.Set<ProcesoPreparacion>()
                .Include(p => p.IdProductoNavigation)
                .Include(p => p.IdEstacionCocinaNavigation)
                .ToListAsync();

            return collection;
        }
    }
}
