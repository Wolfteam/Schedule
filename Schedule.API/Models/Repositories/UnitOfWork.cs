using System;

namespace Schedule.API.Models.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Propiedades
        public AulasRepository AulasRepository { get; private set; }
        public CarrerasRepository CarrerasRepository { get; private set; }
        public DisponibilidadProfesorRepository DisponibilidadProfesorRepository { get; private set; }
        public HorarioProfesorRepository HorarioProfesorRepository { get; private set; }
        public MateriasRepository MateriasRepository { get; private set; }
        public PeriodoCarreraRepository PeriodoCarreraRepository { get; private set; }
        public PrioridadesRepository PrioridadesRepository { get; private set; }
        public ProfesorMateriaRepository ProfesorMateriaRepository { get; private set; }
        public ProfesorRepository ProfesorRepository { get; private set; }
        public SeccionesRepository SeccionesRepository { get; private set; }
        public SemestresRepository SemestresRepository { get; private set; }
        public UsuarioRepository UsuarioRepository { get; private set; }
        public TokenRepository TokenRepository { get; private set; }
        #endregion

        private readonly HorariosContext _horariosContext;
        private bool disposedValue = false; // To detect redundant calls

        public UnitOfWork()
        {
            _horariosContext = new HorariosContext();
            AulasRepository = new AulasRepository(_horariosContext);
            CarrerasRepository = new CarrerasRepository(_horariosContext);
            DisponibilidadProfesorRepository = new DisponibilidadProfesorRepository(_horariosContext);
            HorarioProfesorRepository = new HorarioProfesorRepository(_horariosContext);
            MateriasRepository = new MateriasRepository(_horariosContext);
            PeriodoCarreraRepository = new PeriodoCarreraRepository(_horariosContext);
            PrioridadesRepository = new PrioridadesRepository(_horariosContext);
            ProfesorMateriaRepository = new ProfesorMateriaRepository(_horariosContext);
            ProfesorRepository = new ProfesorRepository(_horariosContext);
            SeccionesRepository = new SeccionesRepository(_horariosContext);
            SemestresRepository = new SemestresRepository(_horariosContext);
            UsuarioRepository = new UsuarioRepository(_horariosContext);
            TokenRepository = new TokenRepository(_horariosContext);
        }

        public UnitOfWork(HorariosContext horariosContext)
        {
            //TODO: Terminar este
            _horariosContext = horariosContext;
            AulasRepository = new AulasRepository(horariosContext);
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
                result = _horariosContext.SaveChanges() > 0 ? true: false;
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