using Microsoft.EntityFrameworkCore;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Infraestructure.Repository.Interfaces
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly TempestSushiDbContext _context;

        public RepositoryUsuario(TempestSushiDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> FindByCorreoAsync(string correo)
        {
            return await _context.Usuarios
                .Include(u => u.IdRolUsuarioNavigation)
                .FirstOrDefaultAsync(u => u.Correo == correo);
        }
    }
}