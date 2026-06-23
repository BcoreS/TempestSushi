using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Application.Services.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly IRepositoryProducto _repo;
        private readonly IMapper _mapper;

        public ProductoService(IRepositoryProducto repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ProductoDto>> ObtenerListadoAsync()
        {
            var productos = await _repo.ListAsync();
            return _mapper.Map<List<ProductoDto>>(productos.ToList());
        }

        public async Task<ProductoDetalleDto?> ObtenerDetalleAsync(int id)
        {
            var producto = await _repo.FindByIdAsync(id);
            if (producto == null) return null;
            return _mapper.Map<ProductoDetalleDto>(producto);
        }
    }
}
