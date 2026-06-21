using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class CarritoCombo
{
    public int IdCarritoCombo { get; set; }

    public int IdCarrito { get; set; }

    public int IdCombo { get; set; }

    public int Cantidad { get; set; }

    public string? Observaciones { get; set; }

    public virtual Carrito IdCarritoNavigation { get; set; } = null!;

    public virtual Combo IdComboNavigation { get; set; } = null!;
}
