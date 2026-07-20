using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Application.Services.Implementations
{
    public class MenuService : IMenuService
    {
        private readonly IRepositoryMenu _repoMenu;
        private readonly IRepositoryProducto _repoProducto;
        private readonly IRepositoryCombo _repoCombo;
        private readonly IMapper _mapper;

        private static readonly string[] DiasOrdenados =
            { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

        public MenuService(
    IRepositoryMenu repoMenu,
    IRepositoryProducto repoProducto,
    IRepositoryCombo repoCombo,
    IMapper mapper)
        {
            _repoMenu = repoMenu;
            _repoProducto = repoProducto;
            _repoCombo = repoCombo;
            _mapper = mapper;
        }

        public async Task<List<MenuDto>> ObtenerListadoAsync()
        {
            var menus = await _repoMenu.ListConDetalleAsync();
            return menus
                .OrderByDescending(m => m.FechaInicio)
                .Select(m => new MenuDto
                {
                    IdMenu = m.IdMenu,
                    Nombre = m.Nombre,
                    Dias = m.DiasDisponibles,
                    Horario = $"{FormatearHora(m.HoraInicio)} - {FormatearHora(m.HoraFin)}",
                    Vigencia = FormatearVigencia(m.FechaInicio, m.FechaFin)
                })
                .ToList();
        }

        public async Task<MenuDisponibleDto?> ObtenerMenuDisponibleAsync()
        {
            var menus = await _repoMenu.ListConDetalleAsync(); var ahora = DateTime.Now;
            var fechaActual = DateOnly.FromDateTime(ahora);
            var horaActual = TimeOnly.FromDateTime(ahora);

            var menuVigente = menus
                .Where(m => EsVigente(m, fechaActual, horaActual, ahora.DayOfWeek))
                .OrderByDescending(m => m.FechaInicio)
                .ThenByDescending(m => m.IdMenu)
                .FirstOrDefault(); if (menuVigente == null) return null;

            var itemsConCategoria = new List<(string Categoria, MenuItemDto Item)>();

            foreach (var mp in menuVigente.MenuProductos.Where(mp => mp.Activo))
            {
                var producto = mp.IdProductoNavigation;
                var itemDto = _mapper.Map<MenuItemDto>(producto);
                itemsConCategoria.Add((producto.IdCategoriaNavigation.Nombre, itemDto));
            }

            foreach (var mc in menuVigente.MenuCombos.Where(mc => mc.Activo))
            {
                var combo = mc.IdComboNavigation;
                var itemDto = _mapper.Map<MenuItemDto>(combo);
                itemsConCategoria.Add((combo.IdCategoriaNavigation.Nombre, itemDto));
            }

            var categorias = itemsConCategoria
                .GroupBy(x => x.Categoria)
                .Select(g => new MenuCategoriaDto
                {
                    NombreCategoria = g.Key,
                    Items = g.Select(x => x.Item).ToList()
                })
                .ToList();

            return new MenuDisponibleDto
            {
                Nombre = menuVigente.Nombre,
                Dias = menuVigente.DiasDisponibles,
                Horario = $"{FormatearHora(menuVigente.HoraInicio)} - {FormatearHora(menuVigente.HoraFin)}",
                Vigencia = FormatearVigencia(menuVigente.FechaInicio, menuVigente.FechaFin),
                Categorias = categorias
            };
        }

        // ---------- Métodos auxiliares ----------

        private bool EsVigente(Menu menu, DateOnly fechaActual, TimeOnly horaActual, DayOfWeek diaSemana)
        {
            bool dentroDeFechas = menu.FechaInicio <= fechaActual
                && (menu.FechaFin == null || menu.FechaFin.Value >= fechaActual);

            if (!dentroDeFechas) return false;

            string diaActual = ObtenerDiaEnEspanol(diaSemana);
            if (!DiaEstaEnRango(menu.DiasDisponibles, diaActual)) return false;

            bool dentroDeHorario = horaActual >= menu.HoraInicio && horaActual <= menu.HoraFin;
            return dentroDeHorario;
        }

        private bool DiaEstaEnRango(string diasDisponibles, string diaActual)
        {
            if (diasDisponibles.Contains(" a ", StringComparison.OrdinalIgnoreCase))
            {
                var partes = diasDisponibles.Split(" a ", StringSplitOptions.TrimEntries);
                if (partes.Length != 2) return false;

                int idxInicio = Array.FindIndex(DiasOrdenados, d => string.Equals(d, partes[0], StringComparison.OrdinalIgnoreCase));
                int idxFin = Array.FindIndex(DiasOrdenados, d => string.Equals(d, partes[1], StringComparison.OrdinalIgnoreCase));
                int idxActual = Array.FindIndex(DiasOrdenados, d => string.Equals(d, diaActual, StringComparison.OrdinalIgnoreCase));

                if (idxInicio == -1 || idxFin == -1 || idxActual == -1) return false;

                if (idxInicio <= idxFin)
                    return idxActual >= idxInicio && idxActual <= idxFin;
                else
                    return idxActual >= idxInicio || idxActual <= idxFin;
            }

            return diasDisponibles
                .Split(',')
                .Select(d => d.Trim())
                .Any(d => string.Equals(d, diaActual, StringComparison.OrdinalIgnoreCase));
        }

        private string ObtenerDiaEnEspanol(DayOfWeek dia)
        {
            return dia switch
            {
                DayOfWeek.Monday => "Lunes",
                DayOfWeek.Tuesday => "Martes",
                DayOfWeek.Wednesday => "Miércoles",
                DayOfWeek.Thursday => "Jueves",
                DayOfWeek.Friday => "Viernes",
                DayOfWeek.Saturday => "Sábado",
                DayOfWeek.Sunday => "Domingo",
                _ => ""
            };
        }

        private string FormatearHora(TimeOnly hora)
        {
            return hora.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        }

        private string FormatearVigencia(DateOnly inicio, DateOnly? fin)
        {
            return fin.HasValue
                ? $"{inicio:dd/MM/yyyy} - {fin:dd/MM/yyyy}"
                : $"Desde {inicio:dd/MM/yyyy}";
        }

        public async Task<MenuFormDto> PrepararCrearAsync()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);

            var dto = new MenuFormDto
            {
                FechaInicio = hoy,
                FechaFin = hoy.AddDays(30),

                HoraInicio = new TimeOnly(11, 0),
                HoraFin = new TimeOnly(22, 0),

                DiasDisponibles = "Lunes a Domingo",

                Activo = true
            };

            await CargarOpcionesFormularioAsync(dto);

            return dto;
        }
        public async Task CrearAsync(MenuFormDto dto)
        {
            var menu = _mapper.Map<Menu>(dto);

            foreach (var idProducto in dto.ProductosSeleccionados.Distinct())
            {
                menu.MenuProductos.Add(new MenuProducto
                {
                    IdProducto = idProducto,
                    Activo = true
                });
            }

            foreach (var idCombo in dto.CombosSeleccionados.Distinct())
            {
                menu.MenuCombos.Add(new MenuCombo
                {
                    IdCombo = idCombo,
                    Activo = true
                });
            }

            await _repoMenu.CreateAsync(menu);
        }

        public async Task<MenuFormDto?> ObtenerParaEditarAsync(int id)
        {
            var menu = await _repoMenu.FindByIdAsync(id);

            if (menu == null)
            {
                return null;
            }

            var dto = new MenuFormDto
            {
                IdMenu = menu.IdMenu,
                Nombre = menu.Nombre,
                FechaInicio = menu.FechaInicio,
                FechaFin = menu.FechaFin,
                HoraInicio = menu.HoraInicio,
                HoraFin = menu.HoraFin,
                DiasDisponibles = menu.DiasDisponibles,
                Activo = menu.Activo,

                ProductosSeleccionados = menu.MenuProductos
                    .Where(mp => mp.Activo)
                    .Select(mp => mp.IdProducto)
                    .ToList(),

                CombosSeleccionados = menu.MenuCombos
                    .Where(mc => mc.Activo)
                    .Select(mc => mc.IdCombo)
                    .ToList()
            };

            await CargarOpcionesFormularioAsync(dto);

            return dto;
        }

        public async Task ActualizarAsync(MenuFormDto dto)
        {
            var menu = _mapper.Map<Menu>(dto);

            await _repoMenu.UpdateWithRelationsAsync(
                menu,
                dto.ProductosSeleccionados,
                dto.CombosSeleccionados);
        }
        public async Task EliminarAsync(int id)
        {
            await _repoMenu.DeleteAsync(id);
        }

        //Método privado para cargar cosas del formulario

        private async Task CargarOpcionesFormularioAsync(MenuFormDto dto)
        {
            var productos = await _repoProducto.ListAsync();
            var combos = await _repoCombo.ListAsync();

            dto.ProductosDisponibles = productos
                .Select(p => new SeleccionMenuDto
                {
                    Id = p.IdProducto,
                    Nombre = p.Nombre
                })
                .OrderBy(p => p.Nombre)
                .ToList();

            dto.CombosDisponibles = combos
                .Select(c => new SeleccionMenuDto
                {
                    Id = c.IdCombo,
                    Nombre = c.Nombre
                })
                .OrderBy(c => c.Nombre)
                .ToList();

            dto.OpcionesDiasDisponibles = new List<string>
    {
        "Lunes a Viernes",
        "Lunes a Sábado",
        "Lunes a Domingo",
        "Sábado, Domingo",
        "Lunes, Miércoles, Viernes",
        "Martes, Jueves"
    };
        }
        public async Task PrepararFormularioAsync(MenuFormDto dto)
        {
            await CargarOpcionesFormularioAsync(dto);
        }
    }
}