using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Application.Services.Implementations
{
    public class ServiceProcesoPreparacion : IServiceProcesoPreparacion
    {
        private readonly IRepositoryProcesoPreparacion _repository;

        public ServiceProcesoPreparacion(IRepositoryProcesoPreparacion repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<ProcesoPreparacionListDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();

            var agrupado = list
                .GroupBy(p => new { p.IdProducto, p.IdProductoNavigation.Nombre })
                .Select(g => new ProcesoPreparacionListDTO
                {
                    IdProducto = g.Key.IdProducto,
                    NombreProducto = g.Key.Nombre,
                    CantidadPasos = g.Count()
                })
                .ToList();

            return agrupado;
        }

        public async Task<ProcesoPreparacionDetalleDTO> FindByProductoIdAsync(int idProducto)
        {
            var pasos = await _repository.FindByProductoIdAsync(idProducto);

            if (!pasos.Any()) return null!;

            return new ProcesoPreparacionDetalleDTO
            {
                IdProducto = idProducto,
                NombreProducto = pasos.First().IdProductoNavigation.Nombre,
                Estaciones = pasos.Select(p => new EstacionDTO
                {
                    NumeroPaso = p.NumeroPaso,
                    NombreEstacion = p.IdEstacionCocinaNavigation.Nombre
                }).ToList()
            };
        }
    }
}