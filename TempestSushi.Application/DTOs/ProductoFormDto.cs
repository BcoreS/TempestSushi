using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TempestSushi.Application.DTOs
{
    public class ProductoFormDto
    {
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 150 caracteres.")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 500 caracteres.")]
        public string Descripcion { get; set; } = "";

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(100, 999999, ErrorMessage = "El precio debe estar entre ₡100 y ₡999,999.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos un ingrediente.")]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos un ingrediente.")]
        public List<int> IngredientesSeleccionados { get; set; } = new();

        public IFormFile? Imagen { get; set; }

        public string? ImagenActualUrl { get; set; }

        public List<CategoriaOptionDto> CategoriasDisponibles { get; set; } = new();
        public List<IngredienteOptionDto> IngredientesDisponibles { get; set; } = new();
    }

    public class CategoriaOptionDto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = "";
    }

    public class IngredienteOptionDto
    {
        public int IdIngrediente { get; set; }
        public string Nombre { get; set; } = "";
    }
}