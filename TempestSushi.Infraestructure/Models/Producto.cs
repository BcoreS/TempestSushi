using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public int IdCategoria { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<CarritoProducto> CarritoProductos { get; set; } = new List<CarritoProducto>();

    public virtual ICollection<ComboProducto> ComboProductos { get; set; } = new List<ComboProducto>();

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual ICollection<ImagenProducto> ImagenProductos { get; set; } = new List<ImagenProducto>();

    public virtual ICollection<MenuProducto> MenuProductos { get; set; } = new List<MenuProducto>();

    public virtual ICollection<PedidoDetalleProducto> PedidoDetalleProductos { get; set; } = new List<PedidoDetalleProducto>();

    public virtual ICollection<ProcesoPreparacion> ProcesoPreparacions { get; set; } = new List<ProcesoPreparacion>();

    public virtual ICollection<Ingrediente> IdIngredientes { get; set; } = new List<Ingrediente>();
}
