using Schedule.Entities;

namespace Schedule.Web.Models.Repository
{
    public interface IUnitOfWork
    {
        string Token { set; }
        IAulasRepository AulasRepository { get; }
        ICarrerasRepository CarrerasRepository { get; }
        IDisponibilidadProfesorRepository DisponibilidadRepository { get; }
        IHorarioProfesorRepository HorarioProfesorRepository { get; }
        IMateriasRepository MateriasRepository { get; }
        IPeriodoCarreraRepository PeriodoCarreraRepository { get; }
        IPrioridadesRepository PrioridadesRepository { get; }
        IPrivilegiosRepository PrivilegiosRepository { get; }
        IProfesorMateriaRepository ProfesorMateriaRepository { get; }
        IProfesorRepository ProfesorRepository { get; }
        ISeccionesRepository SeccionesRepository { get; }
        ISemestresRepository SemestresRepository { get; }
        IUsuarioRepository UsuarioRepository  { get; }
    }
}
