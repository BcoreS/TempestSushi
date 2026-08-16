namespace TempestSushi.Application.DTOs
{
    public class UsuarioAutenticadoDTO
    {
        public int IdUsuario { get; set; }

        public string NombreCompleto { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Rol { get; set; } = string.Empty;

        public bool DebeCambiarPassword { get; set; }
    }
}