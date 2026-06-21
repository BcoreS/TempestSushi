using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class MetodoPago
{
    public int IdMetodoPago { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
