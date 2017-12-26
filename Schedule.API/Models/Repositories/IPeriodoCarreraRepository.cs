using System.Collections.Generic;
using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public interface IPeriodoCarreraRepository : IRepository<PeriodoCarrera>
    {
        PeriodoCarreraDTO GetCurrentPeriodo();
        void UpdateAllCurrentStatus();
    }
}