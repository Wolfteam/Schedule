namespace Schedule.Web.Models.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Constantes
        private const string _apiAccount = "api/Account";
        private const string _apiAulas = "api/Aulas";
        private const string _apiCarreras = "api/Carreras";
        private const string _apiDisponibilidad = "api/Disponibilidad";
        private const string _apiHorarioProfesor = "api/HorarioProfesor";
        private const string _apiMaterias = "api/Materias";
        private const string _apiPeriodoCarrera = "api/PeriodoCarrera";
        private const string _apiPrioridades = "api/Prioridades";
        private const string _apiPrivilegios = "api/Privilegios";
        private const string _apiProfesorMateria = "api/ProfesorMateria";
        private const string _apiProfesor = "api/Profesor";
        private const string _apiSecciones = "api/Secciones";
        private const string _apiSemestres = "api/Semestres";
        #endregion

        #region  Propiedades
        public IAulasRepository AulasRepository { get; private set; }
        public ICarrerasRepository CarrerasRepository { get; private set; }
        public IDisponibilidadProfesorRepository DisponibilidadRepository { get; private set; }
        public IHorarioProfesorRepository HorarioProfesorRepository { get; private set; }
        public IMateriasRepository MateriasRepository { get; private set; }
        public IPeriodoCarreraRepository PeriodoCarreraRepository { get; private set; }
        public IPrioridadesRepository PrioridadesRepository { get; private set; }
        public IPrivilegiosRepository PrivilegiosRepository { get; set; }
        public IProfesorMateriaRepository ProfesorMateriaRepository { get; private set; }
        public IProfesorRepository ProfesorRepository { get; private set; }
        public ISeccionesRepository SeccionesRepository { get; private set; }
        public ISemestresRepository SemestresRepository { get; private set; }
        public IUsuarioRepository UsuarioRepository { get; private set; }

        public string Token
        {
            set
            {
                AulasRepository.Token = value;
                CarrerasRepository.Token = value;
                DisponibilidadRepository.Token = value;
                HorarioProfesorRepository.Token = value;
                MateriasRepository.Token = value;
                PeriodoCarreraRepository.Token = value;
                PrioridadesRepository.Token = value;
                PrivilegiosRepository.Token = value;
                ProfesorMateriaRepository.Token = value;
                ProfesorRepository.Token = value;
                SeccionesRepository.Token = value;
                SemestresRepository.Token = value;
                UsuarioRepository.Token = value;
            }
        }
        #endregion

        public UnitOfWork(IHttpClientsFactory httpClientsFactory)
        {
            AulasRepository = new AulasRepository(httpClientsFactory, _apiAulas);
            CarrerasRepository = new CarrerasRepository(httpClientsFactory, _apiCarreras);
            DisponibilidadRepository = new DisponibilidadProfesorRepository(httpClientsFactory, _apiDisponibilidad);
            HorarioProfesorRepository = new HorarioProfesorRepository(httpClientsFactory, _apiHorarioProfesor);
            MateriasRepository = new MateriasRepository(httpClientsFactory, _apiMaterias);
            PeriodoCarreraRepository = new PeriodoCarreraRepository(httpClientsFactory, _apiPeriodoCarrera);
            PrioridadesRepository = new PrioridadesRepository(httpClientsFactory, _apiPrioridades);
            PrivilegiosRepository = new PrivilegiosRepository(httpClientsFactory, _apiPrivilegios);
            ProfesorMateriaRepository = new ProfesorMateriaRepository(httpClientsFactory, _apiProfesorMateria);
            ProfesorRepository = new ProfesorRepository(httpClientsFactory, _apiProfesor);
            SeccionesRepository = new SeccionesRepository(httpClientsFactory, _apiSecciones);
            SemestresRepository = new SemestresRepository(httpClientsFactory, _apiSemestres);
            UsuarioRepository = new UsuarioRepository(httpClientsFactory, _apiAccount);
        }
    }
}