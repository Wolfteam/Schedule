using System.Threading.Tasks;
using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public interface IPeriodoCarreraRepository : IRepository<PeriodoCarreraDTO, PeriodoCarreraDTO>
    {
        /// <summary>
        /// Obtiene el periodo academico actual
        /// </summary>
        /// <returns>PeriodoCarreraDTO</returns>
        Task<PeriodoCarreraDTO> GetCurrentPeriodoAsync();
    }
}
