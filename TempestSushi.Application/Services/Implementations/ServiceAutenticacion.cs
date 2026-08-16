using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Application.Services
{
    public class ServiceAutenticacion : IServiceAutenticacion
    {
        private readonly IRepositoryUsuario _repositoryUsuario;

        public ServiceAutenticacion(
            IRepositoryUsuario repositoryUsuario)
        {
            _repositoryUsuario = repositoryUsuario;
        }

        public async Task<UsuarioAutenticadoDTO?> AutenticarAsync(
            string correo,
            string password)
        {
            correo = correo.Trim();
            var usuario = await _repositoryUsuario
                .FindByCorreoAsync(correo);

            if (usuario == null)
            {
                return null;
            }

            if (!usuario.Activo)
            {
                return null;
            }

            bool passwordValido = BCrypt.Net.BCrypt.Verify(
                password,
                usuario.PasswordHash);

            if (!passwordValido)
            {
                return null;
            }

            return new UsuarioAutenticadoDTO
            {
                IdUsuario = usuario.IdUsuario,

                NombreCompleto = string.IsNullOrWhiteSpace(usuario.Apellidos)
                    ? usuario.Nombre
                    : $"{usuario.Nombre} {usuario.Apellidos}",

                Correo = usuario.Correo,

                Rol = usuario.IdRolUsuarioNavigation.Nombre,

                DebeCambiarPassword = usuario.DebeCambiarPassword
            };
        }
    }
}