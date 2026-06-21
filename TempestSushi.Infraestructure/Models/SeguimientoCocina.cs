using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class SeguimientoCocina
{
    public int IdSeguimientoCocina { get; set; }

    public int IdPedidoDetalleProducto { get; set; }

    public int IdEstacionCocina { get; set; }

    public int NumeroPaso { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? IdUsuarioCocina { get; set; }

    public bool Completado { get; set; }

    public virtual EstacionCocina IdEstacionCocinaNavigation { get; set; } = null!;

    public virtual PedidoDetalleProducto IdPedidoDetalleProductoNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioCocinaNavigation { get; set; }
}
