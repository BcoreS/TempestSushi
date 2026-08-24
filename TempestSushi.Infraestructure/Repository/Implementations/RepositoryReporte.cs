using Microsoft.EntityFrameworkCore;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Infraestructure.Repository.Implementations
{
    public class RepositoryReporte : IRepositoryReporte
    {
        private readonly TempestSushiDbContext _context;

        public RepositoryReporte(
            TempestSushiDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Pedido>>
            ListPedidosParaReporteAsync()
        {
            return await _context.Pedidos
                .AsNoTracking()

                .Include(p =>
                    p.IdEstadoPedidoNavigation)

                .Include(p =>
                    p.PedidoDetalleProductos)

                    .ThenInclude(dp =>
                        dp.IdProductoNavigation)

                .OrderBy(p =>
                    p.FechaPedido)

                .ToListAsync();
        }
    }
}