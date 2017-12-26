using System.Collections.Generic;
using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public interface IAulasRepository : IRepository<Aulas>
    {
        IEnumerable<AulasDTO> GetByTipoCapacidad(byte idTipo, byte capacidad);
        List<AulasDetailsDTO> GetTest(string searchBy, int take, int skip, string sortBy, bool sortDir, out int filteredResultsCount, out int totalResultsCount);
    }
}
