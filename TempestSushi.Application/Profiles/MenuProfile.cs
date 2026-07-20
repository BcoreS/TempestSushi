using AutoMapper;
using TempestSushi.Application.DTOs;
using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Application.Profiles
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Producto, MenuItemDto>()
                .ForMember(
                    destination => destination.Tipo,
                    options => options.MapFrom(source => "Producto"));

            CreateMap<Combo, MenuItemDto>()
                .ForMember(
                    destination => destination.Tipo,
                    options => options.MapFrom(source => "Combo"));

            CreateMap<MenuFormDto, Menu>()
                .ForMember(
                    destination => destination.MenuProductos,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.MenuCombos,
                    options => options.Ignore());

            CreateMap<Menu, MenuFormDto>()
                .ForMember(
                    destination => destination.ProductosSeleccionados,
                    options => options.MapFrom(source =>
                        source.MenuProductos
                            .Where(mp => mp.Activo)
                            .Select(mp => mp.IdProducto)))
                .ForMember(
                    destination => destination.CombosSeleccionados,
                    options => options.MapFrom(source =>
                        source.MenuCombos
                            .Where(mc => mc.Activo)
                            .Select(mc => mc.IdCombo)))
                .ForMember(
                    destination => destination.ProductosDisponibles,
                    options => options.Ignore())
                .ForMember(
                    destination => destination.CombosDisponibles,
                    options => options.Ignore());
        }
    }
}