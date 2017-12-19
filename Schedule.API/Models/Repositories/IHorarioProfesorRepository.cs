using System.Collections.Generic;
using Schedule.Entities;

namespace Schedule.API.Models.Repositories
{
    public interface IHorarioProfesorRepository : IRepository<HorarioProfesores>
    {
        int CalculateHorasAsignadas(uint cedula);

        bool RecordsExists();

        IEnumerable<HorarioProfesorDetailsDTO> GetByAula(int idAula);

        IEnumerable<HorarioProfesorDTO> GetByCedulaDia(uint cedula, byte idDia);

        IEnumerable<HorarioProfesorDTO> GetByDiaAula(byte idDia, byte idAula);

        IEnumerable<HorarioProfesorDTO> GetBySemestreDia(byte idSemestre, byte idDia);

        IEnumerable<HorarioProfesorDetailsDTO> GetBySemestre(int idSemestre);

        int GetLastSeccionAssigned(ushort codigo);
    }
}