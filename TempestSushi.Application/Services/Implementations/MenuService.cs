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
        private readonly IRepositoryMenu _repo;
        private readonly IMapper _mapper;

        private static readonly string[] DiasOrdenados =
            { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

        public MenuService(IRepositoryMenu repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<MenuDto>> ObtenerListadoAsync()
        {
            var menus = await _repo.ListConDetalleAsync();

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
            var menus = await _repo.ListConDetalleAsync();
            var ahora = DateTime.Now;
            var fechaActual = DateOnly.FromDateTime(ahora);
            var horaActual = TimeOnly.FromDateTime(ahora);

            var menuVigente = menus.FirstOrDefault(m => EsVigente(m, fechaActual, horaActual, ahora.DayOfWeek));
            if (menuVigente == null) return null;

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
    }
}