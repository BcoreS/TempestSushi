using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class PedidoHistorialDto
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }

        // Solo se muestran cuando el que consulta es Encargado/Administrador
        // (el cliente ve su propio historial, no necesita ver su propio nombre repetido)
        public string? ClienteNombre { get; set; }

        public string EstadoNombre { get; set; } = null!;
        public string MetodoEntregaNombre { get; set; } = null!;
        public decimal Total { get; set; } // total con impuestos, para mostrar de un vistazo
    }
}