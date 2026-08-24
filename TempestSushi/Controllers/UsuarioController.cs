using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;

        public UsuarioController(
            IServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }

        // ---------- LISTADO ----------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios =
                await _serviceUsuario
                    .ObtenerListadoAsync();

            return View(usuarios);
        }

        // ---------- CREAR ----------
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CargarRolesAsync();

            return View(
                new UsuarioCrearDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            UsuarioCrearDTO model)
        {
            if (!ModelState.IsValid)
            {
                await CargarRolesAsync(
                    model.IdRolUsuario);

                return View(model);
            }

            try
            {
                var creado =
                    await _serviceUsuario
                        .CrearAsync(model);

                if (!creado)
                {
                    ModelState.AddModelError(
                        nameof(model.Correo),
                        "Ya existe un usuario registrado con este correo electrónico.");

                    await CargarRolesAsync(
                        model.IdRolUsuario);

                    return View(model);
                }

                TempData["MensajeExito"] =
                    "Usuario creado correctamente.";

                return RedirectToAction(
                    nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                await CargarRolesAsync(
                    model.IdRolUsuario);

                return View(model);
            }
        }

        // ---------- EDITAR ----------
        [HttpGet]
        public async Task<IActionResult> Edit(
            int id)
        {
            var usuario =
                await _serviceUsuario
                    .ObtenerParaEditarAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            await CargarRolesAsync(
                usuario.IdRolUsuario);

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            UsuarioEditarDTO model)
        {
            if (!ModelState.IsValid)
            {
                await CargarRolesAsync(
                    model.IdRolUsuario);

                return View(model);
            }

            try
            {
                var actualizado =
                    await _serviceUsuario
                        .EditarAsync(model);

                if (!actualizado)
                {
                    ModelState.AddModelError(
                        nameof(model.Correo),
                        "Ya existe otro usuario registrado con este correo electrónico.");

                    await CargarRolesAsync(
                        model.IdRolUsuario);

                    return View(model);
                }

                TempData["MensajeExito"] =
                    "Usuario actualizado correctamente.";

                return RedirectToAction(
                    nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                await CargarRolesAsync(
                    model.IdRolUsuario);

                return View(model);
            }
        }

        // ---------- ACTIVAR / INACTIVAR ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(
            int id)
        {
            try
            {
                var actualizado =
                    await _serviceUsuario
                        .CambiarEstadoAsync(id);

                if (!actualizado)
                {
                    return NotFound();
                }

                TempData["MensajeExito"] =
                    "Estado del usuario actualizado correctamente.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensajeError"] =
                    ex.Message;
            }

            return RedirectToAction(
                nameof(Index));
        }

        // ---------- CARGAR ROLES ----------
        private async Task CargarRolesAsync(
            int? seleccionado = null)
        {
            var roles =
                await _serviceUsuario
                    .ObtenerRolesActivosAsync();

            ViewBag.Roles =
                new SelectList(
                    roles,
                    nameof(RolUsuarioDTO.IdRolUsuario),
                    nameof(RolUsuarioDTO.Nombre),
                    seleccionado);
        }
    }
}