using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class Combo
{
    public int IdCombo { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public int IdCategoria { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<CarritoCombo> CarritoCombos { get; set; } = new List<CarritoCombo>();

    public virtual ICollection<ComboProducto> ComboProductos { get; set; } = new List<ComboProducto>();

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<MenuCombo> MenuCombos { get; set; } = new List<MenuCombo>();

    public virtual ICollection<PedidoDetalleCombo> PedidoDetalleCombos { get; set; } = new List<PedidoDetalleCombo>();
}
