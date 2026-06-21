using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public DateTime FechaPedido { get; set; }

    public int IdEstadoPedido { get; set; }

    public int IdMetodoEntrega { get; set; }

    public string? DireccionEntrega { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuesto { get; set; }

    public decimal CostoEnvio { get; set; }

    public decimal Total { get; set; }

    public int IdCliente { get; set; }

    public int? IdEmpleado { get; set; }

    public virtual Usuario IdClienteNavigation { get; set; } = null!;

    public virtual Usuario? IdEmpleadoNavigation { get; set; }

    public virtual EstadoPedido IdEstadoPedidoNavigation { get; set; } = null!;

    public virtual MetodoEntrega IdMetodoEntregaNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual ICollection<PedidoDetalleCombo> PedidoDetalleCombos { get; set; } = new List<PedidoDetalleCombo>();

    public virtual ICollection<PedidoDetalleProducto> PedidoDetalleProductos { get; set; } = new List<PedidoDetalleProducto>();
}
