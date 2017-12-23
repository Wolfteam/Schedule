using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public interface IUnitOfWork
    {
        IAulasRepository AulasRepository { get; }
        ICarrerasRepository CarrerasRepository { get; }
        IDisponibilidadProfesorRepository DisponibilidadRepository { get; }
        IHorarioProfesorRepository HorarioProfesorRepository { get; }
        IMateriasRepository MateriasRepository { get; }
        IPeriodoCarreraRepository PeriodoCarreraRepository { get; }
        IPrioridadesRepository PrioridadesRepository { get; }
        IProfesorMateriaRepository ProfesorMateriaRepository { get; }
        IProfesorRepository ProfesorRepository { get; }
        ISeccionesRepository SeccionesRepository { get; }
        ISemestresRepository SemestresRepository { get; }
    }
}
