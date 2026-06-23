using AutoMapper;
using TempestSushi.Application.DTOs;
using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Application.Profiles
{
    public class ComboProfile : Profile
    {
        public ComboProfile()
        {
            CreateMap<Combo, ComboDTO>()
                .ForMember(
                    dest => dest.NombreCategoria,
                    opt => opt.MapFrom(src => src.IdCategoriaNavigation.Nombre)
                );

            CreateMap<ComboDTO, Combo>()
                .ForMember(
                    dest => dest.IdCategoriaNavigation,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.ComboProductos,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.CarritoCombos,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.MenuCombos,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.PedidoDetalleCombos,
                    opt => opt.Ignore()
                );
        }
    }
}