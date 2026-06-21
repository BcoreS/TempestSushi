using System;
using System.Collections.Generic;

namespace TempestSushi.Infraestructure.Models;

public partial class ImagenProducto
{
    public int IdImagenProducto { get; set; }

    public int IdProducto { get; set; }

    public string UrlImagen { get; set; } = null!;

    public bool EsPrincipal { get; set; }

    public int Orden { get; set; }

    public virtual Producto IdProductoNavigation { get; set; } = null!;
}
