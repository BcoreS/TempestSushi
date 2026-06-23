using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class MenuCategoriaDto
    {
        public string NombreCategoria { get; set; } = "";
        public List<MenuItemDto> Items { get; set; } = new();
    }
}