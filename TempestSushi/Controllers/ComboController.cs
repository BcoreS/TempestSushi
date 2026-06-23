using Microsoft.AspNetCore.Mvc;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Controllers
{
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
            {
                ViewBag.NotificationMessage = TempData["Mensaje"];
            }

            var collection = await _serviceCombo.ListAsync();
            return View(collection);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var combo = await _serviceCombo.FindByIdAsync(id);

            if (combo == null)
            {
                return NotFound();
            }

            return View(combo);
        }
    }
}