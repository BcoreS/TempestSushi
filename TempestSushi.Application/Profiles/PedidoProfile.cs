using AutoMapper;
using System.Linq;
using TempestSushi.Application.DTOs;
using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Application.Profiles
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            // Línea de detalle - Producto
            CreateMap<PedidoDetalleProducto, PedidoLineaDto>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => "Producto"))
                .ForMember(dest => dest.IdItem, opt => opt.MapFrom(src => src.IdProducto))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.IdProductoNavigation.Nombre))
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.PrecioUnitario * src.Cantidad))
                .ForMember(dest => dest.Impuesto, opt => opt.Ignore())
                .ForMember(dest => dest.TotalLinea, opt => opt.Ignore());

            // Línea de detalle - Combo
            CreateMap<PedidoDetalleCombo, PedidoLineaDto>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => "Combo"))
                .ForMember(dest => dest.IdItem, opt => opt.MapFrom(src => src.IdCombo))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.IdComboNavigation.Nombre))
                .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.PrecioUnitario * src.Cantidad))
                .ForMember(dest => dest.Impuesto, opt => opt.Ignore())
                .ForMember(dest => dest.TotalLinea, opt => opt.Ignore());

            // Encabezado completo (Detalle de Pedido - formato factura)
            CreateMap<Pedido, PedidoDTO>()
                .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.IdClienteNavigation.Nombre + " " + src.IdClienteNavigation.Apellidos))
                .ForMember(dest => dest.ClienteIdentificador, opt => opt.MapFrom(src => src.IdClienteNavigation.Correo))
                .ForMember(dest => dest.EncargadoNombre, opt => opt.MapFrom(src => src.IdEmpleadoNavigation != null ? src.IdEmpleadoNavigation.Nombre + " " + src.IdEmpleadoNavigation.Apellidos : null))
                .ForMember(dest => dest.MetodoEntregaNombre, opt => opt.MapFrom(src => src.IdMetodoEntregaNavigation.Nombre))
                .ForMember(dest => dest.EstadoNombre, opt => opt.MapFrom(src => src.IdEstadoPedidoNavigation.Nombre))
                .ForMember(dest => dest.MetodoPagoNombre, opt => opt.MapFrom(src => src.Pagos.FirstOrDefault() != null ? src.Pagos.FirstOrDefault()!.IdMetodoPagoNavigation.Nombre : ""))
                .ForMember(dest => dest.Lineas, opt => opt.Ignore())
                .ForMember(dest => dest.TotalSinImpuestos, opt => opt.Ignore())
                .ForMember(dest => dest.TotalConImpuestos, opt => opt.Ignore());

            // Fila liviana del Historial
            CreateMap<Pedido, PedidoHistorialDto>()
                .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.IdClienteNavigation.Nombre + " " + src.IdClienteNavigation.Apellidos))
                .ForMember(dest => dest.EstadoNombre, opt => opt.MapFrom(src => src.IdEstadoPedidoNavigation.Nombre))
                .ForMember(dest => dest.MetodoEntregaNombre, opt => opt.MapFrom(src => src.IdMetodoEntregaNavigation.Nombre))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));
        }
    }
}