using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempestSushi.Application.DTOs;

public interface IMenuService
{
    Task<List<MenuDto>> ObtenerListadoAsync();

    Task<MenuDisponibleDto?> ObtenerMenuDisponibleAsync();

    Task<MenuFormDto> PrepararCrearAsync();

    Task CrearAsync(MenuFormDto dto);

    Task<MenuFormDto?> ObtenerParaEditarAsync(int id);

    Task ActualizarAsync(MenuFormDto dto);

    Task EliminarAsync(int id);
    Task PrepararFormularioAsync(MenuFormDto dto);
}