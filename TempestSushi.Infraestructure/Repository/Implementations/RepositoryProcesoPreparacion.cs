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

        public async Task CreateForProductoAsync(
    int idProducto,
    IEnumerable<ProcesoPreparacion> pasos)
        {
            var nuevasEntidades = pasos
                .Select(p => new ProcesoPreparacion
                {
                    IdProducto = idProducto,
                    IdEstacionCocina = p.IdEstacionCocina,
                    NumeroPaso = p.NumeroPaso,
                    DescripcionPaso = p.DescripcionPaso,
                    TiempoEstimadoMinutos = p.TiempoEstimadoMinutos
                })
                .ToList();

            await _context.ProcesoPreparacions.AddRangeAsync(nuevasEntidades);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateForProductoAsync(
    int idProducto,
    IEnumerable<ProcesoPreparacion> pasos)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var pasosExistentes = await _context
                    .Set<ProcesoPreparacion>()
                    .Where(p => p.IdProducto == idProducto)
                    .ToListAsync();

                _context.Set<ProcesoPreparacion>()
                    .RemoveRange(pasosExistentes);

                await _context.SaveChangesAsync();

                var nuevosPasos = pasos
                    .Select(p => new ProcesoPreparacion
                    {
                        IdProducto = idProducto,
                        IdEstacionCocina = p.IdEstacionCocina,
                        NumeroPaso = p.NumeroPaso,
                        DescripcionPaso = p.DescripcionPaso,
                        TiempoEstimadoMinutos = p.TiempoEstimadoMinutos
                    })
                    .ToList();

                await _context.Set<ProcesoPreparacion>()
                    .AddRangeAsync(nuevosPasos);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteByProductoIdAsync(int idProducto)
        {
            var pasos = await _context
                .Set<ProcesoPreparacion>()
                .Where(p => p.IdProducto == idProducto)
                .ToListAsync();

            if (pasos.Count == 0)
            {
                return;
            }

            _context.Set<ProcesoPreparacion>().RemoveRange(pasos);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsForProductoAsync(int idProducto)
        {
            return await _context
                .Set<ProcesoPreparacion>()
                .AnyAsync(p => p.IdProducto == idProducto);
        }
    }
}
