namespace TempestSushi.Application.DTOs
{
    public record ProcesoPreparacionDTO
    {
        public int IdProcesoPreparacion { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = null!;
        public int IdEstacionCocina { get; set; }
        public string NombreEstacionCocina { get; set; } = null!;
        public int NumeroPaso { get; set; }
        public string? DescripcionPaso { get; set; }
        public int? TiempoEstimadoMinutos { get; set; }
    }
}