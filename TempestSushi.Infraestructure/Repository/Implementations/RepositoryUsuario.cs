using Microsoft.EntityFrameworkCore;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Infraestructure.Repository.Implementations
{
    public class RepositoryUsuario : IRepositoryUsuario
    {
        private readonly TempestSushiDbContext _context;

        public RepositoryUsuario(
            TempestSushiDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> FindByCorreoAsync(
            string correo)
        {
            return await _context.Usuarios
                .Include(u => u.IdRolUsuarioNavigation)
                .FirstOrDefaultAsync(
                    u => u.Correo == correo);
        }

        public async Task<bool> ExisteCorreoAsync(
            string correo)
        {
            return await _context.Usuarios
                .AnyAsync(
                    u => u.Correo == correo);
        }

        public async Task<RolUsuario?> FindRolByNombreAsync(
            string nombre)
        {
            return await _context.RolUsuarios
                .FirstOrDefaultAsync(
                    r => r.Nombre == nombre &&
                         r.Activo);
        }

        public async Task<Usuario> CrearAsync(
            Usuario usuario)
        {
            _context.Usuarios.Add(usuario);

            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<List<Usuario>> ListAsync()
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Include(u => u.IdRolUsuarioNavigation)
                .OrderBy(u => u.Nombre)
                .ThenBy(u => u.Apellidos)
                .ToListAsync();
        }

        public async Task<Usuario?> FindByIdAsync(
            int idUsuario)
        {
            return await _context.Usuarios
                .Include(u => u.IdRolUsuarioNavigation)
                .FirstOrDefaultAsync(
                    u => u.IdUsuario == idUsuario);
        }

        public async Task<List<RolUsuario>> ListRolesActivosAsync()
        {
            return await _context.RolUsuarios
                .AsNoTracking()
                .Where(r => r.Activo)
                .OrderBy(r => r.Nombre)
                .ToListAsync();
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}