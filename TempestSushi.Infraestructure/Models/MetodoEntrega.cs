using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class MetodoEntrega
{
    public int IdMetodoEntrega { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
