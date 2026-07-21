using System.Collections.Generic;

namespace TempestSushi.Application.DTOs
{
    public record ComboDTO
    {
        public int IdCombo { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public decimal Precio { get; set; }
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; } = null!;
        public bool Activo { get; set; }
        public List<ComboProductoItemDTO> Productos { get; set; } = new();
    }

    public record ComboProductoItemDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = null!;
        public int Cantidad { get; set; }
    }
}