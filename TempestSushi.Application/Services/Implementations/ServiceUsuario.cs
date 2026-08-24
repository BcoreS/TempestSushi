using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Application.Services.Implementations
{
    public class ServiceUsuario : IServiceUsuario
    {
        private readonly IRepositoryUsuario _repositoryUsuario;
        private readonly IUsuarioActualService _usuarioActual;

        public ServiceUsuario(
            IRepositoryUsuario repositoryUsuario,
            IUsuarioActualService usuarioActual)
        {
            _repositoryUsuario = repositoryUsuario;
            _usuarioActual = usuarioActual;
        }

        // ---------- LISTADO ----------
        public async Task<List<UsuarioListadoDTO>>
            ObtenerListadoAsync()
        {
            var usuarios =
                await _repositoryUsuario.ListAsync();

            return usuarios
                .Select(u => new UsuarioListadoDTO
                {
                    IdUsuario =
                        u.IdUsuario,

                    NombreCompleto =
                        string.IsNullOrWhiteSpace(u.Apellidos)
                            ? u.Nombre
                            : $"{u.Nombre} {u.Apellidos}",

                    Correo =
                        u.Correo,

                    Telefono =
                        u.Telefono,

                    Rol =
                        u.IdRolUsuarioNavigation.Nombre,

                    Activo =
                        u.Activo,

                    FechaRegistro =
                        u.FechaRegistro
                })
                .ToList();
        }

        // ---------- ROLES ----------
        public async Task<List<RolUsuarioDTO>>
            ObtenerRolesActivosAsync()
        {
            var roles =
                await _repositoryUsuario
                    .ListRolesActivosAsync();

            return roles
                .Select(r => new RolUsuarioDTO
                {
                    IdRolUsuario =
                        r.IdRolUsuario,

                    Nombre =
                        r.Nombre
                })
                .ToList();
        }

        // ---------- CREAR ----------
        public async Task<bool> CrearAsync(
            UsuarioCrearDTO dto)
        {
            var correo =
                dto.Correo.Trim();

            var existeCorreo =
                await _repositoryUsuario
                    .ExisteCorreoAsync(correo);

            if (existeCorreo)
            {
                return false;
            }

            var roles =
                await _repositoryUsuario
                    .ListRolesActivosAsync();

            var rol =
                roles.FirstOrDefault(
                    r => r.IdRolUsuario ==
                         dto.IdRolUsuario);

            if (rol == null)
            {
                throw new InvalidOperationException(
                    "El rol seleccionado no existe o se encuentra inactivo.");
            }

            var usuario = new Usuario
            {
                IdRolUsuario =
                    rol.IdRolUsuario,

                Nombre =
                    dto.Nombre.Trim(),

                Apellidos =
                    string.IsNullOrWhiteSpace(
                        dto.Apellidos)
                        ? null
                        : dto.Apellidos.Trim(),

                Correo =
                    correo,

                Telefono =
                    string.IsNullOrWhiteSpace(
                        dto.Telefono)
                        ? null
                        : dto.Telefono.Trim(),

                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(
                        dto.Password),

                DebeCambiarPassword =
                    true,

                Activo =
                    true,

                FechaRegistro =
                    DateTime.Now
            };

            await _repositoryUsuario
                .CrearAsync(usuario);

            return true;
        }

        // ---------- OBTENER PARA EDITAR ----------
        public async Task<UsuarioEditarDTO?>
            ObtenerParaEditarAsync(
                int idUsuario)
        {
            var usuario =
                await _repositoryUsuario
                    .FindByIdAsync(idUsuario);

            if (usuario == null)
            {
                return null;
            }

            return new UsuarioEditarDTO
            {
                IdUsuario =
                    usuario.IdUsuario,

                Nombre =
                    usuario.Nombre,

                Apellidos =
                    usuario.Apellidos,

                Correo =
                    usuario.Correo,

                Telefono =
                    usuario.Telefono,

                IdRolUsuario =
                    usuario.IdRolUsuario,

                
            };
        }

        // ---------- EDITAR ----------
        public async Task<bool> EditarAsync(
            UsuarioEditarDTO dto)
        {
            var usuario =
                await _repositoryUsuario
                    .FindByIdAsync(dto.IdUsuario);

            if (usuario == null)
            {
                throw new InvalidOperationException(
                    "El usuario no existe.");
            }

            var correo =
                dto.Correo.Trim();

            var usuarioConCorreo =
                await _repositoryUsuario
                    .FindByCorreoAsync(correo);

            if (usuarioConCorreo != null &&
                usuarioConCorreo.IdUsuario !=
                dto.IdUsuario)
            {
                return false;
            }

            var roles =
                await _repositoryUsuario
                    .ListRolesActivosAsync();

            var rol =
                roles.FirstOrDefault(
                    r => r.IdRolUsuario ==
                         dto.IdRolUsuario);

            if (rol == null)
            {
                throw new InvalidOperationException(
                    "El rol seleccionado no existe o se encuentra inactivo.");
            }

            usuario.Nombre =
                dto.Nombre.Trim();

            usuario.Apellidos =
                string.IsNullOrWhiteSpace(
                    dto.Apellidos)
                    ? null
                    : dto.Apellidos.Trim();

            usuario.Correo =
                correo;

            usuario.Telefono =
                string.IsNullOrWhiteSpace(
                    dto.Telefono)
                    ? null
                    : dto.Telefono.Trim();

            usuario.IdRolUsuario =
                rol.IdRolUsuario;

           

            await _repositoryUsuario
                .GuardarCambiosAsync();

            return true;
        }

        // ---------- ACTIVAR / INACTIVAR ----------
        public async Task<bool> CambiarEstadoAsync(
            int idUsuario)
        {
            var usuario =
                await _repositoryUsuario
                    .FindByIdAsync(idUsuario);

            if (usuario == null)
            {
                return false;
            }

            if (_usuarioActual.IdUsuario ==
                idUsuario)
            {
                throw new InvalidOperationException(
                    "No puede desactivar su propia cuenta.");
            }

            usuario.Activo =
                !usuario.Activo;

            await _repositoryUsuario
                .GuardarCambiosAsync();

            return true;
        }
    }
}