using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TempestSushi.Application.DTOs
{
    public class ComboFormDto
    {
        public int IdCombo { get; set; }

        [Required(ErrorMessage = "El nombre del combo es obligatorio.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 150 caracteres.")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 500 caracteres.")]
        public string Descripcion { get; set; } = "";

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(100, 999999, ErrorMessage = "El precio debe estar entre ₡100 y ₡999,999.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Debe incluir al menos un producto en el combo.")]
        [MinLength(1, ErrorMessage = "Debe incluir al menos un producto en el combo.")]
        public List<int> ProductosSeleccionados { get; set; } = new();

        public List<ProductoOptionDto> ProductosDisponibles { get; set; } = new();
    }

    public class ProductoOptionDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
    }
}