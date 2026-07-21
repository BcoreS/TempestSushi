namespace TempestSushi.Application.DTOs
{
    public record ProcesoPreparacionMantenimientoDTO
    {
        public int IdProducto { get; set; }

        public string NombreProducto { get; set; } = null!;

        public int CantidadPasos { get; set; }

        public int TiempoEstimadoTotal { get; set; }
    }
}