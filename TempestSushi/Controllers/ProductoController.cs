using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Web.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoService _service;

        public ProductoController(IProductoService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _service.ObtenerListadoAsync();
            return View(productos);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _service.ObtenerDetalleAsync(id);
            if (producto == null) return NotFound();
            return View(producto);
        }
    }
}