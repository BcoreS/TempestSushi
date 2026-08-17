using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class PedidoItemOpcionDto
    {
        public int IdItem { get; set; }
        public string Tipo { get; set; } = null!; // "Producto" | "Combo"
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
    }
}