namespace TempestSushi.Application.DTOs
{
    public class UsuarioListadoDTO
    {
        public int IdUsuario { get; set; }

        public string NombreCompleto { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string Rol { get; set; } = string.Empty;

        public bool Activo { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}