using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class PedidoClienteOpcionDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = null!; // Nombre + Apellidos concatenado
        public string Correo { get; set; } = null!;
    }
}