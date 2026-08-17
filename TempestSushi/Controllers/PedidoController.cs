using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using Microsoft.Extensions.Options;
using TempestSushi.Application.Options;

namespace TempestSushi.Web.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private readonly IServicePedido _servicePedido;
        private readonly IUsuarioActualService _usuarioActual;
        private readonly EnvioOptions _envioOptions;
        public PedidoController(IServicePedido servicePedido, IUsuarioActualService usuarioActual, IOptions<EnvioOptions> envioOptions)
        {
            _servicePedido = servicePedido;
            _usuarioActual = usuarioActual;
            _envioOptions = envioOptions.Value;
        }

        // Historial - filtrado automáticamente por rol dentro del Service
        public async Task<IActionResult> Index(DateTime? fecha, int? idEstadoPedido)
        {
            var historial = await _servicePedido.ObtenerHistorialAsync(fecha, idEstadoPedido);
            var esEncargadoOAdmin = _usuarioActual.Rol != "Cliente";
            ViewBag.EsEncargadoOAdmin = esEncargadoOAdmin;

            if (esEncargadoOAdmin)
            {
                ViewBag.Estados = await _servicePedido.ObtenerEstadosAsync();
            }

            ViewBag.FechaFiltro = fecha?.ToString("yyyy-MM-dd");
            ViewBag.EstadoFiltro = idEstadoPedido;

            return View(historial);
        }

        // Detalle formato factura
        public async Task<IActionResult> Details(int id)
        {
            var pedido = await _servicePedido.ObtenerDetalleAsync(id);
            if (pedido is null)
                return NotFound(); // también cubre el caso de "no es tu pedido"

            return View(pedido);
        }

        // Formulario de registro
        public async Task<IActionResult> Create()
        {
            var datos = await _servicePedido.ObtenerDatosFormularioAsync();
            datos.EncargadoNombre = User.FindFirstValue(ClaimTypes.Name);
            ViewBag.CostoEnvio = _envioOptions.CostoEnvio;
            return View(datos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] PedidoRegistroDto registro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var pedidoCreado = await _servicePedido.RegistrarAsync(registro);
                return Ok(pedidoCreado); // el JS del formulario redirige/notifica con esta respuesta
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // Endpoint asíncrono: recalcula una línea al agregarla/cambiarla en el formulario
        [HttpPost]
        public async Task<IActionResult> CalcularLinea([FromBody] PedidoLineaEntradaDto entrada)
        {
            try
            {
                var linea = await _servicePedido.CalcularLineaAsync(entrada);
                return Ok(linea);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}