using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuService _service;

        public MenuController(IMenuService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _service.ObtenerListadoAsync();
            return View(menus);
        }

        public async Task<IActionResult> Disponible()
        {
            var menu = await _service.ObtenerMenuDisponibleAsync();
            if (menu == null)
            {
                ViewBag.Mensaje = "No hay ningún menú disponible en este momento.";
                return View((object?)null);
            }
            return View(menu);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _service.PrepararCrearAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuFormDto dto)
        {
            if (!ModelState.IsValid)
            {
                await _service.PrepararFormularioAsync(dto);
                return View(dto);
            }

            await _service.CrearAsync(dto);

            TempData["MensajeExito"] = "El menú se creó correctamente.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _service.ObtenerParaEditarAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuFormDto dto)
        {
            if (!ModelState.IsValid)
            {
                await _service.PrepararFormularioAsync(dto);
                return View(dto);
            }

            try
            {
                await _service.ActualizarAsync(dto);

                TempData["MensajeExito"] =
                    "El menú se actualizó correctamente.";

                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var menu = await _service.ObtenerParaEditarAsync(id);

            if (menu == null)
            {
                return NotFound();
            }

            await _service.EliminarAsync(id);

            TempData["MensajeExito"] =
                $"El menú \"{menu.Nombre}\" se eliminó correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}