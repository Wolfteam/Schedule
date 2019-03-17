using System;

namespace Schedule.API.Models.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Propiedades
        public IAulasRepository AulasRepository { get; }

        public ICarrerasRepository CarrerasRepository { get; }

        public IDisponibilidadProfesorRepository DisponibilidadProfesorRepository { get; }

        public IHorarioProfesorRepository HorarioProfesorRepository { get; }

        public IMateriasRepository MateriasRepository { get; }

        public IPeriodoCarreraRepository PeriodoCarreraRepository { get; }

        public IPrioridadesRepository PrioridadesRepository { get; }

        public IPrivilegiosRepository PrivilegiosRepository { get; }

        public IProfesorMateriaRepository ProfesorMateriaRepository { get; }

        public IProfesorRepository ProfesorRepository { get; }

        public ISeccionesRepository SeccionesRepository { get; }

        public ISemestresRepository SemestresRepository { get; }

        public IUsuarioRepository UsuarioRepository { get; }

        public ITipoAulaMateriaRepository TipoAulaMateriaRepository { get; }

        public ITokenRepository TokenRepository { get; }

        #endregion

        private readonly HorariosContext _horariosContext;
        private bool disposedValue = false; // To detect redundant calls

        public UnitOfWork(
            HorariosContext horariosContext,
            IAulasRepository aulas,
            ICarrerasRepository carreras,
            IDisponibilidadProfesorRepository disponibilidadProfesor,
            IHorarioProfesorRepository horarioProfesor,
            IMateriasRepository materias,
            IPeriodoCarreraRepository periodoCarrera,
            IPrioridadesRepository prioridades,
            IPrivilegiosRepository privilegios,
            IProfesorMateriaRepository profesorMateria,
            IProfesorRepository profesor,
            ISeccionesRepository secciones,
            ISemestresRepository semestres,
            IUsuarioRepository usuario,
            ITipoAulaMateriaRepository tipoAulaMateria,
            ITokenRepository token)
        {
            _horariosContext = horariosContext;
            AulasRepository = aulas;
            CarrerasRepository = carreras;
            DisponibilidadProfesorRepository = disponibilidadProfesor;
            HorarioProfesorRepository = horarioProfesor;
            MateriasRepository = materias;
            PeriodoCarreraRepository = periodoCarrera;
            PrioridadesRepository = prioridades;
            PrivilegiosRepository = privilegios;
            ProfesorMateriaRepository = profesorMateria;
            ProfesorRepository = profesor;
            SeccionesRepository = secciones;
            SemestresRepository = semestres;
            UsuarioRepository = usuario;
            TipoAulaMateriaRepository = tipoAulaMateria;
            TokenRepository = token;
        }

        /// <summary>
        /// Guarda todos los cambios realizados en el contexto actual
        /// </summary>
        /// <returns>Devuelve True en caso de exito</returns>
        public bool Save()
        {
            bool result = false;
            try
            {
                result = _horariosContext.SaveChanges() > 0 ? true : false;
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                Console.WriteLine(e.Message);
            }
            return result;
        }

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _horariosContext.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}