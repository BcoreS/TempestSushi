using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class ProductoDetalleDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public decimal Precio { get; set; }
        public string Categoria { get; set; } = "";
        public string? ImagenUrl { get; set; }
        public List<string> Ingredientes { get; set; } = new();
    }
}