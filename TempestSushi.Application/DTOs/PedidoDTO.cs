using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class PedidoDTO
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }

        // Cliente: 2 campos que ayuden a identificarlo (ej. nombre completo + correo)
        public string ClienteNombre { get; set; } = null!;
        public string ClienteIdentificador { get; set; } = null!; // ej. correo o cédula

        // Encargado (nombre de quien registró el pedido)
        public string? EncargadoNombre { get; set; }

        public string MetodoEntregaNombre { get; set; } = null!;
        public string? DireccionEntrega { get; set; }
        public decimal CostoEnvio { get; set; }

        public string MetodoPagoNombre { get; set; } = null!;
        public string EstadoNombre { get; set; } = null!;

        public List<PedidoLineaDto> Lineas { get; set; } = new();

        public decimal TotalSinImpuestos { get; set; }
        public decimal TotalConImpuestos { get; set; } // incluye impuesto + costo de envío
    }
}