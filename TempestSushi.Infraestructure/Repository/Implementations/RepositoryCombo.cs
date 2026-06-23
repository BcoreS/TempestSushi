    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;


using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Infraestructure.Repository.Implementations
{
    public class RepositoryCombo : IRepositoryCombo
    {
        private readonly TempestSushiDbContext  _context;

        public RepositoryCombo(TempestSushiDbContext context)
        {
            _context = context;
        }

        public Task<Combo> FindByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Combo>> ListAsync()
        {
            //Select * from Autor
            var collection = await _context.Set<Combo>().ToListAsync();
            return collection;
        }

    }
}
