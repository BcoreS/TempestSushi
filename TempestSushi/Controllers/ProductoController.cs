using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Web.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private readonly IProductoService _service;
        private readonly IWebHostEnvironment _env;

        public ProductoController(IProductoService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.NotificationMessage = TempData["Mensaje"];

            var productos = await _service.ObtenerListadoAsync();
            return View(productos);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _service.ObtenerDetalleAsync(id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var dto = await _service.ObtenerParaCrearAsync();
            return View(dto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ProductoFormDto dto)
        {
            if (dto.Imagen == null)
                ModelState.AddModelError(
                    nameof(dto.Imagen),
                    "Debe adjuntar una imagen del producto.");

            if (await _service.ExisteNombreAsync(dto.Nombre))
                ModelState.AddModelError(
                    nameof(dto.Nombre),
                    "Ya existe un producto registrado con ese nombre.");

            if (!ModelState.IsValid)
            {
                await _service.CargarListasAsync(dto);
                return View(dto);
            }

            string? imagenUrl = await GuardarImagenAsync(dto.Imagen!);

            await _service.CrearAsync(dto, imagenUrl);

            TempData["Mensaje"] =
                $"Producto \"{dto.Nombre}\" creado correctamente.";

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var dto = await _service.ObtenerParaEditarAsync(id);

            if (dto == null)
                return NotFound();

            return View(dto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, ProductoFormDto dto)
        {
            if (id != dto.IdProducto)
                return NotFound();

            if (await _service.ExisteNombreAsync(dto.Nombre, dto.IdProducto))
                ModelState.AddModelError(
                    nameof(dto.Nombre),
                    "Ya existe otro producto registrado con ese nombre.");

            if (!ModelState.IsValid)
            {
                await _service.CargarListasAsync(dto);
                return View(dto);
            }

            string? imagenUrl =
                dto.Imagen != null
                    ? await GuardarImagenAsync(dto.Imagen)
                    : null;

            var actualizado =
                await _service.ActualizarAsync(dto, imagenUrl);

            if (!actualizado)
                return NotFound();

            TempData["Mensaje"] =
                $"Producto \"{dto.Nombre}\" actualizado correctamente.";

            return RedirectToAction(
                nameof(Detalle),
                new { id = dto.IdProducto });
        }

        private async Task<string> GuardarImagenAsync(IFormFile imagen)
        {
            var carpeta = Path.Combine(
                _env.WebRootPath,
                "images",
                "productos");

            Directory.CreateDirectory(carpeta);

            var extension = Path.GetExtension(imagen.FileName);

            var nombreArchivo =
                $"{Guid.NewGuid()}{extension}";

            var rutaCompleta =
                Path.Combine(carpeta, nombreArchivo);

            using (var stream =
                   new FileStream(rutaCompleta, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            return $"/images/productos/{nombreArchivo}";
        }
    }
}