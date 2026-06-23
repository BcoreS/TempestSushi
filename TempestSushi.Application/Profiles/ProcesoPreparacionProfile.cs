using AutoMapper;
using TempestSushi.Application.DTOs;
using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Application.Profiles
{
    public class ProcesoPreparacionProfile : Profile
    {
        public ProcesoPreparacionProfile()
        {
            CreateMap<ProcesoPreparacion, ProcesoPreparacionDTO>()
                .ForMember(
                    dest => dest.NombreProducto,
                    opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre)
                )
                .ForMember(
                    dest => dest.NombreEstacionCocina,
                    opt => opt.MapFrom(src => src.IdEstacionCocinaNavigation.Nombre)
                );

            CreateMap<ProcesoPreparacionDTO, ProcesoPreparacion>()
                .ForMember(
                    dest => dest.IdProductoNavigation,
                    opt => opt.Ignore()
                )
                .ForMember(
                    dest => dest.IdEstacionCocinaNavigation,
                    opt => opt.Ignore()
                );
        }
    }
}