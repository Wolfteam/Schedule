using System.Net.Http;

namespace Schedule.API.Models.Repositories
{
    public class UnitOfWork
    {
        public AulasRepository Aulas { get; private set; }
        public HorarioProfesorRepository HorarioProfesor { get; private set; }
        public MateriasRepository Materias { get; private set; }
        public ProfesorRepository Profesores { get; private set; }
        public SeccionesRepository Secciones { get; private set; }
        public DisponibilidadProfesorRepository DisponibilidadProfesor { get; private set; }

        public UnitOfWork()
        {
            Aulas = new AulasRepository();
            DisponibilidadProfesor = new DisponibilidadProfesorRepository();
            HorarioProfesor = new HorarioProfesorRepository();
            Materias = new MateriasRepository();
            Profesores = new ProfesorRepository();
            Secciones = new SeccionesRepository();
        }
    }
}