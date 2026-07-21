using AutoMapper;
using System.Linq;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Application.Services.Implementations
{
    public class ServiceCombo : IServiceCombo
    {
        private readonly IRepositoryCombo _repository;
        private readonly IMapper _mapper;

        public ServiceCombo(IRepositoryCombo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ComboDTO> FindByIdAsync(int id)
        {
            var entity = await _repository.FindByIdAsync(id);
            return _mapper.Map<ComboDTO>(entity);
        }

        public async Task<ICollection<ComboDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            return _mapper.Map<ICollection<ComboDTO>>(list);
        }

        public async Task<ComboFormDto> ObtenerParaCrearAsync()
        {
            var dto = new ComboFormDto();
            await CargarListasAsync(dto);
            return dto;
        }

        public async Task<ComboFormDto?> ObtenerParaEditarAsync(int id)
        {
            var combo = await _repository.FindByIdAsync(id);
            if (combo == null) return null;

            var dto = new ComboFormDto
            {
                IdCombo = combo.IdCombo,
                Nombre = combo.Nombre,
                Descripcion = combo.Descripcion,
                Precio = combo.Precio,
                ProductosSeleccionados = combo.ComboProductos.Select(cp => cp.IdProducto).ToList()
            };
            await CargarListasAsync(dto);
            return dto;
        }

        public async Task CargarListasAsync(ComboFormDto dto)
        {
            var productos = await _repository.ListProductosActivosAsync();
            dto.ProductosDisponibles = productos
                .Select(p => new ProductoOptionDto { IdProducto = p.IdProducto, Nombre = p.Nombre, Precio = p.Precio })
                .ToList();
        }

        public async Task<bool> ExisteNombreAsync(string nombre, int? idExcluir = null)
            => await _repository.ExisteNombreAsync(nombre, idExcluir);

        public async Task CrearAsync(ComboFormDto dto)
        {
            var idCategoria = await _repository.ObtenerIdCategoriaComboAsync();

            var combo = new Combo
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion.Trim(),
                Precio = dto.Precio,
                IdCategoria = idCategoria,
                Activo = true
            };

            foreach (var idProducto in dto.ProductosSeleccionados.Distinct())
                combo.ComboProductos.Add(new ComboProducto { IdProducto = idProducto, Cantidad = 1 });

            await _repository.AddAsync(combo);
        }

        public async Task<bool> ActualizarAsync(ComboFormDto dto)
        {
            var combo = await _repository.FindByIdAsync(dto.IdCombo);
            if (combo == null) return false;
            combo.Nombre = dto.Nombre.Trim();
            combo.Descripcion = dto.Descripcion.Trim();
            combo.Precio = dto.Precio;

            await _repository.UpdateAsync(combo, dto.ProductosSeleccionados.Distinct().ToList());
            return true;
        }
    }
}