using System.ComponentModel.DataAnnotations;

namespace TempestSushi.Application.DTOs
{
    public class PasoPreparacionFormDTO
    {
        public int? IdProcesoPreparacion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una estación.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una estación.")]
        public int IdEstacionCocina { get; set; }

        public int NumeroPaso { get; set; }

        [StringLength(
            500,
            ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? DescripcionPaso { get; set; }

        [Range(
            1,
            1440,
            ErrorMessage = "El tiempo estimado debe ser mayor que cero.")]
        public int? TiempoEstimadoMinutos { get; set; }
    }
}