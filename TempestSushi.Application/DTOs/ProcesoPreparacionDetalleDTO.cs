using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public record ProcesoPreparacionDetalleDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = null!;
        public ICollection<EstacionDTO> Estaciones { get; set; } = new List<EstacionDTO>();
    }

    public record EstacionDTO
    {
        public int NumeroPaso { get; set; }
        public string NombreEstacion { get; set; } = null!;
    }
}