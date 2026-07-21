using System.ComponentModel.DataAnnotations;

namespace TempestSushi.Application.DTOs
{
    public class ProcesoPreparacionFormDTO : IValidatableObject
    {
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Debe seleccionar un producto.")]
        public int IdProducto { get; set; }

        public string? NombreProducto { get; set; }

        public List<PasoPreparacionFormDTO> Pasos { get; set; }
            = new();

        public List<SeleccionDTO> ProductosDisponibles { get; set; }
            = new();

        public List<SeleccionDTO> EstacionesDisponibles { get; set; }
            = new();

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (Pasos == null || Pasos.Count == 0)
            {
                yield return new ValidationResult(
                    "Debe agregar al menos una estación al proceso.",
                    new[] { nameof(Pasos) });
            }
        }
    }
}