using Microsoft.AspNetCore.Mvc;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Controllers
{
    public class ProcesoPreparacionController : Controller
    {
        private readonly IServiceProcesoPreparacion _serviceProcesoPreparacion;

        public ProcesoPreparacionController(IServiceProcesoPreparacion serviceProcesoPreparacion)
        {
            _serviceProcesoPreparacion = serviceProcesoPreparacion;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            var collection = await _serviceProcesoPreparacion.ListAsync();
            return View(collection);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model =
                await _serviceProcesoPreparacion.FindByProductoIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}