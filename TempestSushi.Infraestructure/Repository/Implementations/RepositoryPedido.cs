using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TempestSushi.Infraestructure.Data;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Infraestructure.Repository.Implementations
{
    public class RepositoryPedido : IRepositoryPedido
    {
        private readonly TempestSushiDbContext _context;

        public RepositoryPedido(TempestSushiDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido?> GetByIdAsync(int idPedido)
        {
            return await _context.Pedidos
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdEmpleadoNavigation)
                .Include(p => p.IdEstadoPedidoNavigation)
                .Include(p => p.IdMetodoEntregaNavigation)
                .Include(p => p.Pagos)
                    .ThenInclude(pa => pa.IdMetodoPagoNavigation)
                .Include(p => p.PedidoDetalleProductos)
                    .ThenInclude(d => d.IdProductoNavigation)
                .Include(p => p.PedidoDetalleCombos)
                    .ThenInclude(d => d.IdComboNavigation)
                .FirstOrDefaultAsync(p => p.IdPedido == idPedido);
        }

        public async Task<List<Pedido>> GetByClienteAsync(int idCliente)
        {
            return await _context.Pedidos
                .Include(p => p.IdEstadoPedidoNavigation)
                .Include(p => p.IdMetodoEntregaNavigation)
                .Where(p => p.IdCliente == idCliente)
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();
        }

        public async Task<List<Pedido>> GetTodosAsync(DateTime? fecha, int? idEstadoPedido)
        {
            var query = _context.Pedidos
                .Include(p => p.IdClienteNavigation)
                .Include(p => p.IdEstadoPedidoNavigation)
                .Include(p => p.IdMetodoEntregaNavigation)
                .AsQueryable();

            if (fecha.HasValue)
                query = query.Where(p => p.FechaPedido.Date == fecha.Value.Date);

            if (idEstadoPedido.HasValue)
                query = query.Where(p => p.IdEstadoPedido == idEstadoPedido.Value);

            return await query
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();
        }

        public async Task<Pedido> CrearAsync(Pedido pedido)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return pedido;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<EstadoPedido?> GetEstadoPorNombreAsync(string nombre)
        {
            return await _context.EstadoPedidos
                .FirstOrDefaultAsync(e => e.Nombre == nombre);
        }

        public async Task<List<Usuario>> GetClientesAsync()
        {
            return await _context.Usuarios
                .Where(u => u.IdRolUsuarioNavigation.Nombre == "Cliente" && u.Activo)
                .OrderBy(u => u.Nombre)
                .ToListAsync();
        }

        public async Task<List<MetodoEntrega>> GetMetodosEntregaAsync()
        {
            return await _context.MetodoEntregas.ToListAsync();
        }

        public async Task<List<MetodoPago>> GetMetodosPagoAsync()
        {
            return await _context.MetodoPagos.ToListAsync();
        }


        public async Task<List<EstadoPedido>> GetEstadosAsync()
        {
            return await _context.EstadoPedidos.ToListAsync();
        }


        public async Task<EstadoPedido?> GetEstadoPorIdAsync(int idEstadoPedido)
        {
            return await _context.EstadoPedidos
                .FirstOrDefaultAsync(e => e.IdEstadoPedido == idEstadoPedido);
        }

        public async Task GuardarCambiosAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}