using System.Collections.Generic;
using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public interface IMateriasRepository : IRepository<Materias>
    {
        IEnumerable<MateriasDTO> GetBySemestre(int idSemestre);
    }
}