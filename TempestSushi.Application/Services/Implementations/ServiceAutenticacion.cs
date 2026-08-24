using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Application.Services
{
    public class ServiceAutenticacion : IServiceAutenticacion
    {
        private readonly IRepositoryUsuario _repositoryUsuario;

        private const string ROL_CLIENTE = "Cliente";

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

            bool passwordValido =
                BCrypt.Net.BCrypt.Verify(
                    password,
                    usuario.PasswordHash);

            if (!passwordValido)
            {
                return null;
            }

            return new UsuarioAutenticadoDTO
            {
                IdUsuario = usuario.IdUsuario,

                NombreCompleto =
                    string.IsNullOrWhiteSpace(usuario.Apellidos)
                        ? usuario.Nombre
                        : $"{usuario.Nombre} {usuario.Apellidos}",

                Correo = usuario.Correo,

                Rol = usuario
                    .IdRolUsuarioNavigation
                    .Nombre,

                DebeCambiarPassword =
                    usuario.DebeCambiarPassword
            };
        }

        public async Task<bool> RegistrarClienteAsync(
            RegistroClienteDTO registro)
        {
            var correo = registro.Correo.Trim();

            var correoExiste =
                await _repositoryUsuario
                    .ExisteCorreoAsync(correo);

            if (correoExiste)
            {
                return false;
            }

            var rolCliente =
                await _repositoryUsuario
                    .FindRolByNombreAsync(ROL_CLIENTE);

            if (rolCliente == null)
            {
                throw new InvalidOperationException(
                    $"No existe el rol '{ROL_CLIENTE}' activo en la base de datos.");
            }

            var usuario = new Usuario
            {
                IdRolUsuario =
                    rolCliente.IdRolUsuario,

                Nombre =
                    registro.Nombre.Trim(),

                Apellidos =
                    string.IsNullOrWhiteSpace(
                        registro.Apellidos)
                        ? null
                        : registro.Apellidos.Trim(),

                Correo =
                    correo,

                Telefono =
                    string.IsNullOrWhiteSpace(
                        registro.Telefono)
                        ? null
                        : registro.Telefono.Trim(),

                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(
                        registro.Password),

                DebeCambiarPassword = false,

                Activo = true,

                FechaRegistro = DateTime.Now
            };

            await _repositoryUsuario
                .CrearAsync(usuario);

            return true;
        }
    }
}