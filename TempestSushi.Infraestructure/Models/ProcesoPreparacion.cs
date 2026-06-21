using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class ProcesoPreparacion
{
    public int IdProcesoPreparacion { get; set; }

    public int IdProducto { get; set; }

    public int IdEstacionCocina { get; set; }

    public int NumeroPaso { get; set; }

    public string? DescripcionPaso { get; set; }

    public int? TiempoEstimadoMinutos { get; set; }

    public virtual EstacionCocina IdEstacionCocinaNavigation { get; set; } = null!;

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
