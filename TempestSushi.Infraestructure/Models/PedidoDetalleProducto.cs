using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class PedidoDetalleProducto
{
    public int IdPedidoDetalleProducto { get; set; }

    public int IdPedido { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public string? Observaciones { get; set; }

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;

    public virtual ICollection<SeguimientoCocina> SeguimientoCocinas { get; set; } = new List<SeguimientoCocina>();
}
