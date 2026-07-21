using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Models;
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

        public async Task<ProductoFormDto> ObtenerParaCrearAsync()
        {
            var dto = new ProductoFormDto();
            await CargarListasAsync(dto);
            return dto;
        }

        public async Task<ProductoFormDto?> ObtenerParaEditarAsync(int id)
        {
            var producto = await _repo.FindByIdAsync(id);
            if (producto == null) return null;

            var dto = new ProductoFormDto
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                IdCategoria = producto.IdCategoria,
                IngredientesSeleccionados = producto.IdIngredientes.Select(i => i.IdIngrediente).ToList(),
                ImagenActualUrl = producto.ImagenProductos.FirstOrDefault(i => i.EsPrincipal)?.UrlImagen
            };
            await CargarListasAsync(dto);
            return dto;
        }

        public async Task CargarListasAsync(ProductoFormDto dto)
        {
            var categorias = await _repo.ListCategoriasActivasAsync();
            dto.CategoriasDisponibles = categorias
                .Select(c => new CategoriaOptionDto { IdCategoria = c.IdCategoria, Nombre = c.Nombre })
                .ToList();

            var ingredientes = await _repo.ListIngredientesActivosAsync();
            dto.IngredientesDisponibles = ingredientes
                .Select(i => new IngredienteOptionDto { IdIngrediente = i.IdIngrediente, Nombre = i.Nombre })
                .ToList();
        }

        public async Task<bool> ExisteNombreAsync(string nombre, int? idExcluir = null)
            => await _repo.ExisteNombreAsync(nombre, idExcluir);

        public async Task CrearAsync(ProductoFormDto dto, string? imagenUrl)
        {
            var ingredientes = await _repo.ListIngredientesByIdsAsync(dto.IngredientesSeleccionados);

            var producto = new Producto
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion.Trim(),
                Precio = dto.Precio,
                IdCategoria = dto.IdCategoria,
                Activo = true
            };

            foreach (var ingrediente in ingredientes)
                producto.IdIngredientes.Add(ingrediente);

            if (!string.IsNullOrEmpty(imagenUrl))
            {
                producto.ImagenProductos.Add(new ImagenProducto
                {
                    UrlImagen = imagenUrl,
                    EsPrincipal = true,
                    Orden = 1
                });
            }

            await _repo.AddAsync(producto);
        }

        public async Task<bool> ActualizarAsync(ProductoFormDto dto, string? imagenUrl)
        {
            var producto = await _repo.FindByIdAsync(dto.IdProducto);
            if (producto == null) return false;

            producto.Nombre = dto.Nombre.Trim();
            producto.Descripcion = dto.Descripcion.Trim();
            producto.Precio = dto.Precio;
            producto.IdCategoria = dto.IdCategoria;

            producto.IdIngredientes.Clear();
            var ingredientes = await _repo.ListIngredientesByIdsAsync(dto.IngredientesSeleccionados);
            foreach (var ingrediente in ingredientes)
                producto.IdIngredientes.Add(ingrediente);

            if (!string.IsNullOrEmpty(imagenUrl))
            {
                var principal = producto.ImagenProductos.FirstOrDefault(i => i.EsPrincipal);
                if (principal != null)
                    principal.UrlImagen = imagenUrl;
                else
                    producto.ImagenProductos.Add(new ImagenProducto { UrlImagen = imagenUrl, EsPrincipal = true, Orden = 1 });
            }

            await _repo.UpdateAsync(producto);
            return true;
        }
    }
}