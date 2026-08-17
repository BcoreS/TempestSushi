using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class PedidoRegistroDto
    {
        // Cliente: viene autocompletado si el logueado es Cliente,
        // o seleccionado de una lista si el logueado es Encargado
        public int IdCliente { get; set; }

        // Encargado: se resuelve en el servidor con IUsuarioActualService,
        // NUNCA se confía en un IdEmpleado que venga del navegador
        // (por eso este DTO no trae ese campo)

        public int IdMetodoEntrega { get; set; }
        public string? DireccionEntrega { get; set; } // requerido solo si es a domicilio

        public List<PedidoLineaEntradaDto> Lineas { get; set; } = new();

        public PagoRegistroDto Pago { get; set; } = null!;
    }
}