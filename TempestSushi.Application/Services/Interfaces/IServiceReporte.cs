using TempestSushi.Application.DTOs;

namespace TempestSushi.Application.Services.Interfaces
{
    public interface IServiceReporte
    {
        Task<ReporteDashboardDTO> ObtenerDashboardAsync();
    }
}