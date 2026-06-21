using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class EstacionCocina
{
    public int IdEstacionCocina { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<ProcesoPreparacion> ProcesoPreparacions { get; set; } = new List<ProcesoPreparacion>();

    public virtual ICollection<SeguimientoCocina> SeguimientoCocinas { get; set; } = new List<SeguimientoCocina>();
}
