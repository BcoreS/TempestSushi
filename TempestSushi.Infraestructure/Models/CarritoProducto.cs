using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class CarritoProducto
{
    public int IdCarritoProducto { get; set; }

    public int IdCarrito { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public string? Observaciones { get; set; }

    public virtual Carrito IdCarritoNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
