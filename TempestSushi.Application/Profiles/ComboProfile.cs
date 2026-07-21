using AutoMapper;
using System.Linq;
using TempestSushi.Application.DTOs;
using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Application.Profiles
{
    public class ComboProfile : Profile
    {
        public ComboProfile()
        {
            CreateMap<Combo, ComboDTO>()
                .ForMember(dest => dest.NombreCategoria,
                    opt => opt.MapFrom(src => src.IdCategoriaNavigation.Nombre))
                .ForMember(dest => dest.Productos,
                    opt => opt.MapFrom(src => src.ComboProductos.Select(cp => new ComboProductoItemDTO
                    {
                        IdProducto = cp.IdProducto,
                        NombreProducto = cp.IdProductoNavigation.Nombre,
                        Cantidad = cp.Cantidad
                    })));

            CreateMap<ComboDTO, Combo>()
                .ForMember(dest => dest.IdCategoriaNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.ComboProductos, opt => opt.Ignore())
                .ForMember(dest => dest.CarritoCombos, opt => opt.Ignore())
                .ForMember(dest => dest.MenuCombos, opt => opt.Ignore())
                .ForMember(dest => dest.PedidoDetalleCombos, opt => opt.Ignore());
        }
    }
}