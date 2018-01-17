using System.Collections.Generic;
using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public interface IDisponibilidadProfesorRepository
        : IRepository<DisponibilidadProfesores>
    {
        IEnumerable<DisponibilidadProfesorDetailsDTO> GetAllCurrent();

        DisponibilidadProfesorDetailsDTO GetByCedula(int cedula);

        DisponibilidadProfesorDetailsDTO GetByCedulaDia(int cedula, byte idDia);

        IEnumerable<DisponibilidadProfesorDetailsDTO> GetByPrioridadMateria(byte idPrioridad, ushort codigo);

        byte GetHorasAsignadas(int cedula);

        void RemoveByCedula(uint cedula);
    }
}