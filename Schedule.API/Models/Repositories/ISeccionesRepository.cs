using System.Collections.Generic;
using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public interface ISeccionesRepository : IRepository<Secciones>
    {
        IEnumerable<SeccionesDetailsDTO> GetAllCurrent();

        SeccionesDetailsDTO GetCurrent(ushort codigo);
    }
}