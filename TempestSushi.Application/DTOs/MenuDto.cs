using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class MenuDto
    {
        public int IdMenu { get; set; }
        public string Nombre { get; set; } = "";
        public string Dias { get; set; } = "";
        public string Horario { get; set; } = "";
        public string Vigencia { get; set; } = "";
    }
}