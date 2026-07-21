using Microsoft.AspNetCore.Mvc;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Controllers
{
    public class ProcesoPreparacionController : Controller
    {
        private readonly IServiceProcesoPreparacion
            _serviceProcesoPreparacion;

        public ProcesoPreparacionController(
            IServiceProcesoPreparacion serviceProcesoPreparacion)
        {
            _serviceProcesoPreparacion =
                serviceProcesoPreparacion;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage =
                    TempData["Mensaje"];
            }

            var collection =
                await _serviceProcesoPreparacion.ListAsync();

            return View(collection);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model =
                await _serviceProcesoPreparacion
                    .FindByProductoIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Mantenimiento()
        {
            var model =
                await _serviceProcesoPreparacion
                    .ListMantenimientoAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model =
                await _serviceProcesoPreparacion
                    .PrepararCrearAsync();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ProcesoPreparacionFormDTO dto)
        {
            if (!ModelState.IsValid)
            {
                await _serviceProcesoPreparacion
                    .PrepararFormularioAsync(dto);

                return View(dto);
            }

            try
            {
                await _serviceProcesoPreparacion.CrearAsync(dto);

                TempData["MensajeExito"] =
                    "El proceso de preparación se creó correctamente.";

                return RedirectToAction(nameof(Mantenimiento));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    nameof(dto.IdProducto),
                    ex.Message);

                await _serviceProcesoPreparacion
                    .PrepararFormularioAsync(dto);

                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model =
                await _serviceProcesoPreparacion.ObtenerParaEditarAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            ProcesoPreparacionFormDTO dto)
        {
            if (id != dto.IdProducto)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                await _serviceProcesoPreparacion
                    .PrepararFormularioAsync(dto);

                return View(dto);
            }

            try
            {
                await _serviceProcesoPreparacion.ActualizarAsync(dto);

                TempData["MensajeExito"] =
                    "El proceso de preparación se actualizó correctamente.";

                return RedirectToAction(nameof(Mantenimiento));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(
                    string.Empty,
                    ex.Message);

                await _serviceProcesoPreparacion
                    .PrepararFormularioAsync(dto);

                return View(dto);
            }
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _serviceProcesoPreparacion.EliminarAsync(id);

                TempData["MensajeExito"] =
                    "El proceso de preparación se eliminó correctamente.";

                return RedirectToAction(nameof(Mantenimiento));
            }
            catch (InvalidOperationException ex)
            {
                TempData["MensajeError"] = ex.Message;

                return RedirectToAction(nameof(Mantenimiento));
            }
        }
    }
}