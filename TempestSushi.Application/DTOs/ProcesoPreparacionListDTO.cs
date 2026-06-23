using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public record ProcesoPreparacionListDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = null!;
        public int CantidadPasos { get; set; }
    }
}