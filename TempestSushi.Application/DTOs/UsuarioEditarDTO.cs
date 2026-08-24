using System.ComponentModel.DataAnnotations;

namespace TempestSushi.Application.DTOs
{
    public class UsuarioEditarDTO
    {
        public int IdUsuario { get; set; }

        [Required(
            ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Display(Name = "Apellidos")]
        public string? Apellidos { get; set; }

        [Required(
            ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(
            ErrorMessage = "Ingrese un correo electrónico válido.")]
        [Display(Name = "Correo electrónico")]
        public string Correo { get; set; } = string.Empty;

        [Phone(
            ErrorMessage = "Ingrese un número de teléfono válido.")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Required(
            ErrorMessage = "Debe seleccionar un rol.")]
        [Display(Name = "Rol")]
        public int IdRolUsuario { get; set; }
    }
}