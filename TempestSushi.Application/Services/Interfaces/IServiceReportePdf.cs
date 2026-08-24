using TempestSushi.Application.DTOs;

namespace TempestSushi.Application.Services.Interfaces
{
    public interface IServiceReportePdf
    {
        byte[] GenerarReporte(
            ReporteDashboardDTO reporte);
    }
}