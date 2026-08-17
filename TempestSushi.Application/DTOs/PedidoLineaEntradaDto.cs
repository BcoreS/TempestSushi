using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class PedidoLineaEntradaDto
    {
        public string Tipo { get; set; } = null!; // "Producto" | "Combo"
        public int IdItem { get; set; }            // IdProducto o IdCombo
        public int Cantidad { get; set; }
        public string? Observaciones { get; set; }
    }
}