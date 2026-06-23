using AutoMapper;
using TempestSushi.Application.DTOs;
using TempestSushi.Application.Services.Interfaces;
using TempestSushi.Infraestructure.Repository.Interfaces;

namespace TempestSushi.Application.Services.Implementations
{
    public class ServiceProcesoPreparacion : IServiceProcesoPreparacion
    {
        private readonly IRepositoryProcesoPreparacion _repository;
        private readonly IMapper _mapper;

        public ServiceProcesoPreparacion(
            IRepositoryProcesoPreparacion repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProcesoPreparacionDTO> FindByIdAsync(int id)
        {
            var entity = await _repository.FindByIdAsync(id);
            var objectMapped = _mapper.Map<ProcesoPreparacionDTO>(entity);
            return objectMapped;
        }

        public async Task<ICollection<ProcesoPreparacionDTO>> ListAsync()
        {
            var list = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<ProcesoPreparacionDTO>>(list);
            return collection;
        }
    }
}