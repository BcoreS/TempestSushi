using System.ComponentModel.DataAnnotations;

namespace TempestSushi.Application.DTOs
{
    public class RegistroClienteDTO
    {
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
            ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(
            8,
            ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Required(
            ErrorMessage = "Debe confirmar la contraseña.")]
        [DataType(DataType.Password)]
        [Compare(
            nameof(Password),
            ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar contraseña")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }
}