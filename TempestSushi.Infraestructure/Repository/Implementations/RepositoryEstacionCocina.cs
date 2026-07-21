using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Infraestructure.Repository.Implementations
{
    public class RepositoryEstacionCocina : IRepositoryEstacionCocina
    {
        private readonly TempestSushiDbContext _context;

        public RepositoryEstacionCocina(TempestSushiDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<EstacionCocina>> ListAsync()
        {
            return await _context
                .Set<EstacionCocina>()
                .Where(e => e.Activo)
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }
    }
}
