using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class ClimaDto
    {
        public decimal Temperatura { get; set; }
        public string Descripcion { get; set; } = null!;
    }
}