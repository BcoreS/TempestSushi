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
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Producto, MenuItemDto>()
                .ForMember(d => d.Tipo, opt => opt.MapFrom(src => "Producto"));

            CreateMap<Combo, MenuItemDto>()
                .ForMember(d => d.Tipo, opt => opt.MapFrom(src => "Combo"));
        }
    }
}