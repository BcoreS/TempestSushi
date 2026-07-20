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

        public async Task DeleteAsync(int id)
        {
            var menu = await _context.Menus
                .Include(m => m.MenuProductos)
                .Include(m => m.MenuCombos)
                .FirstOrDefaultAsync(m => m.IdMenu == id);

            if (menu == null)
                return;

            _context.MenuProductos.RemoveRange(menu.MenuProductos);

            _context.MenuCombos.RemoveRange(menu.MenuCombos);

            _context.Menus.Remove(menu);

            await _context.SaveChangesAsync();
        }

        public async Task<Menu?> FindByIdAsync(int id)
        {
            return await _context.Menus
                .Include(m => m.MenuProductos)
                .Include(m => m.MenuCombos)
                .FirstOrDefaultAsync(m => m.IdMenu == id);
        }

        public async Task<Menu> CreateAsync(Menu menu)
        {
            _context.Menus.Add(menu);

            await _context.SaveChangesAsync();

            return menu;
        }



        public async Task UpdateWithRelationsAsync(
    Menu menu,
    IEnumerable<int> productosSeleccionados,
    IEnumerable<int> combosSeleccionados)
        {
            var menuExistente = await _context.Menus
                .Include(m => m.MenuProductos)
                .Include(m => m.MenuCombos)
                .FirstOrDefaultAsync(m => m.IdMenu == menu.IdMenu);

            if (menuExistente == null)
            {
                throw new KeyNotFoundException(
                    $"No se encontró el menú con identificador {menu.IdMenu}.");
            }

            menuExistente.Nombre = menu.Nombre;
            menuExistente.FechaInicio = menu.FechaInicio;
            menuExistente.FechaFin = menu.FechaFin;
            menuExistente.HoraInicio = menu.HoraInicio;
            menuExistente.HoraFin = menu.HoraFin;
            menuExistente.DiasDisponibles = menu.DiasDisponibles;
            menuExistente.Activo = menu.Activo;

            _context.MenuProductos.RemoveRange(menuExistente.MenuProductos);
            _context.MenuCombos.RemoveRange(menuExistente.MenuCombos);

            await _context.SaveChangesAsync();

            var nuevasRelacionesProductos = productosSeleccionados
                .Distinct()
                .Select(idProducto => new MenuProducto
                {
                    IdMenu = menuExistente.IdMenu,
                    IdProducto = idProducto,
                    Activo = true
                })
                .ToList();

            var nuevasRelacionesCombos = combosSeleccionados
                .Distinct()
                .Select(idCombo => new MenuCombo
                {
                    IdMenu = menuExistente.IdMenu,
                    IdCombo = idCombo,
                    Activo = true
                })
                .ToList();

            await _context.MenuProductos.AddRangeAsync(
                nuevasRelacionesProductos);

            await _context.MenuCombos.AddRangeAsync(
                nuevasRelacionesCombos);

            await _context.SaveChangesAsync();
        }
    }
}