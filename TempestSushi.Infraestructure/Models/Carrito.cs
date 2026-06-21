using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class Carrito
{
    public int IdCarrito { get; set; }

    public int IdUsuario { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<CarritoCombo> CarritoCombos { get; set; } = new List<CarritoCombo>();

    public virtual ICollection<CarritoProducto> CarritoProductos { get; set; } = new List<CarritoProducto>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
