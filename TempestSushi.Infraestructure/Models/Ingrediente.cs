using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class Ingrediente
{
    public int IdIngrediente { get; set; }

    public string Nombre { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Producto> IdProductos { get; set; } = new List<Producto>();
}
