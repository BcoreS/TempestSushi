using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TempestSushi.Application.DTOs;

namespace TempestSushi.Application.Services.Interfaces
{
    public interface IServicePedido
    {
        // Historial: filtra automáticamente según el rol del usuario actual
        // (Cliente -> solo los suyos; Encargado/Administrador -> todos, con filtros opcionales)
        Task<List<PedidoHistorialDto>> ObtenerHistorialAsync(DateTime? fecha, int? idEstadoPedido);

        // Detalle completo en formato factura
        Task<PedidoDTO?> ObtenerDetalleAsync(int idPedido);

        // Recalcula precio/subtotal/impuesto de una sola línea (llamado por fetch desde el formulario)
        Task<PedidoLineaDto> CalcularLineaAsync(PedidoLineaEntradaDto entrada);

        // Lista de clientes activos, para cuando el usuario logueado es Encargado
        Task<List<PedidoClienteOpcionDto>> ObtenerClientesAsync();

        // Registro final: arma el Pedido completo y lo persiste en una sola transacción
        Task<PedidoDTO> RegistrarAsync(PedidoRegistroDto registro);

        Task<PedidoFormularioDto> ObtenerDatosFormularioAsync();



        Task<List<PedidoMetodoOpcionDto>> ObtenerEstadosAsync();
    }
}