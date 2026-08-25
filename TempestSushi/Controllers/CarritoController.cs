using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TempestSushi.Application.Options;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Web.Controllers
{
    // El carrito es una experiencia exclusiva del Cliente:
    // Encargado/Administrador siguen registrando pedidos directo en /Pedido/Create.
    // El checkout completo (entrega, pago, registrar) vive en esta misma vista;
    // nunca se navega a /Pedido/Create.
    [Authorize(Roles = "Cliente")]
    public class CarritoController : Controller
    {
        private readonly IServicePedido _servicePedido;
        private readonly EnvioOptions _envioOptions;

        public CarritoController(IServicePedido servicePedido, IOptions<EnvioOptions> envioOptions)
        {
            _servicePedido = servicePedido;
            _envioOptions = envioOptions.Value;
        }

        public async Task<IActionResult> Index()
        {
            // Reutiliza el mismo método que usa Pedido/Create: nos da
            // MetodosEntrega y MetodosPago ya armados. Los campos de
            // Productos/Combos/ClientesDisponibles no se usan aquí,
            // porque el Carrito nunca deja elegir producto desde un combobox.
            var datos = await _servicePedido.ObtenerDatosFormularioAsync();
            ViewBag.CostoEnvio = _envioOptions.CostoEnvio;
            return View(datos);
        }
    }
}
