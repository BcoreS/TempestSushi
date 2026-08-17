using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempestSushi.Application.DTOs
{
    public class PedidoFormularioDto
    {
        public List<PedidoItemOpcionDto> Productos { get; set; } = new();
        public List<PedidoItemOpcionDto> Combos { get; set; } = new();
        public List<PedidoMetodoOpcionDto> MetodosEntrega { get; set; } = new();
        public List<PedidoMetodoOpcionDto> MetodosPago { get; set; } = new();

        // Si el logueado es Cliente: se autocompleta, no editable
        public string? ClienteNombre { get; set; }
        public string? ClienteCorreo { get; set; }

        // Si el logueado es Encargado: debe elegir de esta lista
        public List<PedidoClienteOpcionDto> ClientesDisponibles { get; set; } = new();

        // Si el logueado es Encargado: se muestra su propio nombre, no editable
        public string? EncargadoNombre { get; set; }

        public string RolActual { get; set; } = null!;
    }
}