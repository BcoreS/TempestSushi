using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Options;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Application.Services.Implementations
{
    public class ServicePedido : IServicePedido
    {
        private readonly IRepositoryPedido _repositoryPedido;
        private readonly IRepositoryProducto _repositoryProducto;
        private readonly IRepositoryCombo _repositoryCombo;
        private readonly IUsuarioActualService _usuarioActual;
        private readonly IMapper _mapper;
        private readonly ImpuestosOptions _impuestosOptions;
        private readonly EnvioOptions _envioOptions;

        private const string ESTADO_INICIAL = "Pendiente de pago";
        private const string METODO_ENTREGA_DOMICILIO = "Entrega a domicilio";

        public ServicePedido(
            IRepositoryPedido repositoryPedido,
            IRepositoryProducto repositoryProducto,
            IRepositoryCombo repositoryCombo,
            IUsuarioActualService usuarioActual,
            IMapper mapper,
            IOptions<ImpuestosOptions> impuestosOptions,
            IOptions<EnvioOptions> envioOptions)
        {
            _repositoryPedido = repositoryPedido;
            _repositoryProducto = repositoryProducto;
            _repositoryCombo = repositoryCombo;
            _usuarioActual = usuarioActual;
            _mapper = mapper;
            _impuestosOptions = impuestosOptions.Value;
            _envioOptions = envioOptions.Value;
        }

        // ---------- HISTORIAL ----------
        public async Task<List<PedidoHistorialDto>> ObtenerHistorialAsync(DateTime? fecha, int? idEstadoPedido)
        {
            if (!_usuarioActual.EstaAutenticado || _usuarioActual.IdUsuario is null)
                return new List<PedidoHistorialDto>();

            List<Pedido> pedidos;

            if (_usuarioActual.Rol == "Cliente")
            {
                // El cliente solo ve los suyos - la variable de usuario actual decide, no la interfaz
                pedidos = await _repositoryPedido.GetByClienteAsync(_usuarioActual.IdUsuario.Value);
            }
            else
            {
                // Encargado / Administrador ven todo, con filtros opcionales
                pedidos = await _repositoryPedido.GetTodosAsync(fecha, idEstadoPedido);
            }

            return _mapper.Map<List<PedidoHistorialDto>>(pedidos);
        }

        // ---------- DETALLE ----------
        public async Task<PedidoDTO?> ObtenerDetalleAsync(int idPedido)
        {
            var pedido = await _repositoryPedido.GetByIdAsync(idPedido);
            if (pedido is null) return null;

            // Un cliente solo puede ver el detalle de sus propios pedidos
            if (_usuarioActual.Rol == "Cliente" && pedido.IdCliente != _usuarioActual.IdUsuario)
                return null;

            var dto = _mapper.Map<PedidoDTO>(pedido);

            var lineasProducto = pedido.PedidoDetalleProductos.Select(d => MapearLineaProducto(d));
            var lineasCombo = pedido.PedidoDetalleCombos.Select(d => MapearLineaCombo(d));
            dto.Lineas = lineasProducto.Concat(lineasCombo).ToList();

            // Los totales ya se calcularon y persistieron al registrar el pedido - se usan tal cual
            dto.TotalSinImpuestos = pedido.Subtotal;
            dto.TotalConImpuestos = pedido.Total;

            return dto;
        }

        private PedidoLineaDto MapearLineaProducto(PedidoDetalleProducto d)
        {
            var subtotal = d.PrecioUnitario * d.Cantidad;
            var impuesto = subtotal * _impuestosOptions.TasaIva;
            return new PedidoLineaDto
            {
                Tipo = "Producto",
                IdItem = d.IdProducto,
                Nombre = d.IdProductoNavigation.Nombre,
                PrecioUnitario = d.PrecioUnitario,
                Cantidad = d.Cantidad,
                Subtotal = subtotal,
                Impuesto = impuesto,
                TotalLinea = subtotal + impuesto,
                Observaciones = d.Observaciones
            };
        }

        private PedidoLineaDto MapearLineaCombo(PedidoDetalleCombo d)
        {
            var subtotal = d.PrecioUnitario * d.Cantidad;
            var impuesto = subtotal * _impuestosOptions.TasaIva;
            return new PedidoLineaDto
            {
                Tipo = "Combo",
                IdItem = d.IdCombo,
                Nombre = d.IdComboNavigation.Nombre,
                PrecioUnitario = d.PrecioUnitario,
                Cantidad = d.Cantidad,
                Subtotal = subtotal,
                Impuesto = impuesto,
                TotalLinea = subtotal + impuesto,
                Observaciones = d.Observaciones
            };
        }

        // ---------- CÁLCULO ASÍNCRONO DE UNA LÍNEA (formulario) ----------
        public async Task<PedidoLineaDto> CalcularLineaAsync(PedidoLineaEntradaDto entrada)
        {
            decimal precioUnitario;
            string nombre;

            if (entrada.Tipo == "Producto")
            {
                var producto = await _repositoryProducto.FindByIdAsync(entrada.IdItem)
                    ?? throw new InvalidOperationException("Producto no encontrado.");
                precioUnitario = producto.Precio;
                nombre = producto.Nombre;
            }
            else if (entrada.Tipo == "Combo")
            {
                var combo = await _repositoryCombo.FindByIdAsync(entrada.IdItem)
                    ?? throw new InvalidOperationException("Combo no encontrado.");
                precioUnitario = combo.Precio;
                nombre = combo.Nombre;
            }
            else
            {
                throw new InvalidOperationException("Tipo de línea inválido.");
            }

            var cantidad = entrada.Cantidad < 0 ? 0 : entrada.Cantidad;
            var subtotal = precioUnitario * cantidad;
            var impuesto = subtotal * _impuestosOptions.TasaIva;

            return new PedidoLineaDto
            {
                Tipo = entrada.Tipo,
                IdItem = entrada.IdItem,
                Nombre = nombre,
                PrecioUnitario = precioUnitario,
                Cantidad = cantidad,
                Subtotal = subtotal,
                Impuesto = impuesto,
                TotalLinea = subtotal + impuesto,
                Observaciones = entrada.Observaciones
            };
        }





        public async Task<PedidoFormularioDto> ObtenerDatosFormularioAsync()
        {
            var productos = await _repositoryProducto.ListAsync();
            var combos = await _repositoryCombo.ListAsync();
            var metodosEntrega = await _repositoryPedido.GetMetodosEntregaAsync();
            var metodosPago = await _repositoryPedido.GetMetodosPagoAsync();

            var dto = new PedidoFormularioDto
            {
                Productos = productos.Select(p => new PedidoItemOpcionDto
                {
                    IdItem = p.IdProducto,
                    Tipo = "Producto",
                    Nombre = p.Nombre,
                    Precio = p.Precio
                }).ToList(),
                Combos = combos.Select(c => new PedidoItemOpcionDto
                {
                    IdItem = c.IdCombo,
                    Tipo = "Combo",
                    Nombre = c.Nombre,
                    Precio = c.Precio
                }).ToList(),
                MetodosEntrega = metodosEntrega.Select(m => new PedidoMetodoOpcionDto { Id = m.IdMetodoEntrega, Nombre = m.Nombre }).ToList(),
                MetodosPago = metodosPago.Select(m => new PedidoMetodoOpcionDto { Id = m.IdMetodoPago, Nombre = m.Nombre }).ToList(),
                RolActual = _usuarioActual.Rol ?? string.Empty
            };

            if (_usuarioActual.Rol == "Cliente" && _usuarioActual.IdUsuario.HasValue)
            {
                var cliente = (await _repositoryPedido.GetClientesAsync())
                    .FirstOrDefault(c => c.IdUsuario == _usuarioActual.IdUsuario.Value);
                if (cliente is not null)
                {
                    dto.ClienteNombre = $"{cliente.Nombre} {cliente.Apellidos}".Trim();
                    dto.ClienteCorreo = cliente.Correo;
                }
            }
            else
            {
                // Encargado: necesita la lista completa de clientes, y ver su propio nombre
                dto.ClientesDisponibles = await ObtenerClientesAsync();
                // El nombre del encargado se resuelve igual que el cliente, pero no filtramos por rol Cliente
                // (se podría exponer un método específico en el repo; aquí usamos IUsuarioActualService + un acceso mínimo)
            }

            return dto;
        }



        // ---------- LISTA DE CLIENTES (para el Encargado) ----------
        public async Task<List<PedidoClienteOpcionDto>> ObtenerClientesAsync()
        {
            var clientes = await _repositoryPedido.GetClientesAsync();
            return clientes.Select(c => new PedidoClienteOpcionDto
            {
                IdUsuario = c.IdUsuario,
                Nombre = $"{c.Nombre} {c.Apellidos}".Trim(),
                Correo = c.Correo
            }).ToList();
        }

        // ---------- LISTA DE ESTADOS (para el filtro del Historial) ----------
        public async Task<List<PedidoMetodoOpcionDto>> ObtenerEstadosAsync()
        {
            var estados = await _repositoryPedido.GetEstadosAsync();
            return estados.Select(e => new PedidoMetodoOpcionDto { Id = e.IdEstadoPedido, Nombre = e.Nombre }).ToList();
        }

        // ---------- REGISTRO FINAL ----------
        public async Task<PedidoDTO> RegistrarAsync(PedidoRegistroDto registro)
        {
            if (!_usuarioActual.EstaAutenticado || _usuarioActual.IdUsuario is null)
                throw new UnauthorizedAccessException("Debe iniciar sesión para registrar un pedido.");

            // --- Resolver Cliente y Encargado SIEMPRE del lado del servidor ---
            int idCliente;
            int? idEmpleado;

            if (_usuarioActual.Rol == "Cliente")
            {
                idCliente = _usuarioActual.IdUsuario.Value; // nunca se confía en registro.IdCliente aquí
                idEmpleado = null; // un cliente no tiene encargado que lo registre
            }
            else
            {
                idCliente = registro.IdCliente; // el encargado sí lo selecciona de una lista real
                idEmpleado = _usuarioActual.IdUsuario.Value; // el encargado logueado, no editable
            }

            var estadoInicial = await _repositoryPedido.GetEstadoPorNombreAsync(ESTADO_INICIAL)
                ?? throw new InvalidOperationException($"No existe el estado '{ESTADO_INICIAL}' precargado en la BD.");

            // --- Armar las líneas, resolviendo precio real desde BD (nunca del navegador) ---
            var detallesProducto = new List<PedidoDetalleProducto>();
            var detallesCombo = new List<PedidoDetalleCombo>();
            decimal subtotalGeneral = 0m;

            foreach (var linea in registro.Lineas.Where(l => l.Cantidad > 0))
            {
                if (linea.Tipo == "Producto")
                {
                    var producto = await _repositoryProducto.FindByIdAsync(linea.IdItem)
                        ?? throw new InvalidOperationException($"Producto {linea.IdItem} no encontrado.");

                    detallesProducto.Add(new PedidoDetalleProducto
                    {
                        IdProducto = producto.IdProducto,
                        Cantidad = linea.Cantidad,
                        PrecioUnitario = producto.Precio,
                        Observaciones = linea.Observaciones
                    });
                    subtotalGeneral += producto.Precio * linea.Cantidad;
                }
                else if (linea.Tipo == "Combo")
                {
                    var combo = await _repositoryCombo.FindByIdAsync(linea.IdItem)
                        ?? throw new InvalidOperationException($"Combo {linea.IdItem} no encontrado.");

                    detallesCombo.Add(new PedidoDetalleCombo
                    {
                        IdCombo = combo.IdCombo,
                        Cantidad = linea.Cantidad,
                        PrecioUnitario = combo.Precio,
                        Observaciones = linea.Observaciones
                    });
                    subtotalGeneral += combo.Precio * linea.Cantidad;
                }
            }

            if (!detallesProducto.Any() && !detallesCombo.Any())
                throw new InvalidOperationException("El pedido debe tener al menos una línea de detalle.");

            // --- Costo de envío: regla de negocio del servidor, no algo que mande el navegador ---
            // (se determina por si hay dirección de entrega informada, que corresponde a "a domicilio")
            var esEntregaADomicilio = !string.IsNullOrWhiteSpace(registro.DireccionEntrega);
            var costoEnvio = esEntregaADomicilio ? _envioOptions.CostoEnvio : 0m;

            var impuestoGeneral = subtotalGeneral * _impuestosOptions.TasaIva;
            var totalGeneral = subtotalGeneral + impuestoGeneral + costoEnvio;

            // --- Armar el pago ---
            var pago = new Pago
            {
                IdMetodoPago = registro.Pago.IdMetodoPago,
                Monto = totalGeneral,
                FechaPago = DateTime.Now
            };

            if (registro.Pago.MontoRecibido.HasValue)
            {
                pago.MontoPagado = registro.Pago.MontoRecibido.Value;
                pago.Vuelto = registro.Pago.MontoRecibido.Value - totalGeneral; // recalculado en servidor, no se confía en el vuelto del navegador
            }

            // --- Armar el Pedido completo ---
            var pedido = new Pedido
            {
                FechaPedido = DateTime.Now,
                IdEstadoPedido = estadoInicial.IdEstadoPedido,
                IdMetodoEntrega = registro.IdMetodoEntrega,
                DireccionEntrega = registro.DireccionEntrega,
                Subtotal = subtotalGeneral,
                Impuesto = impuestoGeneral,
                CostoEnvio = costoEnvio,
                Total = totalGeneral,
                IdCliente = idCliente,
                IdEmpleado = idEmpleado,
                PedidoDetalleProductos = detallesProducto,
                PedidoDetalleCombos = detallesCombo,
                Pagos = new List<Pago> { pago }
            };

            var pedidoCreado = await _repositoryPedido.CrearAsync(pedido);

            var pedidoCompleto = await _repositoryPedido.GetByIdAsync(pedidoCreado.IdPedido);
            return await ObtenerDetalleAsync(pedidoCompleto!.IdPedido) ?? throw new InvalidOperationException("Error al recuperar el pedido registrado.");
        }

        public async Task<PedidoDTO> ActualizarEstadoAsync(CambiarEstadoPedidoDto dto)
        {
            if (!_usuarioActual.EstaAutenticado || _usuarioActual.IdUsuario is null)
                throw new UnauthorizedAccessException("Debe iniciar sesión.");

            // Solo Encargado, Cocina o Administrador pueden cambiar el estado - nunca un Cliente
            if (_usuarioActual.Rol == "Cliente")
                throw new UnauthorizedAccessException("No tiene permisos para cambiar el estado del pedido.");

            var pedido = await _repositoryPedido.GetByIdAsync(dto.IdPedido)
                ?? throw new InvalidOperationException("Pedido no encontrado.");

            var nuevoEstado = await _repositoryPedido.GetEstadoPorIdAsync(dto.IdEstadoPedido)
                ?? throw new InvalidOperationException("Estado no válido.");

            // Si nadie lo había tomado todavía (lo registró el propio cliente), el empleado que
            // hace el primer cambio de estado queda asociado como el encargado de ese pedido
            if (pedido.IdEmpleado is null)
                pedido.IdEmpleado = _usuarioActual.IdUsuario.Value;

            pedido.IdEstadoPedido = nuevoEstado.IdEstadoPedido;

            await _repositoryPedido.GuardarCambiosAsync();

            return await ObtenerDetalleAsync(pedido.IdPedido)
                ?? throw new InvalidOperationException("Error al recuperar el pedido actualizado.");
        }
    }
}