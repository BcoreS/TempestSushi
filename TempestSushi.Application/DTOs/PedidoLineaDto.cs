using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class PedidoLineaDto
    {
        public string Tipo { get; set; } = null!; // "Producto" | "Combo"
        public int IdItem { get; set; }            // IdProducto o IdCombo
        public string Nombre { get; set; } = null!;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }      // PrecioUnitario * Cantidad
        public decimal Impuesto { get; set; }       // Subtotal * TasaIva
        public decimal TotalLinea { get; set; }     // Subtotal + Impuesto
        public string? Observaciones { get; set; }
    }
}