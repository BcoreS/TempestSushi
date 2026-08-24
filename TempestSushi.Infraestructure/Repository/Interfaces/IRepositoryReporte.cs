using TempestSushi.Infraestructure.Models;

    namespace TempestSushi.Infraestructure.Repository.Interfaces
    {
        public interface IRepositoryReporte
        {
            Task<ICollection<Pedido>> ListPedidosParaReporteAsync();
        }
    }