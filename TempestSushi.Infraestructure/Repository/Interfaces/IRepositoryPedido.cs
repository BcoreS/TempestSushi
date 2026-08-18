using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryPedido
    {
        // Detalle completo de un pedido (con líneas, cliente, encargado, pago, etc.)
        Task<Pedido?> GetByIdAsync(int idPedido);

        // Historial del cliente logueado
        Task<List<Pedido>> GetByClienteAsync(int idCliente);

        // Historial completo para Encargado/Administrador, con filtros opcionales
        Task<List<Pedido>> GetTodosAsync(System.DateTime? fecha, int? idEstadoPedido);

        // Inserta el pedido completo (encabezado + líneas + pago) en una sola transacción
        Task<Pedido> CrearAsync(Pedido pedido);

        // Necesarios para el registro del formulario dinámico
        Task<EstadoPedido?> GetEstadoPorNombreAsync(string nombre);
        Task<List<Usuario>> GetClientesAsync(); // lista de clientes, para cuando el logueado es Encargado


        Task<List<MetodoEntrega>> GetMetodosEntregaAsync();
        Task<List<MetodoPago>> GetMetodosPagoAsync();

        Task<List<EstadoPedido>> GetEstadosAsync();


        Task<EstadoPedido?> GetEstadoPorIdAsync(int idEstadoPedido);
        Task GuardarCambiosAsync();

    }
}