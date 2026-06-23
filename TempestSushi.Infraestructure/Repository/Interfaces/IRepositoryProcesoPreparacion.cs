using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempestSushi.Infraestructure.Models;

namespace TempestSushi.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryProcesoPreparacion
    {
        Task<ProcesoPreparacion> FindByIdAsync(int id);
        Task<ICollection<ProcesoPreparacion>> ListAsync();
    }
}
