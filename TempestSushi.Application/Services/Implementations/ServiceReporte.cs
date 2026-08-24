using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Application.Services.Implementations
{
    public class ServiceReporte : IServiceReporte
    {
        private readonly IRepositoryReporte _repositoryReporte;

        public ServiceReporte(
            IRepositoryReporte repositoryReporte)
        {
            _repositoryReporte = repositoryReporte;
        }

        public async Task<ReporteDashboardDTO>
            ObtenerDashboardAsync()
        {
            var pedidos =
                await _repositoryReporte
                    .ListPedidosParaReporteAsync();

            var hoy = DateTime.Today;
            var fechaInicio = hoy.AddDays(-6);

            var pedidosUltimosSieteDias = pedidos
                .Where(p =>
                    p.FechaPedido.Date >= fechaInicio
                    && p.FechaPedido.Date <= hoy)
                .ToList();

            var pedidosAgrupadosPorFecha =
                pedidosUltimosSieteDias
                    .GroupBy(p => p.FechaPedido.Date)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Count());

            var pedidosPorDia =
                Enumerable.Range(0, 7)
                    .Select(i =>
                    {
                        var fecha =
                            fechaInicio.AddDays(i);

                        return new PedidoPorDiaDTO
                        {
                            Fecha = fecha,

                            CantidadPedidos =
                                pedidosAgrupadosPorFecha
                                    .TryGetValue(
                                        fecha,
                                        out var cantidad)
                                    ? cantidad
                                    : 0
                        };
                    })
                    .ToList();

            var productosMasVendidos = pedidos
    .Where(p => p.IdEstadoPedido != 6)
    .SelectMany(p =>
        p.PedidoDetalleProductos)
    .GroupBy(dp => new
    {
        dp.IdProducto,
        dp.IdProductoNavigation.Nombre
    })
    .Select(g =>
        new ProductoVendidoDTO
        {
            IdProducto =
                g.Key.IdProducto,

            NombreProducto =
                g.Key.Nombre,

            CantidadVendida =
                g.Sum(dp => dp.Cantidad)
        })
    .OrderByDescending(p =>
        p.CantidadVendida)
    .ThenBy(p =>
        p.NombreProducto)
    .Take(5)
    .ToList();

            var pedidosPorEstado = pedidos
                .GroupBy(p =>
                    p.IdEstadoPedidoNavigation.Nombre)
                .Select(g =>
                    new PedidoPorEstadoDTO
                    {
                        Estado = g.Key,

                        CantidadPedidos =
                            g.Count()
                    })
                .OrderByDescending(p =>
                    p.CantidadPedidos)
                .ThenBy(p =>
                    p.Estado)
                .ToList();

            return new ReporteDashboardDTO
            {
                PedidosPorDia =
                    pedidosPorDia,

                ProductosMasVendidos =
                    productosMasVendidos,

                PedidosPorEstado =
                    pedidosPorEstado
            };
        }
    }
}