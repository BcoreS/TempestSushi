using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TempestSushi.Application.Services.Interfaces;

namespace TempestSushi.Web.Controllers
{
    [Authorize(Roles = "Encargado,Administrador")]
    public class ReporteController : Controller
    {
        private readonly IServiceReporte _serviceReporte;
        private readonly IServiceReportePdf _serviceReportePdf;

        public ReporteController(
            IServiceReporte serviceReporte,
            IServiceReportePdf serviceReportePdf)
        {
            _serviceReporte = serviceReporte;
            _serviceReportePdf = serviceReportePdf;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model =
                await _serviceReporte.ObtenerDashboardAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DescargarPdf()
        {
            var reporte =
                await _serviceReporte.ObtenerDashboardAsync();

            var pdf =
                _serviceReportePdf.GenerarReporte(reporte);

            var nombreArchivo =
                $"Reporte-TempestSushi-{DateTime.Now:yyyyMMdd-HHmm}.pdf";

            return File(
                pdf,
                "application/pdf",
                nombreArchivo);
        }
    }
}