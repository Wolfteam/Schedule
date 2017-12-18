using System.Collections.Generic;
using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public interface IDisponibilidadProfesorRepository
        : IRepository<DisponibilidadProfesores>
    {
        IEnumerable<DisponibilidadProfesorDetailsDTO> GetAllCurrent();

        DisponibilidadProfesorDetailsDTO GetByCedula(int cedula);

        IEnumerable<DisponibilidadProfesorDetailsDTO> GetByPrioridadMateria(byte idPrioridad, ushort codigo);

        void RemoveByCedula(uint cedula);
    }
}