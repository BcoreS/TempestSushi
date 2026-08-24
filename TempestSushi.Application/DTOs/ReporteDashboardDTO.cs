namespace TempestSushi.Application.DTOs
{
    public class ReporteDashboardDTO
    {
        public ICollection<PedidoPorDiaDTO> PedidosPorDia { get; set; }
            = new List<PedidoPorDiaDTO>();

        public ICollection<ProductoVendidoDTO> ProductosMasVendidos { get; set; }
            = new List<ProductoVendidoDTO>();

        public ICollection<PedidoPorEstadoDTO> PedidosPorEstado { get; set; }
            = new List<PedidoPorEstadoDTO>();
    }
}