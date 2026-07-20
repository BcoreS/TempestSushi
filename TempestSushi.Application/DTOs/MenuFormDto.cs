using System.ComponentModel.DataAnnotations;

namespace TempestSushi.Application.DTOs
{
    public class MenuFormDto : IValidatableObject
    {
        public int IdMenu { get; set; }

        [Required(ErrorMessage = "El nombre del menú es obligatorio.")]
        [StringLength(
            150,
            MinimumLength = 3,
            ErrorMessage = "El nombre debe contener entre 3 y 150 caracteres.")]
        [Display(Name = "Nombre del menú")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [Display(Name = "Fecha de inicio")]
        [DataType(DataType.Date)]
        public DateOnly FechaInicio { get; set; }

        [Display(Name = "Fecha de finalización")]
        [DataType(DataType.Date)]
        public DateOnly? FechaFin { get; set; }

        [Required(ErrorMessage = "La hora de inicio es obligatoria.")]
        [Display(Name = "Hora de inicio")]
        [DataType(DataType.Time)]
        public TimeOnly HoraInicio { get; set; }

        [Required(ErrorMessage = "La hora de finalización es obligatoria.")]
        [Display(Name = "Hora de finalización")]
        [DataType(DataType.Time)]
        public TimeOnly HoraFin { get; set; }

        [Required(ErrorMessage = "Debe indicar los días disponibles.")]
        [StringLength(
            100,
            ErrorMessage = "Los días disponibles no pueden superar los 100 caracteres.")]
        [Display(Name = "Días disponibles")]
        public string DiasDisponibles { get; set; } = string.Empty;

        [Display(Name = "Productos")]
        public List<int> ProductosSeleccionados { get; set; } = new();

        [Display(Name = "Combos")]
        public List<int> CombosSeleccionados { get; set; } = new();

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        public List<SeleccionMenuDto> ProductosDisponibles { get; set; } = new();

        public List<SeleccionMenuDto> CombosDisponibles { get; set; } = new();
        public List<string> OpcionesDiasDisponibles { get; set; } = new();


        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (FechaFin.HasValue && FechaInicio > FechaFin.Value)
            {
                yield return new ValidationResult(
                    "La fecha de inicio no puede ser posterior a la fecha de finalización.",
                    new[] { nameof(FechaInicio), nameof(FechaFin) });
            }

            if (HoraInicio >= HoraFin)
            {
                yield return new ValidationResult(
                    "La hora de finalización debe ser posterior a la hora de inicio.",
                    new[] { nameof(HoraInicio), nameof(HoraFin) });
            }

            if (!ProductosSeleccionados.Any() &&
                !CombosSeleccionados.Any())
            {
                yield return new ValidationResult(
                    "Debe seleccionar al menos un producto o un combo.",
                    new[]
                    {
                        nameof(ProductosSeleccionados),
                        nameof(CombosSeleccionados)
                    });
            }
        }
    }
}