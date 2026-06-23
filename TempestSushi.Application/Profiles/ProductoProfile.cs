using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempestSushi.Application.DTOs;
using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Application.Profiles
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile()
        {
            CreateMap<Producto, ProductoDto>()
                .ForMember(dest => dest.Categoria,
                    opt => opt.MapFrom(src => src.IdCategoriaNavigation.Nombre))
                .ForMember(dest => dest.ImagenUrl,
                    opt => opt.MapFrom(src => src.ImagenProductos.Any(i => i.EsPrincipal)
                        ? src.ImagenProductos.First(i => i.EsPrincipal).UrlImagen
                        : null));

            CreateMap<Producto, ProductoDetalleDto>()
                .ForMember(dest => dest.Categoria,
                    opt => opt.MapFrom(src => src.IdCategoriaNavigation.Nombre))
                .ForMember(dest => dest.ImagenUrl,
                    opt => opt.MapFrom(src => src.ImagenProductos.Any(i => i.EsPrincipal)
                        ? src.ImagenProductos.First(i => i.EsPrincipal).UrlImagen
                        : null))
                .ForMember(dest => dest.Ingredientes,
                    opt => opt.MapFrom(src => src.IdIngredientes.Select(i => i.Nombre).ToList()));
        }
    }
}