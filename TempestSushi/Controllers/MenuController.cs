using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
    }
}