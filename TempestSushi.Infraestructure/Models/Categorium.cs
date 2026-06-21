using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Combo> Combos { get; set; } = new List<Combo>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
