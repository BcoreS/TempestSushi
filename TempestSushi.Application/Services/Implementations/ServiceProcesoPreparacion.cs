using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Models;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Application.Services.Implementations
{
    public class ServiceProcesoPreparacion : IServiceProcesoPreparacion
    {
        private readonly IRepositoryProcesoPreparacion _repositoryProceso;
        private readonly IRepositoryProducto _repositoryProducto;
        private readonly IRepositoryEstacionCocina _repositoryEstacion;

        public ServiceProcesoPreparacion(
            IRepositoryProcesoPreparacion repositoryProceso,
            IRepositoryProducto repositoryProducto,
            IRepositoryEstacionCocina repositoryEstacion)
        {
            _repositoryProceso = repositoryProceso;
            _repositoryProducto = repositoryProducto;
            _repositoryEstacion = repositoryEstacion;
        }

        public async Task<ICollection<ProcesoPreparacionListDTO>> ListAsync()
        {
            var list = await _repositoryProceso.ListAsync();

            return list
                .GroupBy(p => new
                {
                    p.IdProducto,
                    p.IdProductoNavigation.Nombre
                })
                .Select(g => new ProcesoPreparacionListDTO
                {
                    IdProducto = g.Key.IdProducto,
                    NombreProducto = g.Key.Nombre,
                    CantidadPasos = g.Count()
                })
                .OrderBy(p => p.NombreProducto)
                .ToList();
        }

        public async Task<ProcesoPreparacionDetalleDTO?>
            FindByProductoIdAsync(int idProducto)
        {
            var pasos =
                await _repositoryProceso.FindByProductoIdAsync(idProducto);

            if (!pasos.Any())
            {
                return null;
            }

            return new ProcesoPreparacionDetalleDTO
            {
                IdProducto = idProducto,
                NombreProducto =
                    pasos.First().IdProductoNavigation.Nombre,

                Estaciones = pasos
                    .OrderBy(p => p.NumeroPaso)
                    .Select(p => new EstacionDTO
                    {
                        NumeroPaso = p.NumeroPaso,
                        NombreEstacion =
                            p.IdEstacionCocinaNavigation.Nombre
                    })
                    .ToList()
            };
        }

        public async Task<ICollection<ProcesoPreparacionMantenimientoDTO>>
            ListMantenimientoAsync()
        {
            var procesos = await _repositoryProceso.ListAsync();

            return procesos
                .GroupBy(p => new
                {
                    p.IdProducto,
                    p.IdProductoNavigation.Nombre
                })
                .Select(g => new ProcesoPreparacionMantenimientoDTO
                {
                    IdProducto = g.Key.IdProducto,
                    NombreProducto = g.Key.Nombre,
                    CantidadPasos = g.Count(),

                    TiempoEstimadoTotal = g.Sum(
                        p => p.TiempoEstimadoMinutos ?? 0)
                })
                .OrderBy(p => p.NombreProducto)
                .ToList();
        }

        public async Task<ProcesoPreparacionFormDTO>
            PrepararCrearAsync()
        {
            var dto = new ProcesoPreparacionFormDTO
            {
                Pasos = new List<PasoPreparacionFormDTO>
                {
                    new PasoPreparacionFormDTO
                    {
                        NumeroPaso = 1
                    }
                }
            };

            await CargarOpcionesFormularioAsync(dto);

            return dto;
        }

        public async Task PrepararFormularioAsync(
            ProcesoPreparacionFormDTO dto)
        {
            await CargarOpcionesFormularioAsync(dto);
        }

        public async Task CrearAsync(
            ProcesoPreparacionFormDTO dto)
        {
            var productoYaTieneProceso =
                await _repositoryProceso.ExistsForProductoAsync(
                    dto.IdProducto);

            if (productoYaTieneProceso)
            {
                throw new InvalidOperationException(
                    "El producto seleccionado ya tiene un proceso de preparación.");
            }

            var pasos = ConvertirPasos(dto);

            await _repositoryProceso.CreateForProductoAsync(
                dto.IdProducto,
                pasos);
        }

        public async Task<ProcesoPreparacionFormDTO?>
            ObtenerParaEditarAsync(int idProducto)
        {
            var pasos =
                await _repositoryProceso.FindByProductoIdAsync(
                    idProducto);

            if (!pasos.Any())
            {
                return null;
            }

            var dto = new ProcesoPreparacionFormDTO
            {
                IdProducto = idProducto,
                NombreProducto =
                    pasos.First().IdProductoNavigation.Nombre,

                Pasos = pasos
                    .OrderBy(p => p.NumeroPaso)
                    .Select(p => new PasoPreparacionFormDTO
                    {
                        IdProcesoPreparacion =
                            p.IdProcesoPreparacion,

                        IdEstacionCocina =
                            p.IdEstacionCocina,

                        NumeroPaso =
                            p.NumeroPaso,

                        DescripcionPaso =
                            p.DescripcionPaso,

                        TiempoEstimadoMinutos =
                            p.TiempoEstimadoMinutos
                    })
                    .ToList()
            };

            await CargarOpcionesFormularioAsync(dto);

            return dto;
        }

        public async Task ActualizarAsync(
            ProcesoPreparacionFormDTO dto)
        {
            var procesoExistente =
                await _repositoryProceso.FindByProductoIdAsync(
                    dto.IdProducto);

            if (!procesoExistente.Any())
            {
                throw new KeyNotFoundException(
                    "No se encontró el proceso de preparación.");
            }

            var pasos = ConvertirPasos(dto);

            await _repositoryProceso.UpdateForProductoAsync(
                dto.IdProducto,
                pasos);
        }

        public async Task EliminarAsync(int idProducto)
        {
            await _repositoryProceso.DeleteByProductoIdAsync(
                idProducto);
        }

        private async Task CargarOpcionesFormularioAsync(
            ProcesoPreparacionFormDTO dto)
        {
            var productos =
                await _repositoryProducto.ListAsync();

            var estaciones =
                await _repositoryEstacion.ListAsync();

            var procesosExistentes =
                await _repositoryProceso.ListAsync();

            var idsProductosConProceso = procesosExistentes
                .Select(p => p.IdProducto)
                .Distinct()
                .ToHashSet();

            dto.ProductosDisponibles = productos
                .Where(p =>
                    !idsProductosConProceso.Contains(p.IdProducto)
                    || p.IdProducto == dto.IdProducto)
                .Select(p => new SeleccionDTO
                {
                    Id = p.IdProducto,
                    Nombre = p.Nombre
                })
                .OrderBy(p => p.Nombre)
                .ToList();

            dto.EstacionesDisponibles = estaciones
                .Select(e => new SeleccionDTO
                {
                    Id = e.IdEstacionCocina,
                    Nombre = e.Nombre
                })
                .OrderBy(e => e.Nombre)
                .ToList();
        }

        private static List<ProcesoPreparacion> ConvertirPasos(
            ProcesoPreparacionFormDTO dto)
        {
            return dto.Pasos
                .Select((paso, indice) => new ProcesoPreparacion
                {
                    IdProducto = dto.IdProducto,

                    IdEstacionCocina =
                        paso.IdEstacionCocina,

                    NumeroPaso =
                        indice + 1,

                    DescripcionPaso =
                        string.IsNullOrWhiteSpace(
                            paso.DescripcionPaso)
                            ? null
                            : paso.DescripcionPaso.Trim(),

                    TiempoEstimadoMinutos =
                        paso.TiempoEstimadoMinutos
                })
                .ToList();
        }
    }
}