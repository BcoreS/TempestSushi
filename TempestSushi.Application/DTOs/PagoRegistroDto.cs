using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class PagoRegistroDto
    {
        public int IdMetodoPago { get; set; } // Tarjeta crédito / Tarjeta débito / Efectivo

        // Campos básicos de tarjeta (solo se llenan si el método es tarjeta)
        public string? NumeroTarjeta { get; set; }
        public string? NombreTitular { get; set; }
        public string? FechaVencimiento { get; set; } // MM/AA
        public string? Cvv { get; set; }

        // Campos de efectivo (solo se llenan si el método es efectivo)
        public decimal? MontoRecibido { get; set; }
        public decimal? Vuelto { get; set; } // calculado en JS para mostrar, pero el server lo recalcula igual
    }
}