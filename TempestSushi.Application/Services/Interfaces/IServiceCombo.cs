using TempestSushi.Application.DTOs;

namespace TempestSushi.Application.Services.Interfaces
{
    public interface IServiceCombo
    {
        Task<ICollection<ComboDTO>> ListAsync();
        Task<ComboDTO> FindByIdAsync(int id);
    }
}