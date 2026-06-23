using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempestSushi.Application.DTOs;

namespace TempestSushi.Application.Services.Interfaces
{
    public interface IMenuService
    {
        Task<List<MenuDto>> ObtenerListadoAsync();
        Task<MenuDisponibleDto?> ObtenerMenuDisponibleAsync();
    }
}