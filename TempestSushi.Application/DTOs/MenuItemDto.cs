using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class MenuItemDto
    {
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public string Tipo { get; set; } = ""; // "Producto" o "Combo"
    }
}