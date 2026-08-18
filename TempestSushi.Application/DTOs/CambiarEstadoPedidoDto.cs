using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class CambiarEstadoPedidoDto
    {
        public int IdPedido { get; set; }
        public int IdEstadoPedido { get; set; }
    }
}