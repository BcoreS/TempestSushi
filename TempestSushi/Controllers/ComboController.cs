using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Controllers
{
    [Authorize]
    public class ComboController : Controller
    {
        private readonly IServiceCombo _serviceCombo;

        public ComboController(IServiceCombo serviceCombo)
        {
            _serviceCombo = serviceCombo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.NotificationMessage = TempData["Mensaje"];

            var collection = await _serviceCombo.ListAsync();
            return View(collection);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var combo = await _serviceCombo.FindByIdAsync(id);

            if (combo == null)
                return NotFound();

            return View(combo);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var dto = await _serviceCombo.ObtenerParaCrearAsync();
            return View(dto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ComboFormDto dto)
        {
            if (await _serviceCombo.ExisteNombreAsync(dto.Nombre))
            {
                ModelState.AddModelError(
                    nameof(dto.Nombre),
                    "Ya existe un combo registrado con ese nombre.");
            }

            if (!ModelState.IsValid)
            {
                await _serviceCombo.CargarListasAsync(dto);
                return View(dto);
            }

            await _serviceCombo.CrearAsync(dto);

            TempData["Mensaje"] =
                $"Combo \"{dto.Nombre}\" creado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var dto = await _serviceCombo.ObtenerParaEditarAsync(id);

            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(
            int id,
            ComboFormDto dto)
        {
            if (id != dto.IdCombo)
                return NotFound();

            if (await _serviceCombo.ExisteNombreAsync(
                dto.Nombre,
                dto.IdCombo))
            {
                ModelState.AddModelError(
                    nameof(dto.Nombre),
                    "Ya existe otro combo registrado con ese nombre.");
            }

            if (!ModelState.IsValid)
            {
                await _serviceCombo.CargarListasAsync(dto);
                return View(dto);
            }

            var actualizado =
                await _serviceCombo.ActualizarAsync(dto);

            if (!actualizado)
                return NotFound();

            TempData["Mensaje"] =
                $"Combo \"{dto.Nombre}\" actualizado correctamente.";

            return RedirectToAction(
                nameof(Details),
                new { id = dto.IdCombo });
        }
    }
}