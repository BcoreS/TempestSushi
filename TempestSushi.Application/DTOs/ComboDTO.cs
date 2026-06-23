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
    }
}