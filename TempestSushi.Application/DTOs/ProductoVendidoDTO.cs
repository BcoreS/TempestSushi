namespace TempestSushi.Application.DTOs
{
    public class ProductoVendidoDTO
    {
        public int IdProducto { get; set; }

        public string NombreProducto { get; set; } = string.Empty;

        public int CantidadVendida { get; set; }
    }
}